#nullable enable
#line hidden

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using UnityEngine.Assertions.Comparers;

using Object = UnityEngine.Object;

namespace UniExtension
{
/// <summary>
/// Assertion 유틸리티 클래스
/// </summary>
/// <remarks>
/// 이 클래스는 다양한 조건 검사를 수행하고, 어설션 실패 시 적절한 메시지와 함께 예외를 던집니다.
/// 디버깅과 유효성 검사를 위해 사용되며, 어설션 조건에 따라 다양한 메시지를 제공합니다.
/// 주요 기능:
/// - Boolean 조건 검사(IsTrue, IsFalse)
/// - 객체의 동일성 및 비동일성 검사(AreEqual, AreNotEqual)
/// - Null 및 비Null 검사(IsNull, IsNotNull)
/// - 값의 범위 검사(IsRange, IsZero, IsNotZero)
/// - 컬렉션의 비어있음 및 비어있지 않음 검사(IsEmpty, IsNotEmpty)
/// - 예상 값과 실제 값의 근사치 검사(AreApproximatelyEqual, AreNotApproximatelyEqual)
/// - 디버깅 시 상세 메시지 제공을 위한 AssertionMessageUtil 클래스
/// </remarks>
public static class Assert
{
    internal class AssertionMessageUtil
    {
        private const string k_Expected = "Expected:";

        private const string k_AssertionFailed = "Assertion failure.";

        public static string GetMessage(string failureMessage)
        {
            return string.Format("{0} {1}", "Assertion failure.", failureMessage);
        }

        public static string GetMessage(string failureMessage, string expected)
        {
            return GetMessage(string.Format("{0}{1}{2} {3}", failureMessage, Environment.NewLine, "Expected:", expected));
        }

        public static string GetEqualityMessage(object? actual, object? expected, bool expectEqual)
        {
            return GetMessage(string.Format("Values are {0}equal.", expectEqual ? "not " : ""), string.Format("{0} {2} {1}", actual, expected, expectEqual ? "==" : "!="));
        }

        public static string NullFailureMessage(object? value, bool expectNull)
        {
            return GetMessage(string.Format("Value was {0}Null", expectNull ? "not " : ""), string.Format("Value was {0}Null", expectNull ? "" : "not "));
        }

        public static string NullFailureMessage(object? value, string? valueName, bool expectNull)
        {
            string msg = $"{valueName} Value was {(expectNull ? "not " : "")}Null";
            return GetMessage(msg, msg);
        }

        public static string GetNotInRangeMessage(object actual, object min, object max)
        {
            return GetMessage($"Value({actual}) not contains {min}~{max}.");
        }

        public static string BooleanFailureMessage(bool expected)
        {
            return GetMessage("Value was " + !expected, expected.ToString());
        }

        public static string GetIsNotZeroMessage(object actual)
        {
            return GetMessage($"Value({actual}) is not 0.");
        }

        public static string GetIsZeroMessage(object actual)
        {
            return GetMessage($"Value({actual}) is 0.");
        }

        public static string GetEmptyMessage(object actual)
        {
            return GetMessage($"Value({actual}) is empty.");
        }

        public static string GetNotEmptyMessage(object actual)
        {
            return GetMessage($"Value({actual}) is not empty.");
        }
    }

    internal const string UNITY_ASSERTIONS = "UNITY_ASSERTIONS";

    [DoesNotReturn]
    private static void Fail(string? message, string? userMessage)
    {
        //if (!raiseExceptions)
        //{
        //    if (message == null)
        //    {
        //        message = "Assertion has failed\n";
        //    }

        //    if (userMessage != null)
        //    {
        //        message = userMessage + "\n" + message;
        //    }

        //    Debug.LogAssertion(message);
        //    return;
        //}

        throw new UnityEngine.Assertions.AssertionException(message, userMessage);
    }

    //
    // 요약:
    //     Asserts that the condition is true.
    //
    // 매개 변수:
    //   message:
    //     The string used to describe the Assert.
    //
    //   condition:
    //     true or false.
    [Conditional(UNITY_ASSERTIONS)]
    public static void IsTrue([DoesNotReturnIf(false)] bool condition)
    {
        if (!condition)
        {
            IsTrue(condition, null);
        }
    }

