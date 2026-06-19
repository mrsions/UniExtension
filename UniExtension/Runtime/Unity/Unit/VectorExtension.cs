// Copyright (c) 2016 Sions
// 
// UniExtension version//
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

using UnityEngine;

namespace UnityEngine
{
public static class VectorExtensions
{
    #region Vector3
    public static Vector3 ZEROe3(this Vector3 a)
    {
        if (Mathf.Abs(a.x) < 0.001)
            a.x = 0;
        if (Mathf.Abs(a.y) < 0.001)
            a.y = 0;
        if (Mathf.Abs(a.z) < 0.001)
            a.z = 0;
        return a;
    }
    public static Vector3 ZEROe4(this Vector3 a)
    {
        if (Mathf.Abs(a.x) < 0.0001)
            a.x = 0;
        if (Mathf.Abs(a.y) < 0.0001)
            a.y = 0;
        if (Mathf.Abs(a.z) < 0.0001)
            a.z = 0;
        return a;
    }
    public static Vector3 ZEROe5(this Vector3 a)
    {
        if (Mathf.Abs(a.x) < 0.00001)
            a.x = 0;
        if (Mathf.Abs(a.y) < 0.00001)
            a.y = 0;
        if (Mathf.Abs(a.z) < 0.00001)
            a.z = 0;
        return a;
    }
    public static Vector3 ToVector3(this float a) { return new Vector3(a, a, a); }
    public static Vector3 ToForward(this float a) { return new Vector3(0, 0, a); }
    public static Vector3 ToUp(this float a) { return new Vector3(0, a, 0); }
    public static Vector3 ToRight(this float a) { return new Vector3(a, 0); }

    public static Vector3 ResetY(this Vector3 a) { a.y = 0; return a; }
    public static Vector3 ResetX(this Vector3 a) { a.x = 0; return a; }
    public static Vector3 ResetZ(this Vector3 a) { a.z = 0; return a; }
    public static Vector3 OnlyX(this Vector3 a) { return new Vector3(a.x, 0, 0); }
    public static Vector3 OnlyY(this Vector3 a) { return new Vector3(0, a.y, 0); }
    public static Vector3 OnlyZ(this Vector3 a) { return new Vector3(0, 0, a.z); }
    public static Vector3 SetX(this Vector3 a, float x) { a.x = x; return a; }
    public static Vector3 SetY(this Vector3 a, float y) { a.y = y; return a; }
    public static Vector3 SetZ(this Vector3 a, float z) { a.z = z; return a; }
    public static Vector3 Adds(this Vector3 a, float v) { a.x += v; a.y += v; a.z += v; return a; }
    public static Vector3 AddX(this Vector3 a, float x) { a.x += x; return a; }
    public static Vector3 AddY(this Vector3 a, float y) { a.y += y; return a; }
    public static Vector3 AddZ(this Vector3 a, float z) { a.z += z; return a; }

    public static Vector3 SetXY(this Vector3 a, float xy) { a.x = a.y = xy; return a; }
    public static Vector3 SetYZ(this Vector3 a, float yz) { a.y = a.z = yz; return a; }
    public static Vector3 SetXZ(this Vector3 a, float xz) { a.x = a.z = xz; return a; }

    public static Vector3 MultiX(this Vector3 a, float x) { a.x *= x; return a; }
    public static Vector3 MultiY(this Vector3 a, float y) { a.y *= y; return a; }
    public static Vector3 MultiZ(this Vector3 a, float z) { a.z *= z; return a; }
    public static Vector3 MultiXY(this Vector3 a, float xy) { a.x *= xy; a.y *= xy; return a; }
    public static Vector3 MultiYZ(this Vector3 a, float yz) { a.y *= yz; a.z *= yz; return a; }
    public static Vector3 MultiXZ(this Vector3 a, float xz) { a.x *= xz; a.z *= xz; return a; }

    public static Vector3 SwapXY(this Vector3 a) { var t = a.x; a.x = a.y; a.y = t; return a; }
    public static Vector3 SwapYZ(this Vector3 a) { var t = a.y; a.y = a.z; a.z = t; return a; }
    public static Vector3 SwapXZ(this Vector3 a) { var t = a.x; a.x = a.z; a.z = t; return a; }

    public static Vector3 Round(this Vector3 a, float round = 1f)
    {
        a.x = Mathf.RoundToInt(a.x / round) * round;
        a.y = Mathf.RoundToInt(a.y / round) * round;
        a.z = Mathf.RoundToInt(a.z / round) * round;
        return a;
    }
    public static Vector3 Multiply(this Vector3 a, Vector3 b)
    {
        a.x *= b.x;
        a.y *= b.y;
        a.z *= b.z;
        return a;
    }
    public static Vector3 Divide(this Vector3 a, Vector3 b)
    {
        a.x /= b.x;
        a.y /= b.y;
        a.z /= b.z;
        return a;
    }

    public static Quaternion ToLookQuaternion(this Vector3 direction)
    {
        return Quaternion.LookRotation(direction);
    }

    public static Quaternion ToQuaternion(this Vector3 euler)
    {
        return Quaternion.Euler(euler);
    }

