using System;
using System.Collections.Generic;
using System.IO;
using UniExtension;
using UnityEngine;

namespace System.IO
{
    public static partial class BinaryStreamExtension
    {
        public static void WriteStream(this Stream stream, byte[] data, int offset, int length, int bufferSize)
        {
            var write = 0;
            var writed = 0;
            while (writed < length)
            {
                write = Math.Min(length - writed, bufferSize);
                stream.Write(data, offset + writed, write);
                writed += write;
            }
        }

        /// <summary>
        /// C# DateTime을 Unix타임으로 6바이트를 사용하여 기록한다.
        /// 1970년 ~ 10,895년 범위의 기간에 사용.
        /// </summary>
        public static void WriteDateTimeUnix(this BinaryWriter w, DateTime value)
        {
            w.WriteInt48(new DateTimeUnix(value).Ticks);
        }
        /// <summary>
        /// DateTime을 6바이트로 기록한다.
        /// 0년 ~ 8925년까지 사용.
        /// </summary>
        public static void WriteDateTimeMs(this BinaryWriter w, DateTime value)
        {
            w.WriteInt48(value.Ticks / TimeSpan.TicksPerMillisecond);
        }
        public static void WriteDateTime(this BinaryWriter w, DateTime value)
        {
            w.WriteInt64(value.Ticks);
        }

        public static void WriteBytes(this BinaryWriter self, byte[] v, int offset, int length) => self.Write(v, offset, length);
        public static void WriteBytes(this BinaryWriter self, ArraySegment<byte> v) => self.Write(v.Array, v.Offset, v.Count);

        public static void WriteInt8(this BinaryWriter self, sbyte value) { self.Write(value); }
        public static void WriteUInt8(this BinaryWriter self, byte value) { self.Write(value); }
        public static void WriteInt8<T>(this BinaryWriter self, T v) where T : struct, IConvertible { self.Write(v.ToSByte(null)); }
        public static void WriteUInt8<T>(this BinaryWriter self, T v) where T : struct, IConvertible { self.Write(v.ToByte(null)); }

        public static void WriteInt16(this BinaryWriter self, short value) { self.Write(value); }
        public static void WriteUInt16(this BinaryWriter self, ushort value) { self.Write(value); }
        public static void WriteInt16<T>(this BinaryWriter self, T v) where T : struct, IConvertible { self.Write(v.ToInt16(null)); }
        public static void WriteUInt16<T>(this BinaryWriter self, T v) where T : struct, IConvertible { self.Write(v.ToUInt16(null)); }

        public static void WriteInt32(this BinaryWriter self, int value) { self.Write(value); }
        public static void WriteUInt32(this BinaryWriter self, uint value) { self.Write(value); }
        public static void WriteInt32<T>(this BinaryWriter self, T v) where T : struct, IConvertible { self.Write(v.ToInt32(null)); }
        public static void WriteUInt32<T>(this BinaryWriter self, T v) where T : struct, IConvertible { self.Write(v.ToUInt32(null)); }

        public static void WriteInt64(this BinaryWriter self, long value) { self.Write(value); }
        public static void WriteUInt64(this BinaryWriter self, ulong value) { self.Write(value); }
        public static void WriteInt64<T>(this BinaryWriter self, T v) where T : struct, IConvertible { self.Write(v.ToInt64(null)); }
        public static void WriteUInt64<T>(this BinaryWriter self, T v) where T : struct, IConvertible { self.Write(v.ToUInt64(null)); }

        public static void WriteSingle(this BinaryWriter self, float value) { self.Write(value); }
        public static void WriteFloat(this BinaryWriter self, float value) { self.Write(value); }
        public static void WriteDouble(this BinaryWriter self, double value) { self.Write(value); }
        public static void WriteSingle<T>(this BinaryWriter self, T v) where T : struct, IConvertible { self.Write(v.ToSingle(null)); }
        public static void WriteFloat<T>(this BinaryWriter self, T v) where T : struct, IConvertible { self.Write(v.ToSingle(null)); }
        public static void WriteDouble<T>(this BinaryWriter self, T v) where T : struct, IConvertible { self.Write(v.ToDouble(null)); }

