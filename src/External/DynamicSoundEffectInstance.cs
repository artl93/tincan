using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinCan
{
    class DynamicSoundEffectInstance
    {
        private int _sampleRate;
        private AudioChannels audioChannels;

        public DynamicSoundEffectInstance(int _sampleRate, AudioChannels audioChannels)
        {
            // TODO: Complete member initialization
            this._sampleRate = _sampleRate;
            this.audioChannels = audioChannels;
        }

        public event EventHandler<EventArgs> BufferNeeded;

        public int PendingBufferCount { get; set; }

        internal void SubmitBuffer(byte[] _soundbuffers, int _offset, int _countOfBytes)
        {
            throw new NotImplementedException();
        }

        internal void Stop()
        {
            throw new NotImplementedException();
        }

        internal void Play()
        {
            throw new NotImplementedException();
        }
    }
}
