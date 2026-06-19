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
public static class RectExtensions
{
    public static Rect SetWidth(this Rect r, float width)
    {
        r.width = width;
        return r;
    }
    public static Rect SetHeight(this Rect r, float height)
    {
        r.height = height;
        return r;
    }
    public static Rect SetX(this Rect r, float x)
    {
        r.x = x;
        return r;
    }
    public static Rect SetY(this Rect r, float y)
    {
        r.y = y;
        return r;
    }
    public static Rect AddX(this Rect r, float x)
    {
        r.x += x;
        return r;
    }
    public static Rect AddY(this Rect r, float y)
    {
        r.y += y;
        return r;
    }
    public static Rect AddXMinusWidth(this Rect r, float x)
    {
        r.x += x;
        r.width -= x;
        return r;
    }
    public static Rect AddYMinusHeight(this Rect r, float y)
    {
        r.y += y;
        r.height -= y;
        return r;
    }
    public static Rect FlexX(this Rect r, int i, int len)
    {
        //float ratio = (float)i / len;
        var single = r.width / len;

        r.x += i * single;
        r.width = single;

        return r;
    }
}
}
