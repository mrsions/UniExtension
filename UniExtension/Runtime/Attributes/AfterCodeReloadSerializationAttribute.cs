#if !UNITY_6000_5_OR_NEWER
using System;

namespace Unity.Scripting.LifecycleManagement
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class AfterCodeReloadSerializationAttribute : Attribute
    {
    }
}
#endif
