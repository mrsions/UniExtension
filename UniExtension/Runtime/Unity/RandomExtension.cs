using UnityEngine;

namespace UniExtension
{
    public static class RandomExtension
    {
        public static bool RandomTo(this bool from, float percent = 0.5f)
        {
            return Random.value < percent ? from : !from;
        }

        public static int RandomTo(this int from, int to)
        {
            return Random.Range(from, to);
        }
        public static int RandomToInverse(this int from)
        {
            if (from < 0)
            {
                return Random.Range(from, -from);
            }
            else
            {
                return Random.Range(-from, from);
            }
        }

        public static int RandomFrom(this int val, int from)
        {
            return Random.Range(from, val);
        }
        public static int RandomFromZero(this int val)
        {
            return Random.Range(0, val);
        }
        public static int RandomFromMinus(this int val)
        {
            return Random.Range(-val, val);
        }

        public static float RandomFrom(this float val, float from)
        {
            return Random.Range(from, val);
        }
        public static float RandomFromZero(this float val)
        {
            return Random.Range(0, val);
        }
        public static float RandomFromHalf(this float val)
        {
            return Random.Range(val * 0.5f, val);
        }
        public static float RandomFromMinus(this float val)
        {
            return Random.Range(-val, val);
        }
        public static bool IsInRandom(this float val, float length = 1f)
        {
            return Random.Range(0, length) < val;
        }

        public static Vector3 RandomFrom(this Vector3 val, Vector3 from)
        {
            return new Vector3(val.x.RandomFrom(from.x), val.y.RandomFrom(from.y), val.z.RandomFrom(from.z));
        }
        public static Vector3 RandomFromZero(this Vector3 val)
        {
            return val.RandomFrom(Vector3.zero);
        }
        public static Vector3 RandomFromMinus(this Vector3 val)
        {
            return val.RandomFrom(-val);
        }
    }
}
