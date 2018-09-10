using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinCan
{
    /// <summary>
    /// Loads from a WAVE file based on the format documented at
    /// https://ccrma.stanford.edu/courses/422/projects/WaveFormat/
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>

    public class SampleFile 
    {
        public int Channels
        {
            get;
            private set;
        }

        public uint SampleRate
        {
            get;
            private set;
        }

        public int BitsPerSample
        {
            get;
            private set;
        }

        public void Ensure()
        {
        }

        public byte[] _data;



        static internal SampleFile OpenWAVFile(System.IO.Stream stream)
        {
            using (var reader = new System.IO.BinaryReader(stream))
            {
                ReadRIFFHeader(reader);

                return ReadSubchunks(reader);

            }
        }


        private static void ReadRIFFHeader(System.IO.BinaryReader reader)
        {
            // reads the format "RIFF"
            var riff = reader.ReadInt32();
            if (riff != 0x46464952)
                throw new FormatException("Bad RIFF header");

            // this should == overallFileSize - 2 bytes
            var fileSize = reader.ReadUInt32();

            // reads the format, "WAVE"
            if (reader.ReadInt32() != 0x45564157)
                throw new FormatException("Bad WAVE header");

        }

        private static SampleFile ReadSubchunks(System.IO.BinaryReader reader)
        {
            // reads the format "fmt "
            if (reader.ReadInt32() != 0x20746d66)
                throw new FormatException("Bad fmt header");

            // read chunk size - we expect 16 
            if (reader.ReadInt32() != 16)
                throw new FormatException("Chunk size");

            // read chunk size - we expect 16 
            if (reader.ReadInt16() != 1)
                throw new FormatException("Unknown compression scheme");

            var channels = reader.ReadInt16();
            var sampleRate = reader.ReadUInt32();
            var byteRate = reader.ReadUInt32();
            var blockAlign = reader.ReadUInt16();
            var bitsPerSample = reader.ReadUInt16();

            if (byteRate != (sampleRate * channels * (bitsPerSample / 8)))
                throw new FormatException("ByteRate checksum failed");

            if (blockAlign != (channels * (bitsPerSample / 8)))
                throw new FormatException("BlockAlign checksum failed");

            // start subchunk2 = "data"
            if (reader.ReadUInt32() != 0x61746164)
                throw new FormatException("Cannot find start of data block");

            var dataLength = reader.ReadInt32();

            // if the data is small enough, load into memory
            return new SampleFile()
            {
                _data = reader.ReadBytes(dataLength),
                Channels = channels,
                BitsPerSample = bitsPerSample,
                SampleRate = sampleRate
            };
        }
    }

}
