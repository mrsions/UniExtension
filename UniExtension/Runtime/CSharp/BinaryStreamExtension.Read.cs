using UniExtension;

namespace System.IO
{
    public static partial class BinaryStreamExtension
    {
        public static void ReadCompact(this Stream stream, byte[] buffer, int offset, int length, int bufferSize)
        {
            var read = 0;
            var readed = 0;
            while (readed < length)
            {
                read = stream.Read(buffer, offset + readed, Math.Min(length - readed, bufferSize));
                if (read <= 0)
                {
                    throw new IOException("Can't read. less than equals 0");
                }
                readed += read;
            }
        }

        /// <summary>
        /// C# DateTime을 Unix타임으로 6바이트를 사용하여 읽어온다. 
        /// 1970년 ~ 10,895년 범위의 기간에 사용.
        /// </summary>
        public static DateTime ReadDateTimeUnix(this BinaryReader w)
        {
            return new DateTimeUnix(w.ReadInt48()).ToDateTime();
        }

        /// <summary>
        /// DateTime을 6바이트로 읽는다.
        /// 0년 ~ 8925년까지 사용.
        /// </summary>
        /// <param name="w"></param>
        /// <param name="v"></param>
        public static DateTime ReadDateTimeMs(this BinaryReader w)
        {
            return new DateTime(w.ReadInt48() * TimeSpan.TicksPerMillisecond);
        }
        public static DateTime ReadDateTime(this BinaryReader w)
        {
            return new DateTime(w.ReadInt64());
        }

        public static byte ReadUInt8(this BinaryReader self) { return self.ReadByte(); }
        public static float ReadFloat(this BinaryReader self)
        {
            return self.ReadSingle();
        }
        public static uint ReadPackedUInt32(this BinaryReader self)
        {
            var a0 = self.ReadByte();
            if (a0 < 241)
            {
                return a0;
            }
            var a1 = self.ReadByte();
            if (a0 >= 241 && a0 <= 248)
            {
                return (UInt32)(240 + 256 * (a0 - 241) + a1);
            }
            var a2 = self.ReadByte();
            if (a0 == 249)
            {
                return (UInt32)(2288 + 256 * a1 + a2);
            }
            var a3 = self.ReadByte();
            if (a0 == 250)
            {
                return a1 + (((UInt32)a2) << 8) + (((UInt32)a3) << 16);
            }
            var a4 = self.ReadByte();
            if (a0 >= 251)
            {
                return a1 + (((UInt32)a2) << 8) + (((UInt32)a3) << 16) + (((UInt32)a4) << 24);
            }
            throw new IndexOutOfRangeException("ReadPackedUInt32() failure: " + a0);
        }
        public static ulong ReadPackedUInt64(this BinaryReader self)
        {
            var a0 = self.ReadByte();
            if (a0 < 241)
            {
                return a0;
            }
            var a1 = self.ReadByte();
            if (a0 >= 241 && a0 <= 248)
            {
                return 240 + 256 * (a0 - ((UInt64)241)) + a1;
            }
            var a2 = self.ReadByte();
            if (a0 == 249)
            {
                return 2288 + (((UInt64)256) * a1) + a2;
            }
            var a3 = self.ReadByte();
            if (a0 == 250)
            {
                return a1 + (((UInt64)a2) << 8) + (((UInt64)a3) << 16);
            }
            var a4 = self.ReadByte();
            if (a0 == 251)
            {
                return a1 + (((UInt64)a2) << 8) + (((UInt64)a3) << 16) + (((UInt64)a4) << 24);
            }
            var a5 = self.ReadByte();
            if (a0 == 252)
            {
                return a1 + (((UInt64)a2) << 8) + (((UInt64)a3) << 16) + (((UInt64)a4) << 24) + (((UInt64)a5) << 32);
            }
            var a6 = self.ReadByte();
            if (a0 == 253)
            {
                return a1 + (((UInt64)a2) << 8) + (((UInt64)a3) << 16) + (((UInt64)a4) << 24) + (((UInt64)a5) << 32) + (((UInt64)a6) << 40);
            }
            var a7 = self.ReadByte();
            if (a0 == 254)
            {
                return a1 + (((UInt64)a2) << 8) + (((UInt64)a3) << 16) + (((UInt64)a4) << 24) + (((UInt64)a5) << 32) + (((UInt64)a6) << 40) + (((UInt64)a7) << 48);
            }
            var a8 = self.ReadByte();
            if (a0 == 255)
            {
                return a1 + (((UInt64)a2) << 8) + (((UInt64)a3) << 16) + (((UInt64)a4) << 24) + (((UInt64)a5) << 32) + (((UInt64)a6) << 40) + (((UInt64)a7) << 48) + (((UInt64)a8) << 56);
            }
            throw new IndexOutOfRangeException("ReadPackedUInt64() failure: " + a0);
        }

