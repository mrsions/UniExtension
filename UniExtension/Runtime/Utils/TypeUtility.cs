#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using uobject = UnityEngine.Object;

namespace UniExtension
{
public static class TypeUtility
{
    public static bool IsUnityType(this Type type)
    {
        return typeof(uobject).IsAssignableFrom(type);
    }

    public static bool IsUnityCollectionType(this Type type)
    {
        if (IsCollectionType(type))
        {
            var args = type.GetGenericArguments();
            if (args.Length == 1)
            {
                return IsUnityType(args[0]);
            }
        }
        return false;
    }

    private static bool IsCollectionType(Type type)
    {
        if (type.IsGenericType)
        {
            var definition = type.GetGenericTypeDefinition();
            foreach (var itype in definition.GetInterfaces())
            {
                if (itype == typeof(IList<>)
                    || itype == typeof(IEnumerable<>)
                    || itype == typeof(ICollection<>))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
}