        public static void WriteBoolean(this BinaryWriter self, bool value) { self.Write(value); }
        public static void WriteBoolean<T>(this BinaryWriter self, T v) where T : struct, IConvertible { self.Write(v.ToBoolean(null)); }
        public static void WriteString(this BinaryWriter self, string value) { self.Write(value ?? string.Empty); }
        public static void WriteString<T>(this BinaryWriter self, T v) { self.Write(v.ToString()); }

        public static void WritePackedUInt32(this BinaryWriter self, uint value)
        {
            if (value <= 240)
            {
                self.Write((byte)value);
                return;
            }
            if (value <= 2287)
            {
                self.Write((byte)((value - 240) / 256 + 241));
                self.Write((byte)((value - 240) % 256));
                return;
            }
            if (value <= 67823)
            {
                self.Write((byte)249);
                self.Write((byte)((value - 2288) / 256));
                self.Write((byte)((value - 2288) % 256));
                return;
            }
            if (value <= 16777215)
            {
                self.Write((byte)250);
                self.Write((byte)(value & 0xFF));
                self.Write((byte)((value >> 8) & 0xFF));
                self.Write((byte)((value >> 16) & 0xFF));
                return;
            }

            // all other values of uint
            self.Write((byte)251);
            self.Write((byte)(value & 0xFF));
            self.Write((byte)((value >> 8) & 0xFF));
            self.Write((byte)((value >> 16) & 0xFF));
            self.Write((byte)((value >> 24) & 0xFF));
        }
        public static void WritePackedUInt64(this BinaryWriter self, UInt64 value)
        {
            if (value <= 240)
            {
                self.Write((byte)value);
                return;
            }
            if (value <= 2287)
            {
                self.Write((byte)((value - 240) / 256 + 241));
                self.Write((byte)((value - 240) % 256));
                return;
            }
            if (value <= 67823)
            {
                self.Write((byte)249);
                self.Write((byte)((value - 2288) / 256));
                self.Write((byte)((value - 2288) % 256));
                return;
            }
            if (value <= 16777215)
            {
                self.Write((byte)250);
                self.Write((byte)(value & 0xFF));
                self.Write((byte)((value >> 8) & 0xFF));
                self.Write((byte)((value >> 16) & 0xFF));
                return;
            }
            if (value <= 4294967295)
            {
                self.Write((byte)251);
                self.Write((byte)(value & 0xFF));
                self.Write((byte)((value >> 8) & 0xFF));
                self.Write((byte)((value >> 16) & 0xFF));
                self.Write((byte)((value >> 24) & 0xFF));
                return;
            }
            if (value <= 1099511627775)
            {
                self.Write((byte)252);
                self.Write((byte)(value & 0xFF));
                self.Write((byte)((value >> 8) & 0xFF));
                self.Write((byte)((value >> 16) & 0xFF));
                self.Write((byte)((value >> 24) & 0xFF));
                self.Write((byte)((value >> 32) & 0xFF));
                return;
            }
            if (value <= 281474976710655)
            {
                self.Write((byte)253);
                self.Write((byte)(value & 0xFF));
                self.Write((byte)((value >> 8) & 0xFF));
                self.Write((byte)((value >> 16) & 0xFF));
                self.Write((byte)((value >> 24) & 0xFF));
                self.Write((byte)((value >> 32) & 0xFF));
                self.Write((byte)((value >> 40) & 0xFF));
                return;
            }
            if (value <= 72057594037927935)
            {
                self.Write((byte)254);
                self.Write((byte)(value & 0xFF));
                self.Write((byte)((value >> 8) & 0xFF));
                self.Write((byte)((value >> 16) & 0xFF));
                self.Write((byte)((value >> 24) & 0xFF));
                self.Write((byte)((value >> 32) & 0xFF));
                self.Write((byte)((value >> 40) & 0xFF));
                self.Write((byte)((value >> 48) & 0xFF));
                return;
            }

            // all others
            {
                self.Write((byte)255);
                self.Write((byte)(value & 0xFF));
                self.Write((byte)((value >> 8) & 0xFF));
                self.Write((byte)((value >> 16) & 0xFF));
                self.Write((byte)((value >> 24) & 0xFF));
                self.Write((byte)((value >> 32) & 0xFF));
                self.Write((byte)((value >> 40) & 0xFF));
                self.Write((byte)((value >> 48) & 0xFF));
                self.Write((byte)((value >> 56) & 0xFF));
            }
        }