        public static long ReadInt40(this BinaryReader self)
        {
            var result = (self.ReadByte() & 0xFFL)
                | ((self.ReadByte() << 0x08) & 0xFF00L)
                | ((self.ReadByte() << 0x10) & 0xFF0000L)
                | ((self.ReadByte() << 0x18) & 0xFF000000L)
                | ((self.ReadByte() << 0x20) & 0xFF00000000L);
            return (result & 0x80_0000_0000L) == 0 ? (result & 0x7F_FFFF_FFFFL) : -(result & 0x7F_FFFF_FFFFL);
        }
        public static long ReadInt48(this BinaryReader self)
        {
            var result = (self.ReadByte() & 0xFFL)
                | ((self.ReadByte() << 0x08) & 0xFF00L)
                | ((self.ReadByte() << 0x10) & 0xFF0000L)
                | ((self.ReadByte() << 0x18) & 0xFF000000L)
                | ((self.ReadByte() << 0x20) & 0xFF00000000L)
                | ((self.ReadByte() << 0x28) & 0xFF0000000000L);
            return (result & 0x8000_0000_0000L) == 0 ? (result & 0x7FFF_FFFF_FFFFL) : -(result & 0x7FFF_FFFF_FFFFL);
        }
        public static long ReadInt56(this BinaryReader self)
        {
            var result = (self.ReadByte() & 0xFFL)
                | ((self.ReadByte() << 0x08) & 0xFF00L)
                | ((self.ReadByte() << 0x10) & 0xFF0000L)
                | ((self.ReadByte() << 0x18) & 0xFF000000L)
                | ((self.ReadByte() << 0x20) & 0xFF00000000L)
                | ((self.ReadByte() << 0x28) & 0xFF0000000000L)
                | ((self.ReadByte() << 0x30) & 0xFF000000000000L);

            return (result & 0x80_0000_0000_0000L) == 0 ? (result & 0x7F_FFFF_FFFF_FFFFL) : -(result & 0x7F_FFFF_FFFF_FFFFL);
        }

        public static long ReadUInt40(this BinaryReader self)
        {
            return (self.ReadByte() & 0xFFL)
                | ((self.ReadByte() << 0x08) & 0xFF00L)
                | ((self.ReadByte() << 0x10) & 0xFF0000L)
                | ((self.ReadByte() << 0x18) & 0xFF000000L)
                | ((self.ReadByte() << 0x20) & 0xFF00000000L);
        }
        public static long ReadUInt48(this BinaryReader self)
        {
            return (self.ReadByte() & 0xFFL)
                | ((self.ReadByte() << 0x08) & 0xFF00L)
                | ((self.ReadByte() << 0x10) & 0xFF0000L)
                | ((self.ReadByte() << 0x18) & 0xFF000000L)
                | ((self.ReadByte() << 0x20) & 0xFF00000000L)
                | ((self.ReadByte() << 0x28) & 0xFF0000000000L);
        }
        public static long ReadUInt56(this BinaryReader self)
        {
            return (self.ReadByte() & 0xFFL)
                | ((self.ReadByte() << 0x08) & 0xFF00L)
                | ((self.ReadByte() << 0x10) & 0xFF0000L)
                | ((self.ReadByte() << 0x18) & 0xFF000000L)
                | ((self.ReadByte() << 0x20) & 0xFF00000000L)
                | ((self.ReadByte() << 0x28) & 0xFF0000000000L)
                | ((self.ReadByte() << 0x30) & 0xFF000000000000L);
        }

        /// <summary>
        /// v값은 -2~2 사이인 값을 2byte로 읽어옵니다.
        /// </summary>
        public static float ReadHalf(this BinaryReader self)
        {
            const int MAX = 2;
            const int DIV = MAX * 2;
            const float ONE = (ushort.MaxValue / DIV);
            return self.ReadUInt16() / ONE - MAX;
        }
        /// <summary>
        /// v값은 0~1 사이인 값을 2byte로 읽어옵니다.
        /// </summary>
        public static float ReadHalf01(this BinaryReader self)
        {
            const float VAL = ushort.MaxValue;
            return self.ReadUInt16() / VAL;
        }
        /// <summary>
        /// v값은 -2~2 사이인 값을 1byte로 읽어옵니다.
        /// </summary>
        public static float ReadFixed(this BinaryReader self)
        {
            const int MAX = 2;
            const int DIV = MAX * 2;
            const float ONE = (byte.MaxValue / DIV);
            return self.ReadByte() / ONE - MAX;
        }
        /// <summary>
        /// v값은 0~1 사이인 값을 1byte로 읽어옵니다.
        /// </summary>
        public static float ReadFixed01(this BinaryReader self)
        {
            const float VAL = byte.MaxValue;
            return self.ReadByte() / VAL;
        }


        public static byte[] ReadByteArray(this BinaryReader self) => self.ReadBytes((int)(self.ReadPackedUInt32() & 0x7FFFFFFF));
        public static byte[] ReadByteArraySize8(this BinaryReader self) => self.ReadBytes(self.ReadByte());
        public static byte[] ReadByteArraySize16(this BinaryReader self) => self.ReadBytes(self.ReadUInt16());
        public static byte[] ReadByteArraySize32(this BinaryReader self) => self.ReadBytes(self.ReadInt32());


        public static T ReadEnum8<T>(this BinaryReader self) where T : Enum => (T)Enum.ToObject(typeof(T), self.ReadSByte());
        public static T ReadEnumU8<T>(this BinaryReader self) where T : Enum => (T)Enum.ToObject(typeof(T), self.ReadByte());
        public static T ReadEnum16<T>(this BinaryReader self) where T : Enum => (T)Enum.ToObject(typeof(T), self.ReadInt16());
        public static T ReadEnumU16<T>(this BinaryReader self) where T : Enum => (T)Enum.ToObject(typeof(T), self.ReadUInt16());
        public static T ReadEnum32<T>(this BinaryReader self) where T : Enum => (T)Enum.ToObject(typeof(T), self.ReadInt32());
        public static T ReadEnumU32<T>(this BinaryReader self) where T : Enum => (T)Enum.ToObject(typeof(T), self.ReadUInt32());
        public static T ReadEnum64<T>(this BinaryReader self) where T : Enum => (T)Enum.ToObject(typeof(T), self.ReadInt64());
        public static T ReadEnumU64<T>(this BinaryReader self) where T : Enum => (T)Enum.ToObject(typeof(T), self.ReadUInt64());


    }
}