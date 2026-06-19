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
using System.Collections.Generic;
using System.IO;
using UniExtension.Collections;
using UnityEngine;
using uobject = UnityEngine.Object;

namespace UnityEngine
{
    public static class UnityObjectExtension_Children
    {
        public static void SetEnableChildren<T>(this Component comp, bool enable)
            where T : Behaviour
        {
            using (var list = PList.Take<T>())
            {
                comp.GetComponentsInChildren<T>(enable, list);
                for (var i = 0; i < list.Count; i++)
                    list[i].enabled = enable;
            }
        }
        public static void SetEnableChildrenRenderer<T>(this Component comp, bool enable)
            where T : Renderer
        {
            using (var list = PList.Take<T>())
            {
                comp.GetComponentsInChildren<T>(enable, list);
                for (var i = 0; i < list.Count; i++)
                    list[i].enabled = enable;
            }
        }

        public static void DestroyChildren<T>(this Component comp, bool includeInactive)
        {
            using (var list = PList.Take<T>())
            {
                comp.GetComponentsInChildren<T>(includeInactive, list);
                for (var i = 0; i < list.Count; i++)
                    GameObject.Destroy((uobject)(object)list[i]);
            }
        }

    }
}
