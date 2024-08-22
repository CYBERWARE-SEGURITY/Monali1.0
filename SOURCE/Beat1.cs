using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FNAF2_REMASTER
{
    public class Sound
    {
        public static void SonsDoSistemaIcons()
        {
            SystemSound[] sounds = {
            SystemSounds.Hand,
            SystemSounds.Question,
            SystemSounds.Exclamation,
            SystemSounds.Beep,
            SystemSounds.Asterisk
        };

            Random rand = new Random();

            while (true)
            {
                SystemSound selectedSound = sounds[rand.Next(sounds.Length)];
                selectedSound.Play();

                Thread.Sleep(500);
            }
        }
    }

    public class Beat1 : WaveProvider32
    {
        private int t = 0;
        private bool switchSound = true;

        public Beat1()
        {
            this.SetWaveFormat(8000, 1); // taxa de amostragem mono
        }

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            for (int i = 0; i < sampleCount; i++)
            {
                byte soundByte = switchSound ? GenerateBytebeatStrong(t) : GenerateBytebeatWeak(t);
                buffer[i + offset] = soundByte / 255f;
                t++;
            }

            switchSound = !switchSound; // Alterna para o próximo som na próxima leitura

            return sampleCount;
        }

        private byte GenerateBytebeatStrong(int t)
        {
            return (byte)(((-t & 4095) * (255 & t * (t & t >> 13)) >> 12) + (127 & t * (234 & t >> 8 & t >> 3) >> (3 & t >> 14)));
        }

        private byte GenerateBytebeatWeak(int t)
        {
            // Implementação de um som diferente para alternar
            return (byte)(((-t & 4095) * (255 & t * (t & t >> 13)) >> 12) + (127 & t * (234 & t >> 8 & t >> 3) >> (3 & t >> 14)));
        }

    }

    public class Beat2 : WaveProvider32
    {
        private int t = 0;
        private bool switchSound = true;

        public Beat2()
        {
            this.SetWaveFormat(8000, 1); // taxa de amostragem mono
        }

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            for (int i = 0; i < sampleCount; i++)
            {
                byte soundByte = switchSound ? GenerateBytebeatStrong(t) : GenerateBytebeatWeak(t);
                buffer[i + offset] = soundByte / 255f;
                t++;
            }

            switchSound = !switchSound; // Alterna para o próximo som na próxima leitura

            return sampleCount;
        }

        private byte GenerateBytebeatStrong(int t)
        {
            return (byte)(5 * t & t >> 7 | 3 * t & 4 * t >> 10);
        }

        private byte GenerateBytebeatWeak(int t)
        {
            // Implementação de um som diferente para alternar
            return (byte)(5 * t & t >> 7 | 3 * t & 4 * t >> 10);
        }
    }

    public class Beat3 : WaveProvider32
    {
        private int t = 0;
        private bool switchSound = true;

        public Beat3()
        {
            this.SetWaveFormat(90000, 1); // taxa de amostragem mono
        }

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            for (int i = 0; i < sampleCount; i++)
            {
                byte soundByte = switchSound ? GenerateBytebeatStrong(t) : GenerateBytebeatWeak(t);
                buffer[i + offset] = soundByte / 255f;
                t++;
            }

            switchSound = !switchSound; // Alterna para o próximo som na próxima leitura

            return sampleCount;
        }

        private byte GenerateBytebeatStrong(int t)
        {
            return (byte)(((((t >> 10 & 44) % 32 >> 1) + ((t >> 9 & 44) % 32 >> 1)) * (32768 > t % 65536 ? 1 : 4 / 5) * t | t >> 3) * (t | t >> 8 | t >> 6));
        }

        private byte GenerateBytebeatWeak(int t)
        {
            // Implementação de um som diferente para alternar
            return (byte)(((((t >> 10 & 44) % 32 >> 1) + ((t >> 9 & 44) % 32 >> 1)) * (32768 > t % 65536 ? 1 : 4 / 5) * t | t >> 3) * (t | t >> 8 | t >> 6));
        }
    }

    public class Beat4 : WaveProvider32
    {
        private int t = 0;
        private bool switchSound = true;

        public Beat4()
        {
            this.SetWaveFormat(22050, 1); // taxa de amostragem mono
        }

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            for (int i = 0; i < sampleCount; i++)
            {
                byte soundByte = switchSound ? GenerateBytebeatStrong(t) : GenerateBytebeatWeak(t);
                buffer[i + offset] = soundByte / 255f;
                t++;
            }

            switchSound = !switchSound; // Alterna para o próximo som na próxima leitura

            return sampleCount;
        }

        private byte GenerateBytebeatStrong(int t)
        {
            return (byte)((t ^ t >> 12) * t >> 8);
        }
        // t>>4+t%34|t>>5+t%(t/31108&1?46:43)|t/4|t/8%35
        private byte GenerateBytebeatWeak(int t)
        {
            // Implementação de um som diferente para alternar
            return (byte)((t ^ t >> 12) * t >> 8);
        }
    }
}
