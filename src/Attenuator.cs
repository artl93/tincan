using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinCan
{
    public class Attenuator : IAudioComponent
    {
        public double Level { get; set; }

        public void SetBlockSize(int channels, int sampleRate)
        {
            // throw new NotImplementedException();
        }

        public bool WriteToOutput(short[] outData, int sampleOffset, int sampleCount, int frame)
        {
            for (int i = sampleOffset; i < sampleCount + sampleOffset; i++)
                outData[i] = (short)(outData[i] * Level);
            return true;
        }
    }
}
