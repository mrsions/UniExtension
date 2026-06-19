using UnityEngine;

namespace System.IO
{
    public static class BinaryWriterUnityExtension
    {
        public static void WriteColorRGB(this BinaryWriter self, Color32 value)
        {
            self.Write(value.r);
            self.Write(value.g);
            self.Write(value.b);
        }
        public static void WriteColorRGBA(this BinaryWriter self, Color32 value)
        {
            self.Write(value.r);
            self.Write(value.g);
            self.Write(value.b);
            self.Write(value.a);
        }
        public static void WriteVector2(this BinaryWriter self, Vector2 value)
        {
            self.Write(value.x);
            self.Write(value.y);
        }
        public static void WriteVector3(this BinaryWriter self, Vector3 value)
        {
            self.Write(value.x);
            self.Write(value.y);
            self.Write(value.z);
        }
        public static void WriteVector3Int(this BinaryWriter self, Vector3Int value)
        {
            self.Write(value.x);
            self.Write(value.y);
            self.Write(value.z);
        }
        /// <summary>
        /// 벡터를 3바이트로 사용. (-1 ~ 1)
        /// </summary>
        public static void WriteNormal3B(this BinaryWriter self, Vector3 value)
        {
            self.Write((sbyte)(Mathf.Clamp(value.x, -1, 1) * sbyte.MaxValue));
            self.Write((sbyte)(Mathf.Clamp(value.y, -1, 1) * sbyte.MaxValue));
            self.Write((sbyte)(Mathf.Clamp(value.z, -1, 1) * sbyte.MaxValue));
        }
        /// <summary>
        /// 벡터를 4바이트로 사용. (-1 ~ 1)
        /// </summary>
        public static void WriteNormal4B(this BinaryWriter self, Vector3 value)
        {
            const int MAX_XZ = 1023; // (2^10-1) 11비트를 사용한 signed 최대 값 : 1023
            const int MAX_Y = 511;  // (2^9-1) 10비트를 사용한 signed 최대 값 : 511

            var memory = 0;

            memory |= (value.x < 0 ? 1 : 0) << 0;
            memory |= (value.y < 0 ? 1 : 0) << 1;
            memory |= (value.z < 0 ? 1 : 0) << 2;
            memory |= (int)(Mathf.Clamp(Mathf.Abs(value.x), -1, 1) * MAX_XZ) << 3;
            memory |= (int)(Mathf.Clamp(Mathf.Abs(value.y), -1, 1) * MAX_Y) << 13;
            memory |= (int)(Mathf.Clamp(Mathf.Abs(value.z), -1, 1) * MAX_XZ) << 22;

            self.Write(memory);
        }
        /// <summary>
        /// 벡터를 5바이트로 사용. (-1 ~ 1)
        /// </summary>
        public static void WriteNormal5B(this BinaryWriter self, Vector3 value)
        {
            self.Write((short)(Mathf.Clamp(value.x, -1, 1) * Int16.MaxValue));
            self.Write((short)(Mathf.Clamp(value.y, -1, 1) * Int16.MaxValue));
            self.Write((short)(Mathf.Clamp(value.z, -1, 1) * Int16.MaxValue));
        }

        public static void WriteEuler(this BinaryWriter self, Vector3 value)
        {
            self.Write(value.x);
            self.Write(value.y);
            self.Write(value.z);
        }
        public static void WriteEuler24(this BinaryWriter self, Vector3 value)
        {
            const float CALC_VALUE = (1f / 360f * 255);

            self.Write((byte)(((value.x + 3600) % 360) * CALC_VALUE));
            self.Write((byte)(((value.y + 3600) % 360) * CALC_VALUE));
            self.Write((byte)(((value.z + 3600) % 360) * CALC_VALUE));
        }
        public static void WriteEuler32(this BinaryWriter self, Vector3 value)
        {
            const float SCALE_XZ = 2048 / 360f; // 11비트를 360도로 나눈 값.
            const float SCALE_Y = 1024 / 360f;  // 10비트를 360도로 나눈 값

            var memory = 0;
            memory |= (int)(((value.x + 3600) % 360) * SCALE_XZ);
            memory |= (int)(((value.y + 3600) % 360) * SCALE_Y) << 11;
            memory |= (int)(((value.z + 3600) % 360) * SCALE_XZ) << 21;

            self.Write(memory);
        }
        public static void WriteQuaternion(this BinaryWriter self, Quaternion value)
        {
            self.Write(value.x);
            self.Write(value.y);
            self.Write(value.z);
            self.Write(value.w);
        }
        public static void WriteQuaternion64(this BinaryWriter self, Quaternion value)
        {
            self.WriteHalf(value.x);
            self.WriteHalf(value.y);
            self.WriteHalf(value.z);
            self.WriteHalf(value.w);
        }

        public static void WriteRect(this BinaryWriter self, Rect value)
        {
            self.Write(value.x);
            self.Write(value.y);
            self.Write(value.width);
            self.Write(value.height);
        }
        public static void WriteBounds(this BinaryWriter self, Bounds value)
        {
            self.WriteVector3(value.center);
            self.WriteVector3(value.extents);
        }
        public static void WritePose(this BinaryWriter self, Pose v)
        {
            self.WriteVector3(v.position);
            self.WriteQuaternion(v.rotation);
        }
    }
}