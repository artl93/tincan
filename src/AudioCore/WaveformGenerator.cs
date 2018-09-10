using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinCan
{
    public enum Waveform
    {
        Nothing,
        Sine, 
        Triangle, 
        Square, 
        SawUp, 
        SawDown
    }

    public class WaveformGenerator : IAudioComponent
    {
        private int _channels;
        private int _sampleRate;
        private double _position;
        private Waveform _waveform;


        private WaveformGenerator(Waveform waveform)
        {
            _waveform = waveform;
            Frequency = 440.0d;
            Play = false;
        }

        public double Frequency
        {
            get;
            set;
        }

        public bool Play
        {
            get;
            set;
        }



        public void WriteToOutput(short[] outData, int offset, int length, int frame)
        {
            if (!Play)
            {
                _position = 0;
                return;
            }
            double samplesPerCycle = _sampleRate / Frequency;

            for (int sample = offset; sample < offset + length; sample += (_channels))
            {
                if (_position > samplesPerCycle) 
                    _position -= samplesPerCycle;
                short val = GetPCMValue(samplesPerCycle);
                _position++;

                for (int channel = 0; channel < _channels; ++channel)
                {
                    outData[sample + channel] = val;
                }
            }
            return;
        }

        private short GetPCMValue(double samplesPerCycle)
        {
            switch (_waveform)
            {
                case Waveform.Nothing:
                    return 0;
                case Waveform.Sine:
                    return (short)(Math.Sin(2 * Math.PI * _position / samplesPerCycle) * short.MaxValue);
                case Waveform.Square:
                    return _position < samplesPerCycle ? short.MaxValue : short.MinValue;
                case Waveform.Triangle:
                    double tempPosition = _position + (samplesPerCycle / 4);
                    if (tempPosition < (samplesPerCycle / 2))
                        return (short)((ushort)(tempPosition / (samplesPerCycle / 2)) * ushort.MaxValue);
                    else
                        return (short)(ushort.MaxValue - ((ushort)(tempPosition / (samplesPerCycle / 2)) * ushort.MaxValue));
                case Waveform.SawUp:
                    return (short)((ushort)(_position / (samplesPerCycle)) * ushort.MaxValue);
                case Waveform.SawDown:
                    return (short)(ushort.MaxValue - ((ushort)(_position / (samplesPerCycle)) * ushort.MaxValue));
            }
            throw new Exception("Invalid waveform selection");
        }

        public void SetBlockSize(int channels, int sampleRate)
        {
            _sampleRate = sampleRate;
            _channels = channels;
        }



        public static WaveformGenerator Create(Waveform waveform)
        {
            return new WaveformGenerator(waveform);
        }
    }
}
