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
public static class TextureExtensions
{
    public static Texture2D ToClone(this Texture2D texture)
    {
        var tex = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        tex.SetPixels(texture.GetPixels());
        return tex;
    }
    public static Texture2D FlipHorizontal(this Texture2D texture)
    {
        for (var i = 0; i < texture.width / 2; i++)
        {
            var left = i;
            var right = texture.width - i - 1;
            var leftColors = texture.GetPixels(left, 0, 1, texture.height);
            var rightColors = texture.GetPixels(right, 0, 1, texture.height);
            texture.SetPixels(left, 0, 1, texture.height, rightColors);
            texture.SetPixels(right, 0, 1, texture.height, leftColors);
        }
        return texture;
    }
    public static Texture2D FlipVertical(this Texture2D texture)
    {
        for (var i = 0; i < texture.height / 2; i++)
        {
            var top = i;
            var bottom = texture.height - i - 1;
            var topColors = texture.GetPixels(0, top, texture.width, 1);
            var bottomColors = texture.GetPixels(0, bottom, texture.width, 1);
            texture.SetPixels(0, top, texture.width, 1, bottomColors);
            texture.SetPixels(0, bottom, texture.width, 1, topColors);
        }
        return texture;
    }
    public static Texture2D Rotate90Right(this Texture2D texture)
    {
        var src = texture.GetPixels();
        var dst = new Color[src.Length];
        for (var x = 0; x < texture.width; x++)
        {
            for (var y = 0; y < texture.height; y++)
            {
                var dx = y;
                var dy = texture.width - x - 1;

                var i = x + y * texture.width;
                var di = dx + dy * texture.height;

                dst[di] = src[i];
            }
        }

        var dstTex = texture;
        if (texture.width != texture.height) dstTex = new Texture2D(texture.height, texture.width, TextureFormat.RGBA32, false);
        dstTex.SetPixels(dst);
        return dstTex;
    }
    public static Texture2D Rotate90Left(this Texture2D texture)
    {
        var src = texture.GetPixels();
        var dst = new Color[src.Length];
        for (var x = 0; x < texture.width; x++)
        {
            for (var y = 0; y < texture.height; y++)
            {
                var dx = texture.height - y - 1;
                var dy = x;

                var i = x + y * texture.width;
                var di = dx + dy * texture.height;

                dst[di] = src[i];
            }
        }

        var dstTex = texture;
        if (texture.width != texture.height) dstTex = new Texture2D(texture.height, texture.width, TextureFormat.RGBA32, false);
        dstTex.SetPixels(dst);
        return dstTex;
    }
    public static Texture2D Rotate180(this Texture2D texture)
    {
        var src = texture.GetPixels();
        var dst = new Color[src.Length];
        for (var x = 0; x < texture.width; x++)
        {
            for (var y = 0; y < texture.height; y++)
            {
                var dx = texture.width - x - 1;
                var dy = texture.height - y - 1;

                var i = x + y * texture.width;
                var di = dx + dy * texture.width;

                dst[di] = src[i];
            }
        }
        texture.SetPixels(dst);
        return texture;
    }
}
}
