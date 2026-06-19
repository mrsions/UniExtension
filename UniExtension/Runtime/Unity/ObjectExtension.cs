#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using uobject = UnityEngine.Object;

namespace UnityEngine
{
public static class ObjectExtension
{
    /// <summary>
    /// 객체가 null이 아니고 파괴되지 않았다.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLive([NotNullWhen(true), MaybeNullWhen(false)] this object? obj)
    {
        return (obj is uobject uobj) ? (bool)uobj : obj != null;
    }

    /// <summary>
    /// 객체가 null이 아니고 파괴되지 않았다.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLive([NotNullWhen(true), MaybeNullWhen(false)] this uobject? obj)
    {
        return obj != null;
    }

    /// <summary>
    /// 객체가 파괴됐거나 null일때 true
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDead([NotNullWhen(false), MaybeNullWhen(true)] this object? obj)
    {
        return (obj is uobject uobj) ? (bool)uobj == false : obj == null;
    }

    /// <summary>
    /// 객체가 파괴됐거나 null일때 true
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDead([NotNullWhen(false), MaybeNullWhen(true)] this uobject? obj)
    {
        return (bool)obj == false;
    }

    /// <summary>
    /// 객체가 파괴됐을경우 진짜 null을 리턴한다.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? ToAccess<T>(this T? obj)
        where T : uobject
    {
        return (bool)obj ? obj : null;
    }

    /// <summary>
    /// 객체가 파괴됐을경우 진짜 null을 리턴한다.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryAccess<T>(this T? obj, [NotNullWhen(true)] out T? result)
        where T : uobject
    {
        if ((bool)obj)
        {
            result = obj;
            return true;
        }
        else
        {
            result = null;
            return false;
        }
    }
}
}
