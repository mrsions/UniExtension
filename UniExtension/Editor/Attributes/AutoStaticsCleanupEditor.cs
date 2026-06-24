#if !UNITY_6000_5_OR_NEWER
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Unity.Scripting.LifecycleManagement;

namespace Packages.UniExtension.Editor.Attributes
{
    [InitializeOnLoad]
    internal static class AutoStaticsCleanupEditor
    {
        private static readonly System.Collections.Generic.Dictionary<FieldInfo, object> _initialFieldValues = new System.Collections.Generic.Dictionary<FieldInfo, object>();
        private static readonly System.Collections.Generic.Dictionary<PropertyInfo, object> _initialPropertyValues = new System.Collections.Generic.Dictionary<PropertyInfo, object>();

        static AutoStaticsCleanupEditor()
        {
            CacheInitialValues();
        }

        private static void CacheInitialValues()
        {
            _initialFieldValues.Clear();
            var fields = TypeCache.GetFieldsWithAttribute<AutoStaticsCleanupAttribute>();
            foreach (var field in fields)
            {
                if (!field.IsStatic) continue;
                if (!CanUseLateBoundReflection(field)) continue;

                try
                {
                    _initialFieldValues[field] = CloneObject(field.GetValue(null));
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"[AutoStaticsCleanup] Failed to cache initial value for field {field.DeclaringType?.Name}.{field.Name}: {e.Message}");
                }
            }

            _initialPropertyValues.Clear();
#if UNITY_6000_8_OR_NEWER
            var assemblies = UnityEngine.Assemblies.CurrentAssemblies.GetLoadedAssemblies()
                .Where(a => !a.FullName.StartsWith("Unity") && !a.FullName.StartsWith("System") && !a.FullName.StartsWith("mscorlib"));
#else
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.FullName.StartsWith("Unity") && !a.FullName.StartsWith("System") && !a.FullName.StartsWith("mscorlib"));
#endif
            foreach (var assembly in assemblies)
            {
                Type[] types;
                try { types = assembly.GetTypes(); }
                catch (ReflectionTypeLoadException e) { types = e.Types.Where(t => t != null).ToArray(); }

                foreach (var type in types)
                {
                    var props = type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    foreach (var prop in props)
                    {
                        if (prop.GetCustomAttribute<AutoStaticsCleanupAttribute>() != null && prop.CanRead)
                        {
                            if (!CanUseLateBoundReflection(prop)) continue;

                            try
                            {
                                _initialPropertyValues[prop] = CloneObject(prop.GetValue(null));
                            }
                            catch (Exception e)
                            {
                                Debug.LogWarning($"[AutoStaticsCleanup] Failed to cache initial value for property {type.Name}.{prop.Name}: {e.Message}");
                            }
                        }
                    }
                }
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration()
        {
            CleanupFields();
            CleanupProperties();
        }

        [InitializeOnLoadMethod]
        private static void RegisterUnloading()
        {
            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
            AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
        }

        private static void OnBeforeAssemblyReload()
        {
            var methods = TypeCache.GetMethodsWithAttribute<BeforeCodeUnloadingAttribute>();
            foreach (var method in methods)
            {
                if (!method.IsStatic) continue;
                
                try
                {
                    method.Invoke(null, null);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        private static void OnAfterAssemblyReload()
        {
            var methods = TypeCache.GetMethodsWithAttribute<AfterCodeReloadSerializationAttribute>();
            foreach (var method in methods)
            {
                if (!method.IsStatic) continue;
                
                try
                {
                    method.Invoke(null, null);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        private static object CloneObject(object obj)
        {
            if (obj == null) return null;
            Type type = obj.GetType();

            if (type.IsValueType || obj is string) return obj;

            if (obj is Array arr)
            {
                return arr.Clone();
            }

            if (obj is ICloneable cloneable)
            {
                return cloneable.Clone();
            }

            // Attempt to use a copy constructor (e.g., List(IEnumerable<T>), Dictionary(IDictionary<K,V>))
            try
            {
                return Activator.CreateInstance(type, obj);
            }
            catch
            {
                // Ignore and fallback
            }

            // Fallback for [Serializable] classes using Unity's JSON utility
            try
            {
                if (Attribute.IsDefined(type, typeof(SerializableAttribute)))
                {
                    var json = EditorJsonUtility.ToJson(obj);
                    if (!string.IsNullOrEmpty(json) && json != "{}")
                    {
                        var clone = Activator.CreateInstance(type);
                        EditorJsonUtility.FromJsonOverwrite(json, clone);
                        return clone;
                    }
                }
            }
            catch
            {
                // Ignore and fallback
            }

            return obj;
        }

        private static void CleanupFields()
        {
            var fields = TypeCache.GetFieldsWithAttribute<AutoStaticsCleanupAttribute>();
            foreach (var field in fields)
            {
                if (!field.IsStatic) continue;
                if (!CanUseLateBoundReflection(field)) continue;

                try
                {
                    object defaultValue = _initialFieldValues.TryGetValue(field, out var val) ? CloneObject(val) : (field.FieldType.IsValueType ? Activator.CreateInstance(field.FieldType) : null);
                    field.SetValue(null, defaultValue);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        private static void CleanupProperties()
        {
#if UNITY_6000_8_OR_NEWER
            var assemblies = UnityEngine.Assemblies.CurrentAssemblies.GetLoadedAssemblies()
                .Where(a => !a.FullName.StartsWith("Unity") && !a.FullName.StartsWith("System") && !a.FullName.StartsWith("mscorlib"));
#else
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.FullName.StartsWith("Unity") && !a.FullName.StartsWith("System") && !a.FullName.StartsWith("mscorlib"));
#endif
            foreach (var assembly in assemblies)
            {
                Type[] types;
                try { types = assembly.GetTypes(); }
                catch (ReflectionTypeLoadException e) { types = e.Types.Where(t => t != null).ToArray(); }

                foreach (var type in types)
                {
                    var props = type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    foreach (var prop in props)
                    {
                        if (prop.GetCustomAttribute<AutoStaticsCleanupAttribute>() != null)
                        {
                            if (!prop.CanWrite) continue;
                            if (!CanUseLateBoundReflection(prop)) continue;

                            try
                            {
                                object defaultValue = _initialPropertyValues.TryGetValue(prop, out var val) ? CloneObject(val) : (prop.PropertyType.IsValueType ? Activator.CreateInstance(prop.PropertyType) : null);
                                prop.SetValue(null, defaultValue);
                            }
                            catch (Exception e)
                            {
                                Debug.LogException(e);
                            }
                        }
                    }
                }
            }
        }

        private static bool CanUseLateBoundReflection(FieldInfo field)
        {
            return field.DeclaringType != null
                && !field.DeclaringType.ContainsGenericParameters
                && !field.FieldType.ContainsGenericParameters;
        }

        private static bool CanUseLateBoundReflection(PropertyInfo property)
        {
            return property.DeclaringType != null
                && !property.DeclaringType.ContainsGenericParameters
                && !property.PropertyType.ContainsGenericParameters;
        }
    }
}
#endif
