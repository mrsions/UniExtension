using System.Runtime.CompilerServices;

namespace System
{
public static class FlagExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasFlag(this byte val, int n) => (val & (byte)(1u << n)) != 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte SetFlag(this byte val, int n, bool value)
    {
        byte mask = (byte)(1u << n);
        return value ? (byte)(val | mask) : (byte)(val & (byte)~mask);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasFlag(this short val, int n) => (val & (short)(1u << n)) != 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short SetFlag(this short val, int n, bool value)
    {
        short mask = (short)(1u << n);
        return value ? (short)(val | mask) : (short)(val & (short)~mask);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasFlag(this int val, int n) => (val & (1 << n)) != 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SetFlag(this int val, int n, bool value)
    {
        int mask = 1 << n;
        return value ? (val | mask) : (val & ~mask);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasFlag(this long val, int n) => (val & (1L << n)) != 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long SetFlag(this long val, int n, bool value)
    {
        long mask = 1L << n;
        return value ? (val | mask) : (val & ~mask);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LeadingZeroCount(this byte val) => LeadingZeroCount((uint)val, 8);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TrailingZeroCount(this byte val) => TrailingZeroCount((uint)val, 8);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int PopCount(this byte val) => PopCount((uint)val);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte RotateLeft(this byte val, int offset) => (byte)RotateLeft((uint)val, offset, 8);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte RotateRight(this byte val, int offset) => (byte)RotateRight((uint)val, offset, 8);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Log2(this byte val) => Log2((uint)val, 8);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPow2(this byte val) => IsPow2((uint)val);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte RoundUpToPowerOf2(this byte val) => (byte)RoundUpToPowerOf2((uint)val, 8);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LeadingZeroCount(this short val) => LeadingZeroCount((uint)(ushort)val, 16);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TrailingZeroCount(this short val) => TrailingZeroCount((uint)(ushort)val, 16);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int PopCount(this short val) => PopCount((uint)(ushort)val);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short RotateLeft(this short val, int offset) => (short)RotateLeft((uint)(ushort)val, offset, 16);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short RotateRight(this short val, int offset) => (short)RotateRight((uint)(ushort)val, offset, 16);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Log2(this short val) => Log2((uint)(ushort)val, 16);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPow2(this short val) => val > 0 && IsPow2((uint)(ushort)val);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short RoundUpToPowerOf2(this short val) => val <= 0 ? (short)0 : (short)RoundUpToPowerOf2((uint)(ushort)val, 16);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LeadingZeroCount(this int val) => LeadingZeroCount((uint)val, 32);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TrailingZeroCount(this int val) => TrailingZeroCount((uint)val, 32);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int PopCount(this int val) => PopCount((uint)val);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int RotateLeft(this int val, int offset) => (int)RotateLeft((uint)val, offset, 32);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int RotateRight(this int val, int offset) => (int)RotateRight((uint)val, offset, 32);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Log2(this int val) => Log2((uint)val, 32);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPow2(this int val) => val > 0 && IsPow2((uint)val);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int RoundUpToPowerOf2(this int val) => val <= 0 ? 0 : (int)RoundUpToPowerOf2((uint)val, 32);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LeadingZeroCount(this long val) => LeadingZeroCount((ulong)val, 64);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TrailingZeroCount(this long val) => TrailingZeroCount((ulong)val, 64);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int PopCount(this long val) => PopCount((ulong)val);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long RotateLeft(this long val, int offset) => (long)RotateLeft((ulong)val, offset, 64);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long RotateRight(this long val, int offset) => (long)RotateRight((ulong)val, offset, 64);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Log2(this long val) => Log2((ulong)val, 64);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPow2(this long val) => val > 0 && IsPow2((ulong)val);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long RoundUpToPowerOf2(this long val) => val <= 0 ? 0 : (long)RoundUpToPowerOf2((ulong)val, 64);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int LeadingZeroCount(uint value, int bits)
    {
        if (value == 0) return bits;
        int count = 0;
        for (int i = bits - 1; i >= 0; i--)
        {
            if ((value & (1u << i)) != 0) break;
            count++;
        }
        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int LeadingZeroCount(ulong value, int bits)
    {
        if (value == 0) return bits;
        int count = 0;
        for (int i = bits - 1; i >= 0; i--)
        {
            if ((value & (1UL << i)) != 0) break;
            count++;
        }
        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int TrailingZeroCount(uint value, int bits)
    {
        if (value == 0) return bits;
        int count = 0;
        for (int i = 0; i < bits; i++)
        {
            if ((value & (1u << i)) != 0) break;
            count++;
        }
        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int TrailingZeroCount(ulong value, int bits)
    {
        if (value == 0) return bits;
        int count = 0;
        for (int i = 0; i < bits; i++)
        {
            if ((value & (1UL << i)) != 0) break;
            count++;
        }
        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int PopCount(uint value)
    {
        int count = 0;
        while (value != 0)
        {
            value &= value - 1;
            count++;
        }
        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int PopCount(ulong value)
    {
        int count = 0;
        while (value != 0)
        {
            value &= value - 1;
            count++;
        }
        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint RotateLeft(uint value, int offset, int bits)
    {
        uint mask = bits == 32 ? uint.MaxValue : ((1u << bits) - 1u);
        int shift = offset & (bits - 1);
        return ((value << shift) | (value >> (bits - shift))) & mask;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint RotateRight(uint value, int offset, int bits)
    {
        uint mask = bits == 32 ? uint.MaxValue : ((1u << bits) - 1u);
        int shift = offset & (bits - 1);
        return ((value >> shift) | (value << (bits - shift))) & mask;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong RotateLeft(ulong value, int offset, int bits)
    {
        ulong mask = bits == 64 ? ulong.MaxValue : ((1UL << bits) - 1UL);
        int shift = offset & (bits - 1);
        return ((value << shift) | (value >> (bits - shift))) & mask;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong RotateRight(ulong value, int offset, int bits)
    {
        ulong mask = bits == 64 ? ulong.MaxValue : ((1UL << bits) - 1UL);
        int shift = offset & (bits - 1);
        return ((value >> shift) | (value << (bits - shift))) & mask;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Log2(uint value, int bits)
    {
        if (value == 0) return 0;
        return (bits - 1) - LeadingZeroCount(value, bits);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Log2(ulong value, int bits)
    {
        if (value == 0) return 0;
        return (bits - 1) - LeadingZeroCount(value, bits);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsPow2(uint value) => value != 0 && (value & (value - 1)) == 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsPow2(ulong value) => value != 0 && (value & (value - 1)) == 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint RoundUpToPowerOf2(uint value, int bits)
    {
        if (value == 0) return 0;
        uint highest = 1u << (bits - 1);
        if (value > highest) return 0;
        if (IsPow2(value)) return value;
        uint v = value - 1;
        v |= v >> 1;
        v |= v >> 2;
        v |= v >> 4;
        v |= v >> 8;
        if (bits > 16) v |= v >> 16;
        v++;
        uint mask = bits == 32 ? uint.MaxValue : ((1u << bits) - 1u);
        return v & mask;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong RoundUpToPowerOf2(ulong value, int bits)
    {
        if (value == 0) return 0;
        ulong highest = 1UL << (bits - 1);
        if (value > highest) return 0;
        if (IsPow2(value)) return value;
        ulong v = value - 1;
        v |= v >> 1;
        v |= v >> 2;
        v |= v >> 4;
        v |= v >> 8;
        v |= v >> 16;
        if (bits > 32) v |= v >> 32;
        v++;
        ulong mask = bits == 64 ? ulong.MaxValue : ((1UL << bits) - 1UL);
        return v & mask;
    }
}
}
