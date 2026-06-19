using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniExtension
{
public static class ArrayUtils
{
    public static void Add<T>(ref T[] array, T value)
    {
        Array.Resize(ref array, array.Length + 1);
        array[array.Length - 1] = value;
    }

    public static void AddRange<T>(ref T[] array, T[] values)
    {
        var index = array.Length;
        Array.Resize(ref array, array.Length + values.Length);
        Array.Copy(values, 0, array, index, values.Length);
    }

    public static void AddRange<T>(ref T[] array, IList<T> values)
    {
        var index = array.Length;
        var count = values.Count;
        Array.Resize(ref array, array.Length + count);
        for (int i = 0; i < count; i++)
        {
            T v = values[i];
            array[index++] = v;
        }
    }

    public static void AddRange<T>(ref T[] array, IEnumerable<T> values)
    {
        var index = array.Length;
        Array.Resize(ref array, array.Length + values.Count());
        foreach (var v in values)
        {
            array[index++] = v;
        }
    }

    public static void Insert<T>(ref T[] array, int index, T value)
    {
        var result = new T[array.Length + 1];
        Array.Copy(array, 0, result, 0, index);
        var len = array.Length - index;
        if (len > 0)
            Array.Copy(array, index, result, index + 1, len);
        result[index] = value;

    }

    public static void InsertRange<T>(ref T[] array, int index, T[] dest)
    {
        var result = new T[array.Length + dest.Length];
        Array.Copy(array, 0, result, 0, index);
        Array.Copy(dest, 0, result, index, dest.Length);
        var len = result.Length - dest.Length - index;
        if (len > 0)
            Array.Copy(array, index, result, index + dest.Length, len);
    }

    public static bool Remove<T>(ref T[] array, T target)
        where T : class
    {
        for (var i = 0; i < array.Length; i++)
        {
            if (array[i] == target)
            {
                RemoveAt(ref array, i);
                return true;
            }
        }
        return false;
    }

    public static void RemoveAt<T>(ref T[] array, int index)
    {
        var result = new T[array.Length - 1];
        Array.Copy(array, 0, result, 0, index);
        var len = array.Length - index - 1;
        if (len > 0)
            Array.Copy(array, index + 1, result, index, len);
    }

    public static void RemoveRange<T>(ref T[] array, int index, int count)
    {
        var result = new T[array.Length - count];
        Array.Copy(array, 0, result, 0, index);
        var len = array.Length - index - count - 1;
        if (len > 0)
            Array.Copy(array, index + count, result, index, len);
    }
}
}
