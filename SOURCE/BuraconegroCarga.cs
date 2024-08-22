using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FNAF2_REMASTER
{
    public class BuraconegroCarga
    {
        static int w = Screen.PrimaryScreen.Bounds.Width;
        static int h = Screen.PrimaryScreen.Bounds.Height;

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFOHEADER pbmi, uint pila, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport("gdi32.dll")]
        static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        static extern bool StretchBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSrc, int xSrc, int ySrc, int wSrc, int hSrc, uint rop);

        [DllImport("gdi32.dll")]
        static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSrc, int xSrc, int ySrc, uint rop);

        [DllImport("user32.dll")]
        static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport("user32.dll")]
        static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        const int SRCCOPY = 0x00CC0020;
        const int BI_RGB = 0;

        // Definição da estrutura BITMAPINFOHEADER
        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
        }

        // Definição da estrutura RGBQUAD
        [StructLayout(LayoutKind.Sequential)]
        public struct RGBQUAD
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        }

        public static void VishZe()
        {
            IntPtr dc = GetDC(IntPtr.Zero);
            IntPtr dcCopy = CreateCompatibleDC(dc);

            BITMAPINFOHEADER bmiHeader = new BITMAPINFOHEADER();
            bmiHeader.biSize = (uint)Marshal.SizeOf(typeof(BITMAPINFOHEADER));
            bmiHeader.biWidth = w;
            bmiHeader.biHeight = h;
            bmiHeader.biPlanes = 1;
            bmiHeader.biBitCount = 32;
            bmiHeader.biCompression = BI_RGB;

            IntPtr bits;
            IntPtr bmp = CreateDIBSection(dc, ref bmiHeader, BI_RGB, out bits, IntPtr.Zero, 0);

            RGBQUAD[] rgbquad = new RGBQUAD[w * h];

            SelectObject(dcCopy, bmp);

            int i = 0;
            double angle = 0.0;

            while (true)
            {
                StretchBlt(dcCopy, 0, 0, w, h, dc, 0, 0, w, h, SRCCOPY);

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        int index = y * w + x;

                        int cx = Math.Abs(x - (w / 2));
                        int cy = Math.Abs(y - (h / 2));

                        double rad = angle * Math.PI / 180.0;
                        int zx = (int)(Math.Cos(rad) * cx - Math.Sin(rad) * cy);
                        int zy = (int)(Math.Sin(rad) * cx + Math.Cos(rad) * cy);

                        double distance = Math.Sqrt(cx * cx + cy * cy);
                        int depth = (int)(10 * Math.Sin(distance / 10.0 * angle));

                        int fx = (zx + i) ^ (zy + i);

                        rgbquad[index].rgbRed = (byte)((rgbquad[index].rgbRed + fx + depth) % 256);
                        rgbquad[index].rgbGreen = (byte)((rgbquad[index].rgbGreen + fx + depth) % 256);
                        rgbquad[index].rgbBlue = (byte)((rgbquad[index].rgbBlue + fx + depth) % 256);
                    }
                }

                byte[] rgbBytes = new byte[Marshal.SizeOf(typeof(RGBQUAD)) * w * h];
                for (int j = 0; j < w * h; j++)
                {
                    rgbBytes[j * 4] = rgbquad[j].rgbBlue;
                    rgbBytes[j * 4 + 1] = rgbquad[j].rgbGreen;
                    rgbBytes[j * 4 + 2] = rgbquad[j].rgbRed;
                    rgbBytes[j * 4 + 3] = rgbquad[j].rgbReserved;
                }
                Marshal.Copy(rgbBytes, 0, bits, rgbBytes.Length);

                BitBlt(dc, 0, 0, w, h, dcCopy, 0, 0, SRCCOPY);

                angle += 0.01;
                i++;
            }
        }
    }
}
