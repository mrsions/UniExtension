using UnityEngine;

namespace System.IO
{
    public static class BinaryReaderUnityExtension
    {
        public static Color32 ReadColorRGB(this BinaryReader self)
        {
            return new Color32(self.ReadByte(), self.ReadByte(), self.ReadByte(), (byte)0xFF);
        }
        public static Color32 ReadColorRGBA(this BinaryReader self)
        {
            return new Color32(self.ReadByte(), self.ReadByte(), self.ReadByte(), self.ReadByte());
        }
        public static Vector2 ReadVector2(this BinaryReader self)
        {
            return new Vector2(self.ReadSingle(), self.ReadSingle());
        }
        public static Vector3 ReadVector3(this BinaryReader self)
        {
            return new Vector3(self.ReadSingle(), self.ReadSingle(), self.ReadSingle());
        }
        public static Vector3Int ReadVector3Int(this BinaryReader self)
        {
            return new Vector3Int(self.ReadInt32(), self.ReadInt32(), self.ReadInt32());
        }
        /// <summary>
        /// 벡터를 3바이트로 사용. (-1 ~ 1)
        /// </summary>
        public static Vector3 ReadNormal3B(this BinaryReader self)
        {
            const float SCALE = 1f / sbyte.MaxValue;
            return new Vector3(self.ReadSByte() * SCALE, self.ReadSByte() * SCALE, self.ReadSByte() * SCALE);
        }
        /// <summary>
        /// 벡터를 4바이트로 사용. (-1 ~ 1)
        /// </summary>
        public static Vector3 ReadNormal4B(this BinaryReader self)
        {
            const int MAX_XZ = 1023; // (2^10-1) 11비트를 사용한 signed 최대 값 : 1023
            const int MAX_Y = 511;  // (2^9-1) 10비트를 사용한 signed 최대 값 : 511
            const float SCALE_XZ = 1f / MAX_XZ;
            const float SCALE_Y = 1f / MAX_Y;

            var memory = self.ReadInt32();
            var result = Vector3.zero;

            result.x = ((memory >> 3) & MAX_XZ) * ((memory & 1) == 1 ? -1 : 1) * SCALE_XZ;
            result.y = ((memory >> 13) & MAX_Y) * ((memory & 2) == 2 ? -1 : 1) * SCALE_Y;
            result.z = ((memory >> 22) & MAX_XZ) * ((memory & 4) == 4 ? -1 : 1) * SCALE_XZ;
            return result;
        }
        /// <summary>
        /// 벡터를 5바이트로 사용. (-1 ~ 1)
        /// </summary>
        public static Vector3 ReadNormal5B(this BinaryReader self)
        {
            const float SCALE = 1f / Int16.MaxValue;
            return new Vector3(self.ReadInt16() * SCALE, self.ReadInt16() * SCALE, self.ReadInt16() * SCALE);
        }

        public static Vector3 ReadEuler(this BinaryReader self)
        {
            return new Vector3(self.ReadSingle(), self.ReadSingle(), self.ReadSingle());
        }
        public static Vector3 ReadEuler24(this BinaryReader self)
        {
            const float CALC_VALUE = (1f / 255f * 360f);
            return new Vector3(self.ReadByte() * CALC_VALUE, self.ReadByte() * CALC_VALUE, self.ReadByte() * CALC_VALUE);
        }
        public static Vector3 ReadEuler32(this BinaryReader self)
        {
            const float SCALE_XZ = 1f / (2048f / 360f); // 11비트를 360도로 나눈 값.
            const float SCALE_Y = 1f / (1024f / 360f);  // 10비트를 360도로 나눈 값
            const int MAX_XZ = 2047;
            const int MAX_Y = 1023;

            var memory = self.ReadInt32();
            var result = Vector3.zero;

            result.x = ((memory) & MAX_XZ) * SCALE_XZ;
            result.y = ((memory >> 11) & MAX_Y) * SCALE_Y;
            result.z = ((memory >> 22) & MAX_XZ) * SCALE_XZ;
            return result;
        }
        public static Quaternion ReadQuaternion(this BinaryReader self)
        {
            return new Quaternion(self.ReadSingle(), self.ReadSingle(), self.ReadSingle(), self.ReadSingle());
        }
        public static Quaternion ReadQuaternion64(this BinaryReader self)
        {
            return new Quaternion(self.ReadHalf(), self.ReadHalf(), self.ReadHalf(), self.ReadHalf());
        }
        public static Rect ReadRect(this BinaryReader self)
        {
            return new Rect(self.ReadSingle(), self.ReadSingle(), self.ReadSingle(), self.ReadSingle());
        }
        public static Bounds ReadBounds(this BinaryReader self)
        {
            return new Bounds(self.ReadVector3(), self.ReadVector3());
        }
        public static Pose ReadPose(this BinaryReader self)
        {
            return new Pose(self.ReadVector3(), self.ReadQuaternion());
        }
    }
}