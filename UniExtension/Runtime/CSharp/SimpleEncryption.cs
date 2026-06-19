using System.Security.Cryptography;
using System.Text;

namespace UniExtension
{
    public static class SimpleEncryption
    {

        private static MD5 md5;
        public static string Md5(this string msg) => Encoding.UTF8.GetBytes(msg).Md5();
        public static string Md5(this byte[] msg) { using (var enc = MD5.Create()) { return msg.Md5(enc); } }
        public static string Md5(this string msg, MD5 enc) => Encoding.UTF8.GetBytes(msg).Md5(enc);
        public static string Md5(this byte[] msg, MD5 enc)
        {
            if (enc == null) enc = md5 ?? (md5 = MD5.Create());
            return Hex2Str(enc.ComputeHash(msg));
        }

        private static SHA1 sha1;
        public static string Sha1(this string msg) => Encoding.UTF8.GetBytes(msg).Sha1();
        public static string Sha1(this byte[] msg) { using (var enc = SHA1.Create()) { return msg.Sha1(enc); } }
        public static string Sha1(this string msg, SHA1 enc) => Encoding.UTF8.GetBytes(msg).Sha1(enc);
        public static string Sha1(this byte[] msg, SHA1 enc)
        {
            if (enc == null) enc = sha1 ?? (sha1 = SHA1.Create());
            return Hex2Str(enc.ComputeHash(msg));
        }

        private static SHA256 sha256;
        public static string Sha256(this string msg) => Encoding.UTF8.GetBytes(msg).Sha256();
        public static string Sha256(this byte[] msg) { using (var enc = SHA256.Create()) { return msg.Sha256(enc); } }
        public static string Sha256(this string msg, SHA256 enc) => Encoding.UTF8.GetBytes(msg).Sha256(enc);
        public static string Sha256(this byte[] msg, SHA256 enc)
        {
            if (enc == null) enc = sha256 ?? (sha256 = SHA256.Create());
            return Hex2Str(enc.ComputeHash(msg));
        }

        private static string[] hexs = new string[256];
        static SimpleEncryption()
        {
            hexs = new string[256];
            for (var a = 0; a <= byte.MaxValue; a++)
            {
                hexs[a] = a.ToString("X2");
            }
        }
        public static string Hex2Str(byte[] data)
        {
            var sb = new StringBuilder(data.Length * 2);
            foreach (var b in data)
            {
                sb.Append(hexs[b]);
            }
            return sb.ToString();
        }
    }
}