        public static void WriteInt40(this BinaryWriter self, long value)
        {
            self.Write((byte)((value) & 0xFF));
            self.Write((byte)((value >> 0x08) & 0xFF));
            self.Write((byte)((value >> 0x10) & 0xFF));
            self.Write((byte)((value >> 0x18) & 0xFF));
            self.Write((byte)(((value >> 0x20) & 0x7F) | (long)(((ulong)value >> 63) << 7)));
        }
        public static void WriteInt48(this BinaryWriter self, long value)
        {
            self.Write((byte)((value) & 0xFF));
            self.Write((byte)((value >> 0x08) & 0xFF));
            self.Write((byte)((value >> 0x10) & 0xFF));
            self.Write((byte)((value >> 0x18) & 0xFF));
            self.Write((byte)((value >> 0x20) & 0xFF));
            self.Write((byte)(((value >> 0x28) & 0x7F) | (long)(((ulong)value >> 63) << 7)));
        }
        public static void WriteInt56(this BinaryWriter self, long value)
        {
            self.Write((byte)((value) & 0xFF));
            self.Write((byte)((value >> 0x08) & 0xFF));
            self.Write((byte)((value >> 0x10) & 0xFF));
            self.Write((byte)((value >> 0x18) & 0xFF));
            self.Write((byte)((value >> 0x20) & 0xFF));
            self.Write((byte)((value >> 0x28) & 0xFF));
            self.Write((byte)(((value >> 0x30) & 0x7F) | (long)(((ulong)value >> 63) << 7)));
        }

        public static void WriteUInt40(this BinaryWriter self, long value)
        {
            self.Write((byte)((value) & 0xFF));
            self.Write((byte)((value >> 0x08) & 0xFF));
            self.Write((byte)((value >> 0x10) & 0xFF));
            self.Write((byte)((value >> 0x18) & 0xFF));
            self.Write((byte)((value >> 0x20) & 0xFF));
        }
        public static void WriteUInt48(this BinaryWriter self, long value)
        {
            self.Write((byte)((value) & 0xFF));
            self.Write((byte)((value >> 0x08) & 0xFF));
            self.Write((byte)((value >> 0x10) & 0xFF));
            self.Write((byte)((value >> 0x18) & 0xFF));
            self.Write((byte)((value >> 0x20) & 0xFF));
            self.Write((byte)((value >> 0x28) & 0xFF));
        }
        public static void WriteUInt56(this BinaryWriter self, long value)
        {
            self.Write((byte)((value) & 0xFF));
            self.Write((byte)((value >> 0x08) & 0xFF));
            self.Write((byte)((value >> 0x10) & 0xFF));
            self.Write((byte)((value >> 0x18) & 0xFF));
            self.Write((byte)((value >> 0x20) & 0xFF));
            self.Write((byte)((value >> 0x28) & 0xFF));
            self.Write((byte)((value >> 0x30) & 0xFF));
        }


