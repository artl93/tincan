using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TinCan
{
    public interface IAudioComponent 
    {
        void SetBlockSize(int channels, int sampleRate);
        void WriteToOutput(short[] outData, int sampleOffset, int sampleCount, int frame);
    }
}
