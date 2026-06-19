#if ENABLE_BITMAP
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace UniExtension
{
    public static class BitmapExtension
    {
        public static byte[] GetPixelBytes(this Bitmap bm, PixelFormat format = PixelFormat.Format32bppRgb, bool reverseVertical = false)
        {
            Size size = bm.Size;

            BitmapData bd = bm.LockBits(new Rectangle(0, 0, size.Width, size.Height), ImageLockMode.ReadOnly, format);
            byte[] buffer = new byte[size.Height * bd.Stride];
            try
            {
                if (reverseVertical)
                {
                    long pIndex = ((long)bd.Scan0) + ((bd.Height - 1) * bd.Stride);
                    int length = Math.Abs(bd.Stride);
                    for (int y = 0; y < size.Height; y++)
                    {
                        Marshal.Copy((IntPtr)pIndex, buffer, y * length, length);
                        pIndex -= bd.Stride;
                    }
                }
                else
                {
                    long pIndex = (long)bd.Scan0;
                    int length = Math.Abs(bd.Stride);
                    for (int y = 0; y < size.Height; y++)
                    {
                        Marshal.Copy((IntPtr)pIndex, buffer, y * length, length);
                        pIndex += bd.Stride;
                    }
                }
            }
            finally
            {
                bm.UnlockBits(bd);
            }
            return buffer;
        }
    }
}
#endif