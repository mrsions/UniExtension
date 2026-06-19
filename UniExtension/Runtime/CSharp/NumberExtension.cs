// Copyright (c) 2016 Sions
// 
// UniExtension version 1.0.0
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Collections.Generic;
using UniExtension.Collections;
using UniExtension;
using UnityEngine;

namespace System
{
public static class NumberExtension
{
    public static int Sum(this int[] vals)
    {
        var result = 0;
        for (var i = 0; i < vals.Length; i++)
            result += vals[i];
        return result;
    }
    public static float Sum(this float[] vals)
    {
        float result = 0;
        for (var i = 0; i < vals.Length; i++)
            result += vals[i];
        return result;
    }
    public static string ToRemainTime_Discarding_MM_SS_F(float time)
    {
        if (time > 60)
        {
            var t = (int)time;
            return "{0:D2}:{1:D2}".ToFormat(t / 60, t % 60);
        }
        else if (time > 10)
        {
            return "{0}".ToFormat(time);
        }
        else
        {
            return "{0:f1}".ToFormat(time);
        }
    }
    public static string ToRemainTime_MM_SS(float time)
    {
        var t = (int)time;
        return "{0:D2}:{1:D2}".ToFormat(t / 60, t % 60);
    }
    public static bool EqualsEpsilon(this float a, float b)
    {
        return Mathf.Abs(a - b) < Mathf.Epsilon;
    }

    public static bool IsIn(this float val, float min, float max) => min <= val && val <= max;
    public static bool IsOut(this float val, float min, float max) => min > val || val > max;
    public static bool IsIn(this int val, int min, int max) => min <= val && val <= max;
    public static bool IsOut(this int val, int min, int max) => min > val || val > max;


    readonly static string[] BytesSuffixex = new string[] { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
    public static string ToStringReadableBytes(this long bytes, string[] bytesSuffixes)
    {
        return bytes.ToStringReadableBytes(BytesSuffixex);
    }
    public static string ToStringReadableBytes(this long bytes)
    {
        var idx = 0;
        long divid = 1;
        while (bytes > divid * 1024)
        {
            divid *= 1024;
            idx++;
        }

        float size = 0;
        if (idx == 0) size = bytes;
        else size = (float)(bytes / (divid / 1024)) / 1024f;

        if (size > 900)
        {
            size /= 1024f;
            idx++;
        }

        if (idx == 0 || size >= 100)
        {
            return ((int)size) + BytesSuffixex[idx];
        }
        else if (size >= 10)
        {
            return size.ToString("f1") + BytesSuffixex[idx];
        }
        else
        {
            return size.ToString("f2") + BytesSuffixex[idx];
        }
    }
    public static string ToStringPercent100(this float percent)
    {
        percent *= 100;
        if (percent >= 100) return percent.ToString("f0");
        else if (percent > 10) return percent.ToString("f1");
        else return percent.ToString("f2");
    }

    private const string DefaultSplit = ", \r\n\t";
    public static float[] ToSplitFloatArray(this string str, int length, string splitSource = DefaultSplit)
    {
        var rst = new float[length];
        var idx = 0;
        var tokenizer = new StringTokenizer(str, splitSource, true, false);
        foreach (var v in tokenizer)
        {
            rst[idx++] = float.Parse(v);
            if (idx == length) break;
        }
        return rst;
    }
    public static float[] ToSplitFloatArray(this string str, string splitSource = DefaultSplit)
    {
        using (var list = PList.Take<float>())
        {
            var tokenizer = new StringTokenizer(str, splitSource, true, false);
            foreach (var v in tokenizer)
            {
                list.Add(float.Parse(v));
            }
            return list.ToArray();
        }
    }
    public static int[] ToSplitIntArray(this string str, int length, string splitSource = DefaultSplit)
    {
        var rst = new int[length];
        var idx = 0;
        var tokenizer = new StringTokenizer(str, splitSource, true, false);
        foreach (var v in tokenizer)
        {
            rst[idx++] = int.Parse(v);
            if (idx == length) break;
        }
        return rst;
    }
    public static int[] ToSplitIntArray(this string str, string splitSource = DefaultSplit)
    {
        using (var list = PList.Take<int>())
        {
            var tokenizer = new StringTokenizer(str, splitSource, true, false);
            foreach (var v in tokenizer)
            {
                list.Add(int.Parse(v));
            }
            return list.ToArray();
        }
    }
}
}
