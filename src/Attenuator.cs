using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinCan
{
     public sealed class Attenuator : IAudioComponent
    {
        private Attenuator() { }
        static public Attenuator Create(double level)
        {
            var a = new Attenuator();
            a.Level = level;
            return a;
        }
        public double Level { get; set; }

        public void SetBlockSize(int channels, int sampleRate)
        {
        }

        public void WriteToOutput(short[] outData, int sampleOffset, int sampleCount, int frame)
        {
            for (int i = sampleOffset; i < sampleCount + sampleOffset; i++)
                outData[i] = (short)(outData[i] * Level);
        }
    }
}
