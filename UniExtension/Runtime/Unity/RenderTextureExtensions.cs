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
public static class RenderTextureExtensions
{
    public static Texture2D ToTexture(this RenderTexture rt, Texture2D tex = null, int padding = 0, bool isTransparent = true)
    {
        TextureFormat format;
        switch (rt.format)
        {
            case RenderTextureFormat.Depth:
                format = TextureFormat.Alpha8;
                break;
            case RenderTextureFormat.ARGBHalf:
                format = isTransparent ? TextureFormat.ARGB4444 : TextureFormat.RGB565;
                break;
            case RenderTextureFormat.Shadowmap:
                format = TextureFormat.Alpha8;
                break;
            case RenderTextureFormat.RGB565:
                format = TextureFormat.RGB565;
                break;
            case RenderTextureFormat.ARGB4444:
                format = isTransparent ? TextureFormat.ARGB4444 : TextureFormat.RGB565;
                break;
            case RenderTextureFormat.ARGBFloat:
                format = isTransparent ? TextureFormat.RGBAFloat : TextureFormat.RGB24;
                break;
            case RenderTextureFormat.RGFloat:
                format = TextureFormat.RGFloat;
                break;
            case RenderTextureFormat.RGHalf:
                format = TextureFormat.RGHalf;
                break;
            case RenderTextureFormat.RFloat:
                format = TextureFormat.RFloat;
                break;
            case RenderTextureFormat.RHalf:
                format = TextureFormat.RHalf;
                break;
            case RenderTextureFormat.BGRA32:
                format = isTransparent ? TextureFormat.BGRA32 : TextureFormat.RGB24;
                break;
            default:
                format = isTransparent ? TextureFormat.ARGB32 : TextureFormat.RGB24;
                break;
        }

        var rwidth = rt.width + padding * 2;
        var rheight = rt.height + padding * 2;

        if (!tex || tex.width != rwidth || tex.height != rheight || tex.format != format)
            tex = new Texture2D(rwidth, rheight, format, false, true);

        var brt = RenderTexture.active;

        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), padding, padding);
        tex.Apply();
        RenderTexture.active = brt;

        //if (backgroundColor != default)
        //{
        //    var trt = RenderTexture.GetTemporary(rt.descriptor);

        //    Graphics.Blit(Texture2D.blackTexture, trt);
        //    Graphics.Blit(rt, trt);
        //    Graphics.Blit(trt, rt);
        //    RenderTexture.ReleaseTemporary(trt);
        //}

        return tex;
    }
    public static byte[] EncodePNG(this RenderTexture rt)
    {
        return rt.ToTexture().EncodeToPNG();
    }
}
}
