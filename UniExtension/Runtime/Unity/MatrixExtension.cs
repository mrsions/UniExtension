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
public static class MatrixExtension
{
    public static float ToFieldOfView(this Matrix4x4 proj)
    {
        var t = proj.m11;
        var fov = Mathf.Atan(1.0f / t) * 2.0f * Mathf.Rad2Deg;
        return fov;
    }
    public static float ToAspectRatio(this Matrix4x4 proj)
    {
        return proj.m11 / proj.m00;
    }

    public static Vector3 GetPosition(this Matrix4x4 matrix)
    {
        return new Vector3(matrix.m03, matrix.m13, matrix.m23);
    }

    public static Vector3 GetForwardVector(this Matrix4x4 matrix)
    {
        return new Vector3(matrix.m02, matrix.m12, matrix.m22);
    }

    public static Vector3 GetRightVector(this Matrix4x4 matrix)
    {
        return new Vector3(matrix.m00, matrix.m10, matrix.m20);
    }

    public static Vector3 GetUpVector(this Matrix4x4 matrix)
    {
        return new Vector3(matrix.m01, matrix.m11, matrix.m21);
    }

    public static Matrix4x4 SetPosition(this Matrix4x4 matrix, Vector3 val)
    {
        matrix.m03 = val.x; matrix.m13 = val.y; matrix.m23 = val.z;
        return matrix;
    }

    public static Matrix4x4 SetForwardVector(this Matrix4x4 matrix, Vector3 val)
    {
        matrix.m02 = val.x; matrix.m12 = val.y; matrix.m22 = val.z;
        return matrix;
    }

    public static Matrix4x4 SetRightVector(this Matrix4x4 matrix, Vector3 val)
    {
        matrix.m00 = val.x; matrix.m10 = val.y; matrix.m20 = val.z;
        return matrix;
    }

    public static Matrix4x4 SetUpVector(this Matrix4x4 matrix, Vector3 val)
    {
        matrix.m01 = val.x; matrix.m11 = val.y; matrix.m21 = val.z;
        return matrix;
    }

    public static string ToStringTRS(this Matrix4x4 matrix)
    {
        return $"({matrix.GetPosition()}, {matrix.rotation.eulerAngles}, {matrix.lossyScale})";
    }
}
}
