using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinCan
{
    public class Mixer : IAudioComponent
    {
        Dictionary<string, IAudioComponent> _channels = new Dictionary<string, IAudioComponent>();
        private short _channelCount;
        int _tempBufferLen = 0;
        short[] _tempBuffer;
        float[] _renderBuffer;
        private int _audioChannels;
        private int _sampleRate;


        protected Mixer() {
        }

        static public Mixer Create()
        {
            return new Mixer();
        }

        void EnsureTempBuffers(int length)
        {
            if (length > _tempBufferLen)
            {
                _tempBufferLen = length;
                _tempBuffer = new short[_tempBufferLen * _channelCount];
                _renderBuffer = new float[_tempBufferLen];
            }
            for (int i = 0; i < _tempBufferLen; i++)
                _renderBuffer[i] = 0;
            for (int i = 0; i < _tempBuffer.Length; i++)
                _tempBuffer[i] = 0;
        }


        public void WriteToOutput(short[] outData, int offset, int length, int frame)
        {
            EnsureTempBuffers(length);

            int currentChannel = 0;
            foreach (var channel in _channels)
            {
                channel.Value.WriteToOutput(_tempBuffer, _tempBufferLen * currentChannel, _tempBufferLen, frame);
                AddToRenderBuffer(_tempBuffer, _tempBufferLen * currentChannel, _tempBufferLen);
                currentChannel++;
            }
            
            RenderOutput(outData, offset, length, _renderBuffer);
        }

        private void AddToRenderBuffer(short[] tempBuffer, int tempBufferOffset, int length)
        {
            for (int i = 0; i < length; i++)
            {
                short value = tempBuffer[i + tempBufferOffset];

                _renderBuffer[i] += value;
            }
        }

        protected virtual void RenderOutput(short[] outData, int offset, int length, float[]tempBuffer)
        {
            for (int i = 0; i < length; i++)
            {
                outData[offset + i] = (short)tempBuffer[i];
            }
        }

        public void AddComponent(string name, IAudioComponent component)
        {
            _channels.Add(name, component);
            component.SetBlockSize(_audioChannels, _sampleRate);
            _channelCount = (short)_channels.Count;
        }


        public IDictionary<string, IAudioComponent> GetComponents()
        {
            return _channels;
        }

        public void SetBlockSize(int channels, int sampleRate)
        {
            _sampleRate = sampleRate;
            _audioChannels = channels;
            foreach (var channel in _channels)
            {
                channel.Value.SetBlockSize(channels, sampleRate);
            }
        }

    }
}
