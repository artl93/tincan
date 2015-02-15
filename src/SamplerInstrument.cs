using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinCan
{
    public class SamplerInstrument : Mixer, IInstrument, IAudioComponent, IPreloadableInstrument
    {
        int _maxVoices;
        IInstrument[] _voices;
        int _nextInstrument;
        private Patch _patch;
        private double _outputRatio;

        private SamplerInstrument(int voices) 
        {
            _maxVoices = voices;
            _outputRatio = Math.Sqrt(_maxVoices);
            _voices = new IInstrument[_maxVoices];
        }

        public void Play(AudioEventInfo info)
        {
            // pick the right picth
            IInstrument player = GetNextAvailablePlayer();
            player.Play(info);
        }

        private IInstrument GetNextAvailablePlayer()
        {
            var voice = _voices[_nextInstrument];
            _nextInstrument++;
            if (_nextInstrument >= _maxVoices)
                _nextInstrument = 0;
            return voice; 
        }


        public static SamplerInstrument Create(Patch patch, int maxVoices)
        {
            var sampler = new SamplerInstrument(maxVoices);
            sampler._patch = patch;
            for (int i = 0; i < maxVoices; i++)
            {

                var voice = SimpleSamplePlayer.Create(patch);
                sampler._voices[i] = voice;
                sampler.AddComponent(i.ToString(), voice);
            }
            return sampler;
        }

        public void PreloadPatch(int[] notes)
        {
            foreach (int note in notes)
                _patch.GetSampleFile(note);
        }

        protected override void RenderOutput(short[] outData, int offset, int length, float[] tempBuffer)
        {
            for (int i = 0; i < length; i++)
            {
                var attenuated = tempBuffer[i] / _outputRatio;
                short value = (short)(attenuated);
                outData[offset + i] = value;
            }
        }
    }
}
