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

#nullable enable

using System;
using System.Text;

namespace System
{
public static class HexExtension
{
    public static string ToHexString(this byte[] data, string? format = null)
    {
        return ToHexString(data, format, 0, data.Length);
    }
    public static string ToHexString(this byte[] data, string? format, int length)
    {
        return ToHexString(data, format, 0, length);
    }
    public static string ToHexString(this byte[] data, string? format, int offset, int length)
    {
        int len, i, j;
        char c;

        var sb = new StringBuilder();
        for (i = 0; i < length; i += 16)
        {
            sb.AppendFormat("{0:X4}:  ", i);
            len = Math.Min(length - i, 16);
            for (j = 0; j < 16; j++)
            {
                if (j < len)
                {
                    sb.AppendFormat("{0:X2} ", data[i + offset + j]);
                }
                else
                {
                    sb.Append("   ");
                }
                if (j % 4 == 3) sb.Append(" ");
                if (j == 7) sb.Append(" ");
            }

            sb.Append("     ");
            for (j = 0; j < len; j++)
            {
                c = (char)data[i + offset + j];
                if (0x20 <= c && c <= 0x7F)
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append('.');
                }
            }
            sb.Append("\r\n");
        }
        return sb.ToString();
    }
    public static string ToHexStringForCode(this byte[] data, int offset = 0, int length = 0)
    {
        if (length == 0) length = data.Length - offset;

        if (offset + length > data.Length)
            throw new ArgumentOutOfRangeException($"data(length: {data.Length}) > offset({offset}) + length({length})");

        var sb = new StringBuilder(10 + (length - 1) * (12) + (length / 16 * 2));
        for (var i = 0; i < length; i++)
        {
            if (i > 0)
            {
                sb.Append(", ");
            }
            sb.Append("(byte)0x").Append(data[i + offset].ToString("X"));
            if (i % 16 == 15) sb.AppendLine();
        }
        return sb.ToString();
    }
}
}