    //
    // 요약:
    //     Asserts that the condition is true.
    //
    // 매개 변수:
    //   message:
    //     The string used to describe the Assert.
    //
    //   condition:
    //     true or false.
    [Conditional(UNITY_ASSERTIONS)]
    public static void IsTrue([DoesNotReturnIf(false)] bool condition, string? message)
    {
        if (!condition)
        {
            Fail(AssertionMessageUtil.BooleanFailureMessage(expected: true), message);
        }
    }

    //
    // 요약:
    //     Return true when the condition is false. Otherwise return false.
    //
    // 매개 변수:
    //   condition:
    //     true or false.
    //
    //   message:
    //     The string used to describe the result of the Assert.
    [Conditional(UNITY_ASSERTIONS)]
    public static void IsFalse([DoesNotReturnIf(true)] bool condition)
    {
        if (condition)
        {
            IsFalse(condition, null);
        }
    }

    //
    // 요약:
    //     Return true when the condition is false. Otherwise return false.
    //
    // 매개 변수:
    //   condition:
    //     true or false.
    //
    //   message:
    //     The string used to describe the result of the Assert.
    [Conditional(UNITY_ASSERTIONS)]
    public static void IsFalse([DoesNotReturnIf(true)] bool condition, string? message)
    {
        if (condition)
        {
            Fail(AssertionMessageUtil.BooleanFailureMessage(expected: false), message);
        }
    }

    //
    // 요약:
    //     Assert the values are approximately equal.
    //
    // 매개 변수:
    //   tolerance:
    //     Tolerance of approximation.
    //
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreApproximatelyEqual(float expected, float actual)
    {
        AreEqual(expected, actual, null, FloatComparer.s_ComparerWithDefaultTolerance);
    }

    //
    // 요약:
    //     Assert the values are approximately equal.
    //
    // 매개 변수:
    //   tolerance:
    //     Tolerance of approximation.
    //
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreApproximatelyEqual(float expected, float actual, string? message)
    {
        AreEqual(expected, actual, message, FloatComparer.s_ComparerWithDefaultTolerance);
    }

    //
    // 요약:
    //     Assert the values are approximately equal.
    //
    // 매개 변수:
    //   tolerance:
    //     Tolerance of approximation.
    //
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreApproximatelyEqual(float expected, float actual, float tolerance)
    {
        AreApproximatelyEqual(expected, actual, tolerance, null);
    }

    //
    // 요약:
    //     Assert the values are approximately equal.
    //
    // 매개 변수:
    //   tolerance:
    //     Tolerance of approximation.
    //
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreApproximatelyEqual(float expected, float actual, float tolerance, string? message)
    {
        AreEqual(expected, actual, message, new FloatComparer(tolerance));
    }

    //
    // 요약:
    //     Asserts that the values are approximately not equal.
    //
    // 매개 변수:
    //   tolerance:
    //     Tolerance of approximation.
    //
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotApproximatelyEqual(float expected, float actual)
    {
        AreNotEqual(expected, actual, null, FloatComparer.s_ComparerWithDefaultTolerance);
    }

    //
    // 요약:
    //     Asserts that the values are approximately not equal.
    //
    // 매개 변수:
    //   tolerance:
    //     Tolerance of approximation.
    //
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotApproximatelyEqual(float expected, float actual, string? message)
    {
        AreNotEqual(expected, actual, message, FloatComparer.s_ComparerWithDefaultTolerance);
    }

    //
    // 요약:
    //     Asserts that the values are approximately not equal.
    //
    // 매개 변수:
    //   tolerance:
    //     Tolerance of approximation.
    //
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotApproximatelyEqual(float expected, float actual, float tolerance)
    {
        AreNotApproximatelyEqual(expected, actual, tolerance, null);
    }