        /// <summary>
        /// v값은 -2~2 사이인 값을 2byte로 입력합니다.
        /// </summary>
        public static void WriteHalf(this BinaryWriter self, float v)
        {
            const int MAX = 2;
            const int DIV = MAX * 2;
            const int ONE = (ushort.MaxValue / DIV);
            const int FOUR = ONE * DIV;
            self.Write((ushort)(Math.Clamp(v - MAX, 0, DIV) * (1f / DIV) * FOUR));
        }
        /// <summary>
        /// v값은 0~1 사이인 값을 2byte로 입력합니다.
        /// </summary>
        public static void WriteHalf01(this BinaryWriter self, float v)
        {
            const int VAL = ushort.MaxValue;
            self.Write((ushort)(Math.Clamp(v, 0, 1) * VAL));
        }
        /// <summary>
        /// v값은 -2~2 사이인 값을 1byte로 입력합니다.
        /// </summary>
        public static void WriteFixed(this BinaryWriter self, float v)
        {
            const int MAX = 2;
            const int DIV = MAX * 2;
            const int VAL = (byte.MaxValue / DIV) * DIV;
            self.Write((byte)(Math.Clamp(v - MAX, 0, DIV) * (1f / DIV) * VAL));
        }
        /// <summary>
        /// v값은 0~1 사이인 값을 1byte로 입력합니다.
        /// </summary>
        public static void WriteFixed01(this BinaryWriter self, float v)
        {
            const int VAL = byte.MaxValue;
            self.Write((byte)(Math.Clamp(v, 0, 1) * VAL));
        }


        public static void WriteByteArray(this BinaryWriter self, byte[] v) => self.WriteByteArray(v, 0, v.Length);
        public static void WriteByteArray(this BinaryWriter self, byte[] v, int offset, int length)
        {
            ThrowIfOutOfLength(length, int.MaxValue);
            self.WritePackedUInt32((uint)length);
            self.Write(v, offset, length);
        }
        public static void WriteByteArraySize8(this BinaryWriter self, byte[] v) => self.WriteByteArraySize8(v, 0, v.Length);
        public static void WriteByteArraySize8(this BinaryWriter self, byte[] v, int offset, int length)
        {
            ThrowIfOutOfLength(length, byte.MaxValue);
            self.Write((byte)length);
            self.Write(v, offset, length);
        }
        public static void WriteByteArraySize16(this BinaryWriter self, byte[] v) => self.WriteByteArraySize16(v, 0, v.Length);
        public static void WriteByteArraySize16(this BinaryWriter self, byte[] v, int offset, int length)
        {
            ThrowIfOutOfLength(length, ushort.MaxValue);
            self.Write((ushort)length);
            self.Write(v, offset, length);
        }
        public static void WriteByteArraySize32(this BinaryWriter self, byte[] v) => self.WriteByteArraySize32(v, 0, v.Length);
        public static void WriteByteArraySize32(this BinaryWriter self, byte[] v, int offset, int length)
        {
            ThrowIfOutOfLength(length, int.MaxValue);
            self.Write((int)length);
            self.Write(v, offset, length);
        }


        public static void WriteEnum8<T>(this BinaryWriter self, T v) where T : Enum => self.Write((sbyte)v.GetHashCode());
        public static void WriteEnumU8<T>(this BinaryWriter self, T v) where T : Enum => self.Write((byte)v.GetHashCode());
        public static void WriteEnum16<T>(this BinaryWriter self, T v) where T : Enum => self.Write((short)v.GetHashCode());
        public static void WriteEnumU16<T>(this BinaryWriter self, T v) where T : Enum => self.Write((ushort)v.GetHashCode());
        public static void WriteEnum32<T>(this BinaryWriter self, T v) where T : Enum => self.Write((int)v.GetHashCode());
        public static void WriteEnumU32<T>(this BinaryWriter self, T v) where T : Enum => self.Write((uint)v.GetHashCode());
        public static void WriteEnum64<T>(this BinaryWriter self, T v) where T : Enum, IConvertible => self.Write(v.ToInt64(null));
        public static void WriteEnumU64<T>(this BinaryWriter self, T v) where T : Enum, IConvertible => self.Write(v.ToUInt64(null));
    }
}