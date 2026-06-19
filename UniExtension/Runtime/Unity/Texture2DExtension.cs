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

using System;
using UnityEngine;

namespace UnityEngine
{
public static class Texture2DExtension
{
    public static void FillColor(this Texture2D tex, Color color)
    {
        var colors = new Color[tex.width * tex.height];
        for (var i = 0; i < colors.Length; i++)
        {
            colors[i] = color;
        }
        tex.SetPixels(colors);
    }

    public static void DrawLine(this Texture2D tex, int x0, int y0, int x1, int y1, Color col)
    {
        var dy = (int)(y1 - y0);
        var dx = (int)(x1 - x0);
        int stepx, stepy;

        if (dy < 0)
        { dy = -dy; stepy = -1; }
        else
        { stepy = 1; }
        if (dx < 0)
        { dx = -dx; stepx = -1; }
        else
        { stepx = 1; }
        dy <<= 1;
        dx <<= 1;

        float fraction = 0;

        tex.SetPixel(x0, y0, col);
        if (dx > dy)
        {
            fraction = dy - (dx >> 1);
            while (Mathf.Abs(x0 - x1) > 1)
            {
                if (fraction >= 0)
                {
                    y0 += stepy;
                    fraction -= dx;
                }
                x0 += stepx;
                fraction += dy;
                tex.SetPixel(x0, y0, col);
            }
        }
        else
        {
            fraction = dx - (dy >> 1);
            while (Mathf.Abs(y0 - y1) > 1)
            {
                if (fraction >= 0)
                {
                    x0 += stepx;
                    fraction -= dy;
                }
                y0 += stepy;
                fraction += dx;
                tex.SetPixel(x0, y0, col);
            }
        }
    }

    public static Sprite ToSprite(this Texture2D tex)
    {
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
    }

    public static Texture2D ScaleTexture(this Texture2D source, int targetWidth, int targetHeight)
    {
        var result = new Texture2D(targetWidth, targetHeight, source.format, true);
        //Debug.Log($"[ScaleTest1] [{DateTime.Now:HH:mm:ss.fff}]");

        //Color[] rpixels = result.GetPixels(0);
        //float incX = (1.0f / (float)targetWidth);
        //float incY = (1.0f / (float)targetHeight);
        //for (int px = 0; px < rpixels.Length; px++)
        //{
        //    rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
        //}
        //result.SetPixels(rpixels, 0);
        //result.Apply();
        //Debug.Log($"[ScaleTest1] [{DateTime.Now:HH:mm:ss.fff}]");
        Debug.Log($"[ScaleTest2] [{DateTime.Now:HH:mm:ss.fff}]");
        var rt = RenderTexture.GetTemporary(targetWidth, targetHeight, 0, RenderTextureFormat.ARGB32);
        result.filterMode = FilterMode.Bilinear;
        try
        {
            Graphics.Blit(source, rt);

            var rtBef = RenderTexture.active;
            try
            {
                RenderTexture.active = rt;
                result.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
                result.Apply();
            }
            finally
            {
                RenderTexture.active = rtBef;
            }
        }
        finally
        {
            RenderTexture.ReleaseTemporary(rt);
        }
        Debug.Log($"[ScaleTest2] [{DateTime.Now:HH:mm:ss.fff}]");
        return result;
    }
}
}
