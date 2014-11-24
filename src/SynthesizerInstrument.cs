using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinCan
{
    public class SynthesizerInstrument : IInstrument
    {

        WaveformGenerator _generator;
        Attenuator _attenuator = new Attenuator();

        private SynthesizerInstrument() {
            _attenuator.Level = 0.01d; 

        }
        
        public void Play(AudioEventInfo eventInfo)
        {
            // A + 3 = C
            var currentNote = (int)(eventInfo.note + 3);
            NoteValue value = (NoteValue)(currentNote % (int)NoteValue.Octave);
            var octave = (currentNote / (int)NoteValue.Octave) + 1;
            var ratio = MusicNote.GetNoteRatio(value);
            _generator.Frequency = 440.0 * ratio * octave;
            _generator.Play = true;
        }

        public void SetBlockSize(int channels, int sampleSize)
        {
            _generator.SetBlockSize(channels, sampleSize);
        }

        public bool WriteToOutput(short[] outData, int offset, int length, int frame)
        {
            var ret =_generator.WriteToOutput(outData, offset, length, frame);
            if (ret)
                _attenuator.WriteToOutput(outData, offset, length, frame);
            return ret;
        }


        internal static IInstrument Create(Waveform waveform)
        {
            var synth = new SynthesizerInstrument();
            synth._generator = WaveformGenerator.Create(waveform);
            return synth;
        }
    }
}