    public static Vector3[] Plus(this Vector3[] src, Vector3 v)
    {
        var result = new Vector3[src.Length];
        for (var i = 0; i < src.Length; i++)
        {
            result[i] = src[i] + v;
        }
        return result;
    }
    public static Vector3[] Multiply(this Vector3[] src, Vector3 v)
    {
        var result = new Vector3[src.Length];
        for (var i = 0; i < src.Length; i++)
        {
            result[i] = src[i].Multiply(v);
        }
        return result;
    }
    public static Vector3[] Divide(this Vector3[] src, Vector3 v)
    {
        var result = new Vector3[src.Length];
        for (var i = 0; i < src.Length; i++)
        {
            result[i] = src[i].Divide(v);
        }
        return result;
    }

    public static float MaxValue(this Vector3 src)
    {
        return Mathf.Max(src.x, src.y, src.z);
    }
    public static float MinValue(this Vector3 src)
    {
        return Mathf.Min(src.x, src.y, src.z);
    }
    public static float Average(this Vector3 src)
    {
        const float AVERAGE_COUNT = 1f / 3;
        return (src.x + src.y + src.z) * AVERAGE_COUNT;
    }

    public static Vector3 Clamp(this Vector3 src, Vector3 min, Vector3 max)
    {
        return new Vector3(Mathf.Clamp(src.x, min.x, max.x), Mathf.Clamp(src.y, min.y, max.y), Mathf.Clamp(src.z, min.z, max.z));
    }

    public static Vector3 NearPoint(this Vector3[] src, Vector3 point)
    {
        var result = point;
        float minDistance = float.MaxValue, dist;
        for (var i = 0; i < src.Length; i++)
        {
            dist = Vector3.Distance(src[i], point);
            if (dist < minDistance)
            {
                result = src[i];
                minDistance = dist;
            }
        }
        return result;
    }

    public static bool IsNaN(this Vector3 pos)
    {
        return float.IsNaN(pos.x) || float.IsNaN(pos.y) || float.IsNaN(pos.z);
    }

    public static string ToString(this Vector3 pos, float count)
    {
        var f = "f" + count;
        return string.Format("({0}, {1}, {2})", pos.x.ToString(f), pos.y.ToString(f), pos.z.ToString(f));
    }
    public static Vector3 Average(this Vector3[] q)
    {
        var result = Vector3.zero;
        for (var i = 0; i < q.Length; i++)
        {
            result.x += q[i].x;
            result.y += q[i].y;
            result.z += q[i].z;
        }
        return result / q.Length;
    }

    public static float RandomX2Y(this Vector3 src)
    {
        return UnityEngine.Random.Range(src.x, src.y);
    }
    public static float RandomX2Z(this Vector3 src)
    {
        return UnityEngine.Random.Range(src.x, src.z);
    }
    public static float RandomY2Z(this Vector3 src)
    {
        return UnityEngine.Random.Range(src.y, src.z);
    }
    public static Vector2 ToV2XZ(this Vector3 a) { return new Vector2(a.x, a.z); }
    #endregion

    #region Vector3Int
    public static Vector3Int ToV3Int(this Vector3 a) => new((int)a.x, (int)a.y, (int)a.z);
    public static Vector3Int ToV3RoundInt(this Vector3 a) => Vector3Int.RoundToInt(a);
    public static Vector3Int ToV3CeilInt(this Vector3 a) => Vector3Int.CeilToInt(a);
    public static Vector3Int ToV3FloorInt(this Vector3 a) => Vector3Int.FloorToInt(a);

    public static int MaxValue(this Vector3Int src)
    {
        return Mathf.Max(src.x, src.y, src.z);
    }
    public static int MinValue(this Vector3Int src)
    {
        return Mathf.Min(src.x, src.y, src.z);
    }
    public static int MaxValueXZ(this Vector3Int src)
    {
        return Mathf.Max(src.x, src.z);
    }
    public static int MinValueXZ(this Vector3Int src)
    {
        return Mathf.Min(src.x, src.z);
    }
    public static float Average(this Vector3Int src)
    {
        const float AVERAGE_COUNT = 1f / 3;
        return (src.x + src.y + src.z) * AVERAGE_COUNT;
    }
    #endregion

    #region Vector2
    public static Vector2 ZEROe3(this Vector2 a)
    {
        if (Mathf.Abs(a.x) < 0.001)
            a.x = 0;
        if (Mathf.Abs(a.y) < 0.001)
            a.y = 0;
        return a;
    }
    public static Vector2 ZEROe4(this Vector2 a)
    {
        if (Mathf.Abs(a.x) < 0.0001)
            a.x = 0;
        if (Mathf.Abs(a.y) < 0.0001)
            a.y = 0;
        return a;
    }
    public static Vector2 ZEROe5(this Vector2 a)
    {
        if (Mathf.Abs(a.x) < 0.00001)
            a.x = 0;
        if (Mathf.Abs(a.y) < 0.00001)
            a.y = 0;
        return a;
    }
    public static Vector2 ResetY(this Vector2 a) { a.y = 0; return a; }
    public static Vector2 ResetX(this Vector2 a) { a.x = 0; return a; }
    public static Vector2 OnlyX(this Vector2 a) { return new Vector2(a.x, 0); }
    public static Vector2 OnlyY(this Vector2 a) { return new Vector2(0, a.y); }
    public static Vector2 SetX(this Vector2 a, float x) { a.x = x; return a; }
    public static Vector2 SetY(this Vector2 a, float y) { a.y = y; return a; }

