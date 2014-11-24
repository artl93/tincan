 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using TinCan;

namespace TinCan
{
    public class AudioEngine
    {
        private int _sampleRate = 44100;
        private DynamicSoundEffectInstance _soundEngine;
        private Mixer _rootAudioGraph = Mixer.Create();
        public const int _sampleSize = 1024;
        private int _offset;
        private int _frame;
        private int _channels;
        private byte[] _soundbuffers;
        private short[] _internalSoundBuffer;
        const int _bufferCount = 4;
        private int _countOfShortsInFrame;
        Sequencer _sequencer;
        const double _defaultTempo = 120.0d;

        private AudioEngine(int sampleRate, AudioChannels audioChannels)
        {
            _sampleRate = sampleRate;
            _channels = (int) audioChannels;
            _countOfShortsInFrame = _sampleSize * _channels;
            _sequencer = new Sequencer(_sampleRate, _defaultTempo);

            _soundEngine = new DynamicSoundEffectInstance(_sampleRate, AudioChannels.Stereo);
            _soundEngine.BufferNeeded += new EventHandler<EventArgs>(_soundEngine_BufferNeeded);
            _soundbuffers = new byte[_bufferCount * _sampleSize * _channels * sizeof(short)];
            _internalSoundBuffer = new short[_countOfShortsInFrame];

        }

        void _soundEngine_BufferNeeded(object sender, EventArgs e)
        {
            if (this._soundEngine.PendingBufferCount < 2)
            {
                Debug.WriteLine("Buffers left: {0}", _soundEngine.PendingBufferCount);
                SubmitAudioToBuffer(); // catchup

            } SubmitAudioToBuffer();
        }

        private void SubmitAudioToBuffer()
        {
            RenderAudio();
            RenderTempToOutput();
            SubmitHalfBuffer();
            SubmitHalfBuffer();
        }

        private void RenderTempToOutput()
        {
            for (int i = 0; i < _countOfShortsInFrame; i++)
            {
                short value = (short)(_internalSoundBuffer[i]);

                var byte1 = (byte)(value);
                var byte2 = (byte)(value >> 8);

                _soundbuffers[_offset + i * 2] = byte1;
                _soundbuffers[_offset + (i * 2) + 1] = byte2;
            }
        }

        private void SubmitHalfBuffer()
        {
            int _countOfBytes = (_sampleSize * _channels * sizeof(short) / 2);
            _soundEngine.SubmitBuffer(_soundbuffers, _offset, _countOfBytes);
            _offset += _countOfBytes;
            if (_offset >= _soundbuffers.Length)
                _offset = 0;
        }

        private void RenderAudio()
        {
            if (_rootAudioGraph == null && _sequencer == null)
                return;
            _sequencer.UpdatePosition(_frame, _sampleSize);
            ClearBuffer(_internalSoundBuffer, _countOfShortsInFrame);
            _rootAudioGraph.WriteToOutput(_internalSoundBuffer, 0, _countOfShortsInFrame, _frame);
            _frame++;
        }

        private void ClearBuffer(short[] internalSoundBuffer, int countOfShortsInFrame)
        {
            for (int i = 0; i < countOfShortsInFrame; i++)
                _internalSoundBuffer[i] = 0;
        }

        public static AudioEngine Create(int sampleRate, AudioChannels audioChannels)
        {
            AudioEngine engine = new AudioEngine(sampleRate, audioChannels);
            return engine;
        }

        public Sequencer Sequencer 
        {
            get
            {
                return _sequencer;
            }
        }

        public void Start()
        {
            _soundEngine.Play();
            _sequencer.Start();
        }

        public void Stop()
        {
            _sequencer.Stop();
            _soundEngine.Stop();
        }

        public void AddInstrumentTrack(string trackName, IInstrument instrument, int loopLength, double attenuation)
        {
            var track = SequencerTrack.Create(instrument, 4, loopLength);
            _sequencer.AddTrack(trackName, track);
            var attenuator = new Attenuator() { Level = attenuation };
            var insertChain = InsertChain.Create(instrument);
            insertChain.AddInsert(attenuator);
            _rootAudioGraph.AddComponent(trackName, insertChain);
            _rootAudioGraph.SetBlockSize(_channels, _sampleRate);

        }

        public bool ReadXML(System.Xml.XmlReader reader)
        {
            if (!reader.ReadToDescendant("AudioEngine"))
                return false;
            if (!_sequencer.ReadXML(reader))
                return false;
            reader.ReadEndElement(); // AudioEngine
            return true;
        }

        public void SaveXML(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("AudioEngine");
            _sequencer.SaveXML(writer);
            writer.WriteEndElement(); // AudioEngine
        }
    }
}
