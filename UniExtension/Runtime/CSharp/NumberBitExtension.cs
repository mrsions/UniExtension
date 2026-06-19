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
public static class NumberBitExtension
{
    public static int CountBit(this short v) => sizeof(short) * 8;
    public static bool IsBit(this short v, int index) => (v & (1 << index)) != 0;
    public static short SetBit(this ref short v, int index, bool value)
    {
        const int BIT_MOD = sizeof(short) * 8 - 1;

        if ((uint)index >= (uint)v.CountBit())
        {
            ThrowArgumentOutOfRangeException(index);
        }

        var num = (short)(1 << (index & BIT_MOD));
        if (value)
        {
            v |= num;
        }
        else
        {
            v &= (short)~num;
        }
        return v;
    }

    public static int CountBit(this byte v) => sizeof(byte) * 8;
    public static bool IsBit(this byte v, int index) => (v & (1 << index)) != 0;
    public static byte SetBit(this ref byte v, int index, bool value)
    {
        const int BIT_MOD = sizeof(byte) * 8 - 1;

        if ((uint)index >= (uint)v.CountBit())
        {
            ThrowArgumentOutOfRangeException(index);
        }

        var num = (byte)(1 << (index & BIT_MOD));
        if (value)
        {
            v |= num;
        }
        else
        {
            v &= (byte)~num;
        }
        return v;
    }

    public static int CountBit(this int v) => sizeof(int) * 8;
    public static bool IsBit(this int v, int index) => (v & (1 << index)) != 0;
    public static int SetBit(this ref int v, int index, bool value)
    {
        const int BIT_MOD = sizeof(int) * 8 - 1;

        if ((uint)index >= (uint)v.CountBit())
        {
            ThrowArgumentOutOfRangeException(index);
        }

        var num = (int)(1 << (index & BIT_MOD));
        if (value)
        {
            v |= num;
        }
        else
        {
            v &= (int)~num;
        }
        return v;
    }

    private static void ThrowArgumentOutOfRangeException(int index)
    {
        throw new ArgumentOutOfRangeException("index", index, "ArgumentOutOfRange_Index");
    }
}
}
