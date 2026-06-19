
using URandom = UnityEngine.Random;

namespace UnityEngine
{
    public static class BoundsExtension
    {
        public static Vector3 Random(this Bounds b)
        {
            return new Vector3(URandom.Range(b.min.x, b.max.x), URandom.Range(b.min.y, b.max.y), URandom.Range(b.min.z, b.max.z));
        }
    }
}