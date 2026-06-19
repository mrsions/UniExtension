#nullable enable
//#line hidden

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace IDaNote.Utils
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidationAttribute : Attribute
    {
    }
}