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

using System;

namespace System
{
public static class PrimitiveExtension
{
    public static byte ToByte(this string txt, byte def = 0) { byte result; if (byte.TryParse(txt, out result)) return result; else return def; }
    public static sbyte ToSbyte(this string txt, sbyte def = 0) { sbyte result; if (sbyte.TryParse(txt, out result)) return result; else return def; }
    public static short ToShort(this string txt, short def = 0) { short result; if (short.TryParse(txt, out result)) return result; else return def; }
    public static ushort ToUShort(this string txt, ushort def = 0) { ushort result; if (ushort.TryParse(txt, out result)) return result; else return def; }
    public static int ToInt(this string txt, int def = 0) { int result; if (int.TryParse(txt, out result)) return result; else return def; }
    public static uint ToUint(this string txt, uint def = 0) { uint result; if (uint.TryParse(txt, out result)) return result; else return def; }
    public static long ToLong(this string txt, long def = 0) { long result; if (long.TryParse(txt, out result)) return result; else return def; }
    public static ulong ToULong(this string txt, ulong def = 0) { ulong result; if (ulong.TryParse(txt, out result)) return result; else return def; }
    public static float ToFloat(this string txt, float def = 0) { float result; if (float.TryParse(txt, out result)) return result; else return def; }
    public static double ToDouble(this string txt, double def = 0) { double result; if (double.TryParse(txt, out result)) return result; else return def; }
    public static decimal ToDecimal(this string txt, decimal def = 0) { decimal result; if (decimal.TryParse(txt, out result)) return result; else return def; }
    public static char ToChar(this string txt, char def = '\0') { char result; if (char.TryParse(txt, out result)) return result; else return def; }
    public static bool ToBool(this string txt, bool def = false)
    {
        if (string.IsNullOrEmpty(txt))
            return def;
        var c = txt[0];
        return c == 't' || c == 'T' || c == '1';
    }

    public static sbyte ToSbyteOrPrint(this string str) => sbyte.TryParse(str, out var rst) ? rst : throw new FormatException($"'{str}' Input string was not correct format.");
    public static byte ToByteOrPrint(this string str) => byte.TryParse(str, out var rst) ? rst : throw new FormatException($"'{str}' Input string was not correct format.");
    public static char ToCharOrPrint(this string str) => char.TryParse(str, out var rst) ? rst : throw new FormatException($"'{str}' Input string was not correct format.");
    public static short ToShortOrPrint(this string str) => short.TryParse(str, out var rst) ? rst : throw new FormatException($"'{str}' Input string was not correct format.");
    public static ushort ToUshortOrPrint(this string str) => ushort.TryParse(str, out var rst) ? rst : throw new FormatException($"'{str}' Input string was not correct format.");
    public static int ToIntOrThrow(this string str) => int.TryParse(str, out var rst) ? rst : throw new FormatException($"'{str}' Input string was not correct format.");
    public static uint ToUintOrPrint(this string str) => uint.TryParse(str, out var rst) ? rst : throw new FormatException($"'{str}' Input string was not correct format.");
    public static long ToLongOrPrint(this string str) => long.TryParse(str, out var rst) ? rst : throw new FormatException($"'{str}' Input string was not correct format.");
    public static ulong ToUlongOrPrint(this string str) => ulong.TryParse(str, out var rst) ? rst : throw new FormatException($"'{str}' Input string was not correct format.");
    public static bool ToBoolOrPrint(this string str) => bool.TryParse(str, out var rst) ? rst : throw new FormatException($"'{str}' Input string was not correct format.");
    public static float ToFloatOrPrint(this string str) => float.TryParse(str, out var rst) ? rst : throw new FormatException($"'{str}' Input string was not correct format.");
    public static double ToDoubleOrPrint(this string str) => double.TryParse(str, out var rst) ? rst : throw new FormatException($"'{str}' Input string was not correct format.");
    public static decimal ToDecimalOrPrint(this string str) => decimal.TryParse(str, out var rst) ? rst : throw new FormatException($"'{str}' Input string was not correct format.");
}
}
