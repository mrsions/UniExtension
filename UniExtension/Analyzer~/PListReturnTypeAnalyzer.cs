using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace UniExtension.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class PListReturnTypeAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "UEA0001";
    public const string DisposeDiagnosticId = "UEA0002";

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        "PList return type must be explicit",
        "'{0}' returns a pooled list. Declare the return type as '{1}' instead of '{2}'.",
        "Usage",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Methods that return PList<T> should declare PList<T> explicitly so callers know the result must be disposed.");

    private static readonly DiagnosticDescriptor DisposeRule = new(
        DisposeDiagnosticId,
        "PList rental must use using",
        "'{0}' is pooled. Use 'using var' so it is disposed and returned to the pool.",
        "Usage",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "PList.Take() returns a pooled collection that should be disposed with using var.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule, DisposeRule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterOperationAction(AnalyzeReturn, OperationKind.Return);
        context.RegisterOperationAction(AnalyzeInvocation, OperationKind.Invocation);
    }

    private static void AnalyzeReturn(OperationAnalysisContext context)
    {
        if (context.Operation is not IReturnOperation returnOperation || returnOperation.ReturnedValue is null)
        {
            return;
        }

        var returnedValue = UnwrapConversions(returnOperation.ReturnedValue);
        var returnedType = returnedValue.Type as INamedTypeSymbol;
        if (!IsPooledList(returnedType))
        {
            return;
        }

        if (!TryGetDeclaredReturnType(context.ContainingSymbol, out var declaredReturnType))
        {
            return;
        }

        if (SymbolEqualityComparer.Default.Equals(returnedType, declaredReturnType))
        {
            return;
        }

        if (IsPooledList(declaredReturnType))
        {
            return;
        }

        var diagnostic = Diagnostic.Create(
            Rule,
            returnedValue.Syntax.GetLocation(),
            context.ContainingSymbol.Name,
            returnedType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
            declaredReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));

        context.ReportDiagnostic(diagnostic);
    }

    private static void AnalyzeInvocation(OperationAnalysisContext context)
    {
        if (context.Operation is not IInvocationOperation invocation)
        {
            return;
        }

        if (!IsPListTake(invocation.TargetMethod))
        {
            return;
        }

        var localDeclaration = invocation.Syntax.FirstAncestorOrSelf<LocalDeclarationStatementSyntax>();
        if (localDeclaration is null)
        {
            return;
        }

        var declarator = localDeclaration.Declaration.Variables.FirstOrDefault(v => v.Initializer?.Value == invocation.Syntax);
        if (declarator is null)
        {
            return;
        }

        if (!localDeclaration.UsingKeyword.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.None))
        {
            return;
        }

        if (IsReturnedLocal(declarator))
        {
            return;
        }

        var pooledType = invocation.Type?.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat) ?? "PList<T>";
        context.ReportDiagnostic(Diagnostic.Create(DisposeRule, invocation.Syntax.GetLocation(), pooledType));
    }

    private static IOperation UnwrapConversions(IOperation operation)
    {
        var current = operation;
        while (current is IConversionOperation conversion)
        {
            current = conversion.Operand;
        }

        return current;
    }

    private static bool TryGetDeclaredReturnType(ISymbol symbol, out INamedTypeSymbol declaredReturnType)
    {
        switch (symbol)
        {
            case IMethodSymbol methodSymbol when methodSymbol.MethodKind != MethodKind.Constructor:
                declaredReturnType = methodSymbol.ReturnType as INamedTypeSymbol;
                return declaredReturnType is not null;

            case IPropertySymbol propertySymbol:
                declaredReturnType = propertySymbol.Type as INamedTypeSymbol;
                return declaredReturnType is not null;

            case IMethodSymbol { AssociatedSymbol: IPropertySymbol associatedProperty }:
                declaredReturnType = associatedProperty.Type as INamedTypeSymbol;
                return declaredReturnType is not null;

            default:
                declaredReturnType = null!;
                return false;
        }
    }

    private static bool IsPooledList(INamedTypeSymbol? type)
    {
        if (type is null)
        {
            return false;
        }

        var originalDefinition = type.OriginalDefinition;
        return originalDefinition.ToDisplayString() is "UniExtension.Collections.PList<T>" or "SLib.Collections.PList<T>";
    }

    private static bool IsPListTake(IMethodSymbol method)
    {
        if (method.Name != "Take")
        {
            return false;
        }

        return method.ContainingType.ToDisplayString() is "UniExtension.Collections.PList" or "SLib.Collections.PList";
    }

    private static bool IsReturnedLocal(VariableDeclaratorSyntax declarator)
    {
        var enclosingBody = declarator.FirstAncestorOrSelf<BaseMethodDeclarationSyntax>()?.Body
            ?? declarator.FirstAncestorOrSelf<AccessorDeclarationSyntax>()?.Body;
        if (enclosingBody is null)
        {
            return false;
        }

        var localName = declarator.Identifier.ValueText;
        return enclosingBody.DescendantNodes()
            .OfType<ReturnStatementSyntax>()
            .Any(r => r.Expression is IdentifierNameSyntax id && id.Identifier.ValueText == localName);
    }
}
