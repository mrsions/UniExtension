// Copyright (c) 2016 Sions
// 
// UniExtension version 1.0.0
//
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
public static class QuaternionExtension
{
    public static Quaternion Inverse(this Quaternion q)
    {
        return Quaternion.Inverse(q);
    }

    // q 에서 b까지의 delta를 구합니다.
    public static Quaternion DeltaTo(this Quaternion q, Quaternion b)
    {
        return Quaternion.Inverse(q) * b;
    }

    public static bool IsNAN(this Quaternion q)
    {
        return float.IsNaN(q.x) || float.IsNaN(q.y) || float.IsNaN(q.z) || float.IsNaN(q.w);
    }

    public static Quaternion Normalize(this Quaternion q)
    {
        var sum = Mathf.Sqrt(q.x * q.x) + (q.y * q.y) + (q.z * q.z) + (q.w * q.w);
        q.x /= sum;
        q.y /= sum;
        q.z /= sum;
        q.w /= sum;
        return q;
    }

    public static Vector3 Forward(this Quaternion q, float dist = 1)
    {
        return q * new Vector3(0, 0, dist);
    }

    /// <summary>
    /// Euler각의 x,z를 제거한 y의 방향만을 가리킵니다.
    /// </summary>
    /// <param name="q"></param>
    /// <param name="dist"></param>
    /// <returns></returns>
    public static Quaternion ToHeadingRotation(this Quaternion q)
    {
        return Quaternion.LookRotation(q.Forward(1).ResetY());
    }

    public static Quaternion Average(this Quaternion[] q)
    {
        var forward = Vector3.zero;
        var upward = Vector3.zero;

        for (var i = 0; i < q.Length; i++)
        {
            forward += q[i] * Vector3.forward;
            upward += q[i] * Vector3.up;
        }
        return Quaternion.LookRotation(forward, upward);
    }

    public static Quaternion ToDirectionLerp(this Quaternion from, Quaternion to, float ratio)
    {
        if (ratio <= 0) return from;
        else if (ratio >= 1) return to;
        var forward = Vector3.Lerp(from * Vector3.forward, to * Vector3.forward, ratio);
        var upward = Vector3.Lerp(from * Vector3.up, to * Vector3.up, ratio);
        return Quaternion.LookRotation(forward, upward);
    }

}
}