    //
    // 요약:
    //     Asserts that the values are approximately not equal.
    //
    // 매개 변수:
    //   tolerance:
    //     Tolerance of approximation.
    //
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotApproximatelyEqual(float expected, float actual, float tolerance, string? message)
    {
        AreNotEqual(expected, actual, message, new FloatComparer(tolerance));
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual<T>(T expected, T actual)
    {
        AreEqual(expected, actual, null);
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual<T>(T expected, T actual, string? message)
    {
        AreEqual(expected, actual, message, EqualityComparer<T>.Default);
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual<T>(T expected, T actual, string? message, IEqualityComparer<T> comparer)
    {
        if (typeof(Object).IsAssignableFrom(typeof(T)))
        {
            AreEqual(expected as Object, actual as Object, message);
        }
        else if (!comparer.Equals(actual, expected))
        {
            Fail(AssertionMessageUtil.GetEqualityMessage(actual, expected, expectEqual: true), message);
        }
    }

    //
    // 요약:
    //     Assert that the values are equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual(Object? expected, Object? actual, string? message)
    {
        if (actual != expected)
        {
            Fail(AssertionMessageUtil.GetEqualityMessage(actual, expected, expectEqual: true), message);
        }
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual<T>(T expected, T actual)
    {
        AreNotEqual(expected, actual, null);
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual<T>(T expected, T actual, string? message)
    {
        AreNotEqual(expected, actual, message, EqualityComparer<T>.Default);
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual<T>(T expected, T actual, string? message, IEqualityComparer<T> comparer)
    {
        if (typeof(Object).IsAssignableFrom(typeof(T)))
        {
            AreNotEqual(expected as Object, actual as Object, message);
        }
        else if (comparer.Equals(actual, expected))
        {
            Fail(AssertionMessageUtil.GetEqualityMessage(actual, expected, expectEqual: false), message);
        }
    }

    //
    // 요약:
    //     Assert that the values are not equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual(Object? expected, Object? actual, string? message)
    {
        if (actual == expected)
        {
            Fail(AssertionMessageUtil.GetEqualityMessage(actual, expected, expectEqual: false), message);
        }
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void IsNull<T>([MaybeNull] T? value) where T : class
    {
        IsNull(value, null);
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void IsNull<T>([MaybeNull] T? value, string? message) where T : class
    {
        if (typeof(Object).IsAssignableFrom(typeof(T)))
        {
            IsNull(value as Object, message);
        }
        else if (value != null)
        {
            Fail(AssertionMessageUtil.NullFailureMessage(value, expectNull: true), message);
        }
    }

    //
    // 요약:
    //     Assert the value is null.
    //
    // 매개 변수:
    //   value:
    //     The Object or type being checked for.
    //
    //   message:
    //     The string used to describe the Assert.
    [Conditional(UNITY_ASSERTIONS)]
    public static void IsNull([MaybeNull] Object? value, string? message)
    {
        if (value != null)
        {
            Fail(AssertionMessageUtil.NullFailureMessage(value, expectNull: true), message);
        }
    }

    //[Conditional(UNITY_ASSERTIONS)]
    //public static void IsNotNull<T>([NotNull] T? value) where T : class
    //{
    //    IsNotNull(value, null);
    //}

    [Conditional(UNITY_ASSERTIONS)]
    public static void IsNotNullOrWhiteSpace([NotNull] string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            Fail(AssertionMessageUtil.NullFailureMessage(value, "Not null or whitespace", expectNull: false), "Not null or whitespace");
        }
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void IsNotNull<T>([NotNull] T? value, [CallerArgumentExpression("value")] string? valueName = default) where T : class
    {
        if (typeof(Object).IsAssignableFrom(typeof(T)))
        {
            IsNotNull(value as Object, valueName, valueName);
        }
        else if (value == null)
        {
            Fail(AssertionMessageUtil.NullFailureMessage(value, valueName, expectNull: false), valueName);
        }
    }

    //
    // 요약:
    //     Assert that the value is not null.
    //
    // 매개 변수:
    //   value:
    //     The Object or type being checked for.
    //
    //   message:
    //     The string used to describe the Assert.
    [Conditional(UNITY_ASSERTIONS)]
    public static void IsNotNull([NotNull] Object? value, string? message, [CallerArgumentExpression("value")] string? valueName = default)
    {
        if (value == null || (value is UnityEngine.Object uobj && !uobj))
        {
            Fail(AssertionMessageUtil.NullFailureMessage(value, valueName, expectNull: false), message);
        }
    }

    //
    // 요약:
    //     Assert that the values are equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual(sbyte expected, sbyte actual)
    {
        if (expected != actual)
        {
            Assert.AreEqual<sbyte>(expected, actual, (string?)null);
        }
    }

    //
    // 요약:
    //     Assert that the values are equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual(sbyte expected, sbyte actual, string? message)
    {
        if (expected != actual)
        {
            Assert.AreEqual<sbyte>(expected, actual, message);
        }
    }

    //
    // 요약:
    //     Assert that the values are not equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual(sbyte expected, sbyte actual)
    {
        if (expected == actual)
        {
            Assert.AreNotEqual<sbyte>(expected, actual, (string?)null);
        }
    }

    //
    // 요약:
    //     Assert that the values are not equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual(sbyte expected, sbyte actual, string? message)
    {
        if (expected == actual)
        {
            Assert.AreNotEqual<sbyte>(expected, actual, message);
        }
    }

    //
    // 요약:
    //     Assert that the values are equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual(byte expected, byte actual)
    {
        if (expected != actual)
        {
            Assert.AreEqual<byte>(expected, actual, (string?)null);
        }
    }

    //
    // 요약:
    //     Assert that the values are equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual(byte expected, byte actual, string? message)
    {
        if (expected != actual)
        {
            Assert.AreEqual<byte>(expected, actual, message);
        }
    }

    //
    // 요약:
    //     Assert that the values are not equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual(byte expected, byte actual)
    {
        if (expected == actual)
        {
            Assert.AreNotEqual<byte>(expected, actual, (string?)null);
        }
    }

    //
    // 요약:
    //     Assert that the values are not equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual(byte expected, byte actual, string? message)
    {
        if (expected == actual)
        {
            Assert.AreNotEqual<byte>(expected, actual, message);
        }
    }

    //
    // 요약:
    //     Assert that the values are equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual(char expected, char actual)
    {
        if (expected != actual)
        {
            Assert.AreEqual<char>(expected, actual, (string?)null);
        }
    }

    //
    // 요약:
    //     Assert that the values are equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual(char expected, char actual, string? message)
    {
        if (expected != actual)
        {
            Assert.AreEqual<char>(expected, actual, message);
        }
    }

    //
    // 요약:
    //     Assert that the values are not equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual(char expected, char actual)
    {
        if (expected == actual)
        {
            Assert.AreNotEqual<char>(expected, actual, (string?)null);
        }
    }

    //
    // 요약:
    //     Assert that the values are not equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual(char expected, char actual, string? message)
    {
        if (expected == actual)
        {
            Assert.AreNotEqual<char>(expected, actual, message);
        }
    }

    //
    // 요약:
    //     Assert that the values are equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual(short expected, short actual)
    {
        if (expected != actual)
        {
            Assert.AreEqual<short>(expected, actual, (string?)null);
        }
    }

    //
    // 요약:
    //     Assert that the values are equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual(short expected, short actual, string? message)
    {
        if (expected != actual)
        {
            Assert.AreEqual<short>(expected, actual, message);
        }
    }

    //
    // 요약:
    //     Assert that the values are not equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual(short expected, short actual)
    {
        if (expected == actual)
        {
            Assert.AreNotEqual<short>(expected, actual, (string?)null);
        }
    }

    //
    // 요약:
    //     Assert that the values are not equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual(short expected, short actual, string? message)
    {
        if (expected == actual)
        {
            Assert.AreNotEqual<short>(expected, actual, message);
        }
    }

    //
    // 요약:
    //     Assert that the values are equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual(ushort expected, ushort actual)
    {
        if (expected != actual)
        {
            Assert.AreEqual<ushort>(expected, actual, (string?)null);
        }
    }

    //
    // 요약:
    //     Assert that the values are equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual(ushort expected, ushort actual, string? message)
    {
        if (expected != actual)
        {
            Assert.AreEqual<ushort>(expected, actual, message);
        }
    }

    //
    // 요약:
    //     Assert that the values are not equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual(ushort expected, ushort actual)
    {
        if (expected == actual)
        {
            Assert.AreNotEqual<ushort>(expected, actual, (string?)null);
        }
    }

    //
    // 요약:
    //     Assert that the values are not equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual(ushort expected, ushort actual, string? message)
    {
        if (expected == actual)
        {
            Assert.AreNotEqual<ushort>(expected, actual, message);
        }
    }

    //
    // 요약:
    //     Assert that the values are equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual(int expected, int actual)
    {
        if (expected != actual)
        {
            Assert.AreEqual<int>(expected, actual, (string?)null);
        }
    }

    //
    // 요약:
    //     Assert that the values are equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual(int expected, int actual, string? message)
    {
        if (expected != actual)
        {
            Assert.AreEqual<int>(expected, actual, message);
        }
    }

    //
    // 요약:
    //     Assert that the values are not equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual(int expected, int actual)
    {
        if (expected == actual)
        {
            Assert.AreNotEqual<int>(expected, actual, (string?)null);
        }
    }

    //
    // 요약:
    //     Assert that the values are not equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual(int expected, int actual, string? message)
    {
        if (expected == actual)
        {
            Assert.AreNotEqual<int>(expected, actual, message);
        }
    }

    //
    // 요약:
    //     Assert that the values are equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual(uint expected, uint actual)
    {
        if (expected != actual)
        {
            Assert.AreEqual<uint>(expected, actual, (string?)null);
        }
    }

    //
    // 요약:
    //     Assert that the values are equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual(uint expected, uint actual, string? message)
    {
        if (expected != actual)
        {
            Assert.AreEqual<uint>(expected, actual, message);
        }
    }

    //
    // 요약:
    //     Assert that the values are not equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual(uint expected, uint actual)
    {
        if (expected == actual)
        {
            Assert.AreNotEqual<uint>(expected, actual, (string?)null);
        }
    }

    //
    // 요약:
    //     Assert that the values are not equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual(uint expected, uint actual, string? message)
    {
        if (expected == actual)
        {
            Assert.AreNotEqual<uint>(expected, actual, message);
        }
    }

    //
    // 요약:
    //     Assert that the values are equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual(long expected, long actual)
    {
        if (expected != actual)
        {
            Assert.AreEqual<long>(expected, actual, (string?)null);
        }
    }

    //
    // 요약:
    //     Assert that the values are equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual(long expected, long actual, string? message)
    {
        if (expected != actual)
        {
            Assert.AreEqual<long>(expected, actual, message);
        }
    }

    //
    // 요약:
    //     Assert that the values are not equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual(long expected, long actual)
    {
        if (expected == actual)
        {
            Assert.AreNotEqual<long>(expected, actual, (string?)null);
        }
    }

    //
    // 요약:
    //     Assert that the values are not equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual(long expected, long actual, string? message)
    {
        if (expected == actual)
        {
            Assert.AreNotEqual<long>(expected, actual, message);
        }
    }

    //
    // 요약:
    //     Assert that the values are equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual(ulong expected, ulong actual)
    {
        if (expected != actual)
        {
            Assert.AreEqual<ulong>(expected, actual, (string?)null);
        }
    }

    //
    // 요약:
    //     Assert that the values are equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreEqual(ulong expected, ulong actual, string? message)
    {
        if (expected != actual)
        {
            Assert.AreEqual<ulong>(expected, actual, message);
        }
    }

    //
    // 요약:
    //     Assert that the values are not equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual(ulong expected, ulong actual)
    {
        if (expected == actual)
        {
            Assert.AreNotEqual<ulong>(expected, actual, (string?)null);
        }
    }

    //
    // 요약:
    //     Assert that the values are not equal.
    //
    // 매개 변수:
    //   expected:
    //     The assumed Assert value.
    //
    //   actual:
    //     The exact Assert value.
    //
    //   message:
    //     The string used to describe the Assert.
    //
    //   comparer:
    //     Method to compare expected and actual arguments have the same value.
    [Conditional(UNITY_ASSERTIONS)]
    public static void AreNotEqual(ulong expected, ulong actual, string? message)
    {
        if (expected == actual)
        {
            Assert.AreNotEqual<ulong>(expected, actual, message);
        }
    }

    [Conditional(UNITY_ASSERTIONS)]
    [DoesNotReturn]
    public static void ThrowNotImplemented(string msg)
    {
        throw new NotImplementedException(msg);
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void HaveIndex(int value, int count, string? message = "")
    {
        if (value < 0 || value >= count)
        {
            Fail(AssertionMessageUtil.GetNotInRangeMessage(value, 0, count - 1), message);
        }
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void HaveIndex<T>(int value, IList<T> array, string? message = "")
    {
        HaveIndex(value, array.Count, message);
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void IsEmpty<T>(ICollection<T> array, string? message = "")
    {
        if (array.Count != 0)
        {
            Fail(AssertionMessageUtil.GetNotEmptyMessage(array), message);
        }
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void IsNotEmpty<T>(ICollection<T> array, string? message = "")
    {
        if (array.Count == 0)
        {
            Fail(AssertionMessageUtil.GetEmptyMessage(array), message);
        }
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void IsInt8(long value, string? message = "")
    {
        IsRange(value, sbyte.MinValue, sbyte.MaxValue, message);
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void IsInt16(long value, string? message = "")
    {
        IsRange(value, short.MinValue, short.MaxValue, message);
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void IsInt32(long value, string? message = "")
    {
        IsRange(value, int.MinValue, int.MaxValue, message);
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void IsUInt8(long value, string? message = "")
    {
        IsRange(value, byte.MinValue, byte.MaxValue, message);
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void IsUInt16(long value, string? message = "")
    {
        IsRange(value, ushort.MinValue, ushort.MaxValue, message);
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void IsUInt32(long value, string? message = "")
    {
        IsRange(value, uint.MinValue, uint.MaxValue, message);
    }

    /// <summary>
    /// min <= value <= max 값이어야한다.
    /// </summary>
    [Conditional(UNITY_ASSERTIONS)]
    public static void IsRange(long value, long min, long max, string? message = "")
    {
        if (value < min || max < value)
        {
            Fail(AssertionMessageUtil.GetNotInRangeMessage(value, min, max), message);
        }
    }

    /// <summary>
    /// min <= value <= max 값이어야한다.
    /// </summary>
    [Conditional(UNITY_ASSERTIONS)]
    public static void IsZero(long value, string? message = "")
    {
        if (value != 0)
        {
            Fail(AssertionMessageUtil.GetIsZeroMessage(value), message);
        }
    }
    [Conditional(UNITY_ASSERTIONS)]
    public static void IsZero(ulong value, string? message = "")
    {
        if (value != 0)
        {
            Fail(AssertionMessageUtil.GetIsZeroMessage(value), message);
        }
    }
    [Conditional(UNITY_ASSERTIONS)]
    public static void IsZero(decimal value, string? message = "")
    {
        if (value != 0)
        {
            Fail(AssertionMessageUtil.GetIsZeroMessage(value), message);
        }
    }

    /// <summary>
    /// min <= value <= max 값이어야한다.
    /// </summary>
    [Conditional(UNITY_ASSERTIONS)]
    public static void IsNotZero(long value, string? message = "")
    {
        if (value == 0)
        {
            Fail(AssertionMessageUtil.GetIsNotZeroMessage(value), message);
        }
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void IsNotZero(ulong value, string? message = "")
    {
        if (value == 0)
        {
            Fail(AssertionMessageUtil.GetIsNotZeroMessage(value), message);
        }
    }

    [Conditional(UNITY_ASSERTIONS)]
    public static void IsNotZero(decimal value, string? message = "")
    {
        if (value == 0)
        {
            Fail(AssertionMessageUtil.GetIsNotZeroMessage(value), message);
        }
    }
}
}
