using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TinCan
{
    public class SimpleSamplePlayer : IInstrument
    {

        private int _position = -1;
        private Patch _patch;
        private int _blockSize = 0;
        private int _channels;
        private int _currentNote = 0;

        private SimpleSamplePlayer() { }
        static public SimpleSamplePlayer Create(Patch patch)
        {
            var player = new SimpleSamplePlayer();
            player._patch = patch;
            player.Stop();
            return player;
        }

        Dictionary<int, Queue<AudioEventInfo>> _queue = new Dictionary<int, Queue<AudioEventInfo>>(16);

        void Stop()
        {
            if (_patch.IsSampleLoaded(_currentNote))
                _position = _patch.GetSampleFile(_currentNote)._data.Length;

        }

        public void WriteToOutput(short[] outData, int outputOffset, int outputLength, int frame)
        {
            int outputOffsetInBytes = outputOffset * sizeof(short);
            int outputLengthInBytes = outputLength * sizeof(short);
            int currentWriteOffsetBytes = 0;
            int nextOffsetInBytes = outputLengthInBytes;
            RenderInfo renderInfo = new RenderInfo();
#if DEBUG 
            List<RenderInfo> debugRenders = new List<RenderInfo>();
#endif 

            Debug.Assert(_queue.Count <= 1);

            while (_queue.ContainsKey(frame))
            {
                var playInfo = _queue[frame].Dequeue();
                nextOffsetInBytes = playInfo.sampleOffset * _blockSize * _channels;
                if (!EOF)
                {
                    // now that you know how long the last one was, render it! 
                    renderInfo.outputOffsetBytes = outputOffsetInBytes + currentWriteOffsetBytes;
                    renderInfo.sourceOffsetBytes = _position;
                    renderInfo.writeLenthBytes = Math.Min(outputLengthInBytes - currentWriteOffsetBytes, GetData().Length - _position);
#if DEBUG
                    debugRenders.Add(renderInfo);
#endif 
                    Render(outData, renderInfo);
                }
                _currentNote = playInfo.note;
                Reset();
                currentWriteOffsetBytes = nextOffsetInBytes;
                if (_queue[frame].Count == 0)
                {
                    _queue.Remove(frame);
                }
            }
            if (!EOF)
            {
                renderInfo.outputOffsetBytes = outputOffsetInBytes + currentWriteOffsetBytes;
                renderInfo.sourceOffsetBytes = _position;
                renderInfo.writeLenthBytes = Math.Min(outputLengthInBytes - currentWriteOffsetBytes, GetData().Length - _position);
#if DEBUG
                debugRenders.Add(renderInfo);
#endif
                Render(outData, renderInfo);
            }
        }

        private bool Render(short[] outData, RenderInfo renderInfo)
        {
            Buffer.BlockCopy(GetData(), _position, outData, renderInfo.outputOffsetBytes, renderInfo.writeLenthBytes);
            _position += renderInfo.writeLenthBytes;
            //for (int i = renderInfo.outputOffsetBytes; i < renderInfo.writeLenthBytes; i++)
            //{
            //    outData[i] = _sampleData._data[_position];
            //    _position++;
            //}
            return renderInfo.writeLenthBytes > 0;
        }


        internal bool EOF
        {
            get
            {
                return _position >= GetData().Length || _position < 0;
            }
        }

        private byte[] GetData()
        {
            return this._patch.GetSampleFile(_currentNote)._data;
        }

        internal void Reset()
        {
            _position = 0;
        }

        public void SetBlockSize(int channels, int sampleRate)
        {
            _channels = channels;
        }

        public void Play(AudioEventInfo info)
        {
            // System.Diagnostics.Trace.TraceWarning("Play: {0}, {1}", info.frame, info.sampleOffset);
            if (!_queue.ContainsKey(info.frame))
                _queue.Add(info.frame, new Queue<AudioEventInfo>());
            Debug.Assert(info.sampleOffset >= 0);
            _queue[info.frame].Enqueue(info);                
        }

        struct RenderInfo
        {
#if DEBUG
            int _sourceOffsetBytes;
            public int sourceOffsetBytes
            {
                get
                {
                    return _sourceOffsetBytes;
                }
                set
                {
                    Debug.Assert(value >= 0);
                    _sourceOffsetBytes = value;
                }
            }

            int _outputOffsetBytes;
            public int outputOffsetBytes
            {
                get
                {
                    return _outputOffsetBytes;
                }
                set
                {
                    Debug.Assert(value >= 0);
                    _outputOffsetBytes = value;
                }
            }
            int _writeLenthBytes;
            public int writeLenthBytes
            {
                get
                {
                    return _writeLenthBytes;
                }
                set
                {
                    Debug.Assert(value >= 0);
                    _writeLenthBytes = value;
                }

            }
#else
            public int sourceOffsetBytes;
            public int outputOffsetBytes;
            public int writeLenthBytes;
#endif
        };

    }
    
}
