using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinCan
{
    public interface IInstrument : IAudioComponent
    {
        void Play(AudioEventInfo eventInfo);

    }

    public interface IPreloadableInstrument
    {
        void PreloadPatch(int[] notes);
    }
}
