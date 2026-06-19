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

using System.Text;
using UnityEngine;

namespace System.Text
{
public static class StringBuilderExtension
{
    public static StringBuilder AppendV(this StringBuilder sb, Quaternion v) { return sb.Append(v.x).Append(',').Append(v.y).Append(',').Append(v.z).Append(',').Append(v.w); }
    public static StringBuilder AppendV(this StringBuilder sb, Vector4 v) { return sb.Append(v.x).Append(',').Append(v.y).Append(',').Append(v.z).Append(',').Append(v.w); }
    public static StringBuilder AppendV(this StringBuilder sb, Vector3 v) { return sb.Append(v.x).Append(',').Append(v.y).Append(',').Append(v.z); }
    public static StringBuilder AppendV(this StringBuilder sb, Vector2 v) { return sb.Append(v.x).Append(',').Append(v.y); }

    public static StringBuilder Append(this StringBuilder sb, double value, char delimiter) { if (sb.Length != 0) sb.Append(delimiter); return sb.Append(value); }
    public static StringBuilder Append(this StringBuilder sb, long value, char delimiter) { if (sb.Length != 0) sb.Append(delimiter); return sb.Append(value); }
    public static StringBuilder Append(this StringBuilder sb, int value, char delimiter) { if (sb.Length != 0) sb.Append(delimiter); return sb.Append(value); }
    public static StringBuilder Append(this StringBuilder sb, short value, char delimiter) { if (sb.Length != 0) sb.Append(delimiter); return sb.Append(value); }
    public static StringBuilder Append(this StringBuilder sb, float value, char delimiter) { if (sb.Length != 0) sb.Append(delimiter); return sb.Append(value); }
    public static StringBuilder Append(this StringBuilder sb, decimal value, char delimiter) { if (sb.Length != 0) sb.Append(delimiter); return sb.Append(value); }
    public static StringBuilder Append(this StringBuilder sb, byte value, char delimiter) { if (sb.Length != 0) sb.Append(delimiter); return sb.Append(value); }
    public static StringBuilder Append(this StringBuilder sb, string value, char delimiter) { if (sb.Length != 0) sb.Append(delimiter); return sb.Append(value); }
    public static StringBuilder Append(this StringBuilder sb, object value, char delimiter) { if (sb.Length != 0) sb.Append(delimiter); return sb.Append(value); }
    public static StringBuilder Append(this StringBuilder sb, char[] value, char delimiter) { if (sb.Length != 0) sb.Append(delimiter); return sb.Append(value); }
    public static StringBuilder Append(this StringBuilder sb, ushort value, char delimiter) { if (sb.Length != 0) sb.Append(delimiter); return sb.Append(value); }
    public static StringBuilder Append(this StringBuilder sb, uint value, char delimiter) { if (sb.Length != 0) sb.Append(delimiter); return sb.Append(value); }
    public static StringBuilder Append(this StringBuilder sb, ulong value, char delimiter) { if (sb.Length != 0) sb.Append(delimiter); return sb.Append(value); }
    public static StringBuilder Append(this StringBuilder sb, char value, char delimiter) { if (sb.Length != 0) sb.Append(delimiter); return sb.Append(value); }
    public static StringBuilder Append(this StringBuilder sb, bool value, char delimiter) { if (sb.Length != 0) sb.Append(delimiter); return sb.Append(value); }
    public static StringBuilder Append(this StringBuilder sb, sbyte value, char delimiter) { if (sb.Length != 0) sb.Append(delimiter); return sb.Append(value); }
    public static StringBuilder AppendTab(this StringBuilder sb) { if (sb.Length != 0) sb.Append('\t'); return sb; }
    public static StringBuilder AppendTab(this StringBuilder sb, double value) { sb.Append(value, '\t'); return sb; }
    public static StringBuilder AppendTab(this StringBuilder sb, long value) { sb.Append(value, '\t'); return sb; }
    public static StringBuilder AppendTab(this StringBuilder sb, int value) { sb.Append(value, '\t'); return sb; }
    public static StringBuilder AppendTab(this StringBuilder sb, short value) { sb.Append(value, '\t'); return sb; }
    public static StringBuilder AppendTab(this StringBuilder sb, float value) { sb.Append(value, '\t'); return sb; }
    public static StringBuilder AppendTab(this StringBuilder sb, decimal value) { sb.Append(value, '\t'); return sb; }
    public static StringBuilder AppendTab(this StringBuilder sb, byte value) { sb.Append(value, '\t'); return sb; }
    public static StringBuilder AppendTab(this StringBuilder sb, string value) { sb.Append(value, '\t'); return sb; }
    public static StringBuilder AppendTab(this StringBuilder sb, object value) { sb.Append(value, '\t'); return sb; }
    public static StringBuilder AppendTab(this StringBuilder sb, char[] value) { sb.Append(value, '\t'); return sb; }
    public static StringBuilder AppendTab(this StringBuilder sb, ushort value) { sb.Append(value, '\t'); return sb; }
    public static StringBuilder AppendTab(this StringBuilder sb, uint value) { sb.Append(value, '\t'); return sb; }
    public static StringBuilder AppendTab(this StringBuilder sb, ulong value) { sb.Append(value, '\t'); return sb; }
    public static StringBuilder AppendTab(this StringBuilder sb, char value) { sb.Append(value, '\t'); return sb; }
    public static StringBuilder AppendTab(this StringBuilder sb, bool value) { sb.Append(value, '\t'); return sb; }
    public static StringBuilder AppendTab(this StringBuilder sb, sbyte value) { sb.Append(value, '\t'); return sb; }

    public static string ToStringAndClear(this StringBuilder sb)
    {
        try
        {
            return sb.ToString();
        }
        finally
        {
            sb.Length = 0;
        }
    }
}
}