    public static Vector2 SetXY(this Vector2 a, float xy) { a.x = a.y = xy; return a; }

    public static Vector2 MultiX(this Vector2 a, float x) { a.x *= x; return a; }
    public static Vector2 MultiY(this Vector2 a, float y) { a.y *= y; return a; }
    public static Vector2 MultiXY(this Vector2 a, float xy) { a.x *= xy; a.x *= xy; return a; }

    public static Vector2 Multiply(this Vector2 a, Vector2 b)
    {
        a.x *= b.x;
        a.y *= b.y;
        return a;
    }
    public static Vector2 Divide(this Vector2 a, Vector2 b)
    {
        a.x /= b.x;
        a.y /= b.y;
        return a;
    }

    public static Vector2[] Plus(this Vector2[] src, Vector2 v)
    {
        var result = new Vector2[src.Length];
        for (var i = 0; i < src.Length; i++)
        {
            result[i] = src[i] + v;
        }
        return result;
    }
    public static Vector2[] Multiply(this Vector2[] src, Vector2 v)
    {
        var result = new Vector2[src.Length];
        for (var i = 0; i < src.Length; i++)
        {
            result[i] = src[i].Multiply(v);
        }
        return result;
    }
    public static Vector2[] Divide(this Vector2[] src, Vector2 v)
    {
        var result = new Vector2[src.Length];
        for (var i = 0; i < src.Length; i++)
        {
            result[i] = src[i].Divide(v);
        }
        return result;
    }

    public static float MaxValue(this Vector2 src)
    {
        return Mathf.Max(src.x, src.y);
    }
    public static float MinValue(this Vector2 src)
    {
        return Mathf.Min(src.x, src.y);
    }
    public static float Average(this Vector2 src)
    {
        return (src.x + src.y) * 0.5f;
    }

    public static string ToString(this Vector2 pos, float count)
    {
        var f = "f" + count;
        return string.Format("({0}, {1})", pos.x.ToString(f), pos.y.ToString(f));
    }

    public static float RandomX2Y(this Vector2 src)
    {
        return UnityEngine.Random.Range(src.x, src.y);
    }

    public static Vector3 ToV3XZ(this Vector2 a) { return new Vector3(a.x, 0, a.y); }

    public static float ToDirectionAngles(this Vector2 p)
    {
        var angle = Mathf.Atan2(p.x, p.y) * Mathf.Rad2Deg;
        if (angle < 0)
        {
            angle += 360;
        }
        return angle;
    }
    public static float ToDirectionSignedAngles(this Vector2 p)
    {
        var angle = p.ToDirectionAngles();
        if (angle > 180)
        {
            angle -= 360;
        }
        return angle;
    }
    #endregion

    #region Vector2Int
    public static Vector2Int ToV2Int(this Vector2 a) => new((int)a.x, (int)a.y);
    public static Vector2Int ToV2RoundInt(this Vector2 a) => new(Mathf.RoundToInt(a.x), Mathf.RoundToInt(a.y));
    public static Vector2Int ToV2IntXZ(this Vector3 a) => new((int)a.x, (int)a.z);
    public static Vector2Int ToV2RoundIntXZ(this Vector3 a) => new(Mathf.RoundToInt(a.x), Mathf.RoundToInt(a.z));
    public static Vector2Int ToV2IntXZ(this Vector3Int a) => new(a.x, a.z);
    public static Vector3Int ToV3IntXZ(this Vector2Int a) => new(a.x, 0, a.y);
    public static Vector3 ToV3XZ(this Vector2Int a) => new(a.x, 0, a.y);
    public static Vector2 ToV2(this Vector2Int a) => new(a.x, a.y);

    public static Vector2Int Normalize(this Vector2Int v)
    {
        return new Vector2Int(v.x / Mathf.Max(1, Mathf.Abs(v.x)), v.y / Mathf.Max(1, Mathf.Abs(v.y)));
    }
    public static int Max(this Vector2Int v)
    {
        return (v.x > v.y) ? v.x : v.y;
    }
    #endregion Vector2Int

    #region ABS
    public static Vector4 Abs(this Vector4 a) { return new Vector4(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z), Mathf.Abs(a.w)); }
    public static Vector3 Abs(this Vector3 a) { return new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z)); }
    public static Vector2 Abs(this Vector2 a) { return new Vector2(Mathf.Abs(a.x), Mathf.Abs(a.y)); }
    public static Vector3Int Abs(this Vector3Int a) { return new Vector3Int(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z)); }
    public static Vector2Int Abs(this Vector2Int a) { return new Vector2Int(Mathf.Abs(a.x), Mathf.Abs(a.y)); }
    #endregion
}
}
