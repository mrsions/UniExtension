using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System
{
public static class ArrayExtension
{
    #region Fill
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Fill<T>(this T[] array, T value)
    {
        Array.Fill(array, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Fill<T>(this T[] array, T value, int index, int length)
    {
        Array.Fill(array, value, index, length);
    }
    #endregion

    #region Reverse
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Reverse<T>(this T[] array)
    {
        Array.Reverse(array);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Reverse<T>(this T[] array, int index, int length)
    {
        Array.Reverse(array, index, length);
    }
    #endregion

    #region IndexOf
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<T>(this T[] array, T value)
    {
        return Array.IndexOf(array, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<T>(this T[] array, T value, int index)
    {
        return Array.IndexOf(array, value, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<T>(this T[] array, T value, int index, int length)
    {
        return Array.IndexOf(array, value, index, length);
    }
    #endregion

    #region LastIndexOf
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOf<T>(this T[] array, T value)
    {
        return Array.LastIndexOf(array, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOf<T>(this T[] array, T value, int startIndex)
    {
        return Array.LastIndexOf(array, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOf<T>(this T[] array, T value, int startIndex, int count)
    {
        return Array.LastIndexOf(array, value);
    }
    #endregion

    #region Sort
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Sort<T>(T[] array)
    {
        Sort(array);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Sort<T>(T[] array, int index, int length)
    {
        Sort(array, index, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Sort<T>(T[] array, int index, int length, IComparer<T> comparer)
    {
        Sort(array, index, length, comparer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Sort<T>(T[] array, IComparer<T> comparer)
    {
        Sort(array, comparer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Sort<T>(T[] array, Comparison<T> comparison)
    {
        Sort(array, comparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items)
    {
        Sort(keys, items);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, int index, int length)
    {
        Sort(keys, items, index, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, int index, int length, IComparer<TKey> comparer)
    {
        Sort(keys, items, index, length, comparer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, IComparer<TKey> comparer)
    {
        Sort(keys, items, comparer);
    }
    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains<T>(this T[] array, T value)
    {
        return Array.IndexOf(array, value) != -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBounds<T>(this T[] array, int index)
    {
        return 0 <= index && index < array.Length;
    }

    public static T[] CropRect<T>(this T[] array, int stride, int x, int y, int width, int height)
    {
        var result = new T[width * height];

        for (var yy = 0; yy < height; yy++)
        {
            var idx = (y + yy) * stride + x;
            Array.Copy(array, idx, result, yy * width, width);
        }

        return result;
    }

    public static bool StartsWith<T>(this T[] array, T[] array2)
        where T : IEquatable<T>
    {
        if (array.Length < array2.Length) return false;

        for (var i = 0; i < array2.Length; i++)
        {
            if (array[i].Equals(array[2]))
            {
                return false;
            }
        }

        return true;
    }

    public static T[] CloneArray<T>(this T[] array)
    {
        return (T[])array.Clone();
    }

    public static T[] CloneArray<T>(this T[] array, int offset, int length)
    {
        if (offset == 0 && length == array.Length)
        {
            return (T[])array.Clone();
        }
        else
        {
            var result = new T[length];
            Array.Copy(array, offset, result, 0, length);
            return result;
        }
    }

    public static byte[] CloneArray(this byte[] array, int offset, int length)
    {
        if (offset == 0 && length == array.Length)
        {
            return (byte[])array.Clone();
        }
        else
        {
            var result = new byte[length];
            Buffer.BlockCopy(array, offset, result, 0, length);
            return result;
        }
    }

    public static bool EqualsArray<T>(this T[] array, T[] target)
         where T : IEquatable<T>
    {
        for (int i = 0, iLen = array.Length; i < iLen; i++)
        {
            if (!array[i].Equals(target[i]))
            {
                return false;
            }
        }
        return true;
    }
}
}
