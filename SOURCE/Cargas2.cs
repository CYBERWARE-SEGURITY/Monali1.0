using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FNAF2_REMASTER
{
    public class Cargas2
    {
        static int w = Screen.PrimaryScreen.Bounds.Width;
        static int h = Screen.PrimaryScreen.Bounds.Height;

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO pbmi, uint pila, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport("gdi32.dll")]
        static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        static extern bool StretchBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSrc, int xSrc, int ySrc, int wSrc, int hSrc, uint rop);

        [DllImport("gdi32.dll")]
        static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSrc, int xSrc, int ySrc, uint rop);

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern bool DeleteDC(IntPtr hdc);

        const int SRCCOPY = 0x00CC0020;
        const int BI_RGB = 0;

        public static void ColorRipple()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            IntPtr dcCopy = CreateCompatibleDC(hdc);

            try
            {
                int ws = w / 16;
                int hs = h / 16;

                BITMAPINFO bmi = new BITMAPINFO();
                bmi.biSize = Marshal.SizeOf(bmi);
                bmi.biWidth = ws;
                bmi.biHeight = hs;
                bmi.biPlanes = 1;
                bmi.biBitCount = 32;
                bmi.biCompression = BI_RGB;

                IntPtr ppvBits;
                IntPtr hbmp = CreateDIBSection(hdc, ref bmi, BI_RGB, out ppvBits, IntPtr.Zero, 0);
                IntPtr oldBmp = SelectObject(dcCopy, hbmp);

                Random rand = new Random();
                int i = 0;

                while (true)
                {
                    IntPtr hBrush = CreateSolidBrush((uint)Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256)).ToArgb());
                    SelectObject(dcCopy, hBrush);

                    StretchBlt(dcCopy, 0, 0, ws, hs, hdc, 0, 0, w, h, SRCCOPY);

                    unsafe
                    {
                        RGBQUAD* rgbquad = (RGBQUAD*)ppvBits.ToPointer();

                        for (int x = 0; x < ws; x++)
                        {
                            for (int y = 0; y < hs; y++)
                            {
                                int index = y * ws + x;

                                int wave = (int)(10 * Math.Sin(x / 6.0) * Math.Cos(y / 6.0));

                                rgbquad[index].rgbRed = (byte)((rgbquad[index].rgbRed + wave) % 256);
                                rgbquad[index].rgbGreen = (byte)((rgbquad[index].rgbGreen + wave) % 256);
                                rgbquad[index].rgbBlue = (byte)((rgbquad[index].rgbBlue + wave) % 256);
                            }
                        }
                    }

                    StretchBlt(hdc, 0, 0, w, h, dcCopy, 0, 0, ws, hs, SRCCOPY);

                    Thread.Sleep(10);
                    RedrawScreen();
                }
            }
            finally
            {
                DeleteDC(dcCopy);
                ReleaseDC(IntPtr.Zero, hdc);
            }
        }

        static void RedrawScreen()
        {
            IntPtr desktop = GetDC(IntPtr.Zero);
            BitBlt(desktop, 0, 0, w, h, desktop, 0, 0, SRCCOPY);
            ReleaseDC(IntPtr.Zero, desktop);
        }

        [StructLayout(LayoutKind.Sequential)]
        struct BITMAPINFO
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public short biPlanes;
            public short biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUsed;
            public int biClrImportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct RGBQUAD
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        }

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateSolidBrush(uint color);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFOHEADER pbmi, uint pila, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);


        [DllImport("user32.dll")]
        static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

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
    }
}
