using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinCan
{
    class InsertChain : IAudioComponent
    {
        List<IAudioComponent> _inserts = new List<IAudioComponent>();

        static public InsertChain Create()
        {
            return new InsertChain();
        }

        public void SetBlockSize(int channels, int sampleRate)
        {
            foreach (var component in _inserts)
                component.SetBlockSize(channels, sampleRate);
        }

        public bool WriteToOutput(short[] outData, int sampleOffset, int sampleCount, int frame)
        {
            bool wasProcessed = false;
            foreach (var component in _inserts)
                wasProcessed |= component.WriteToOutput(outData, sampleOffset, sampleCount, frame);
            return wasProcessed;
        }

        public void AddInsert(IAudioComponent processor)
        {
            _inserts.Add(processor);
        }

        internal static InsertChain Create(IAudioComponent source)
        {
            var chain = Create();
            chain.AddInsert(source);
            return chain;
        }
    }
}
