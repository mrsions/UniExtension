namespace System.IO
{
    public static partial class BinaryStreamExtension
    {
        const int BUFFER_SIZE = 40 * 1024;

        public static int GetSizePackedUInt32(this BinaryWriter self, uint value)
        {
            if (value <= 240)
            {
                return 1;
            }
            if (value <= 2287)
            {
                return 2;
            }
            if (value <= 67823)
            {
                return 3;
            }
            if (value <= 16777215)
            {
                return 4;
            }
            return 5;
        }

        public static int GetSizePackedUInt64(this BinaryWriter self, ulong value)
        {
            if (value <= 240)
            {
                return 1;
            }
            if (value <= 2287)
            {
                return 2;
            }
            if (value <= 67823)
            {
                return 3;
            }
            if (value <= 16777215)
            {
                return 4;
            }
            if (value <= 4294967295)
            {
                return 5;
            }
            if (value <= 1099511627775)
            {
                return 6;
            }
            if (value <= 281474976710655)
            {
                return 7;
            }
            if (value <= 72057594037927935)
            {
                return 8;
            }
            return 9;
        }

        public static void CopyTo(this Stream src, Stream dst, int length)
        {
            src.CopyTo(dst, length, new byte[BUFFER_SIZE]);
        }

        public static void CopyTo(this Stream src, Stream dst, int length, byte[] buffer)
        {
            var read = 0;
            var readed = 0;
            while (readed < length)
            {
                read = src.Read(buffer, 0, Math.Min(length - readed, buffer.Length));
                if (read < 0)
                {
                    throw new IOException("Can't read. less than 0");
                }
                dst.WriteStream(buffer, 0, read, buffer.Length);
                readed += read;
            }
        }

        private static void ThrowIfOutOfLength(int length, int maxValue)
        {
            if (length >= maxValue)
            {
                throw new ArgumentOutOfRangeException($"Array size must less than {byte.MaxValue}.");
            }
        }

    }
}