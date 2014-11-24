using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TinCan
{

    public class Sequencer
    {
        const int BeatsPerMeasure = 4;
        const int TotalMeasures = 2;

        EventLocation _start = new EventLocation();
        EventLocation _end = new EventLocation() { Measure = TotalMeasures };

        Dictionary<string, SequencerTrack> _tracks = new Dictionary<string, SequencerTrack>();

        readonly public bool Looped = true;
        double _samplesPerBeat;
        EventLocation _eventPosition = new EventLocation();
        int _sampleRate;
        bool _running = false;


        public EventLocation CurrentPosition
        {
            get
            {
                return _eventPosition;
            }
        }

        public void SetTempo(double tempo)
        {
            Tempo = tempo;
            _samplesPerBeat = (_sampleRate * 60) / tempo;
        }

        public Sequencer(int sampleRate, double tempo)
        {
            _sampleRate = sampleRate;
            SetTempo(tempo);
        }

        public void AddTrack(string name, SequencerTrack track)
        {
            _tracks.Add(name, track);
        }

        public void UpdatePosition(int frame, int samples)
        {
            var startEventPosition = _eventPosition;

            UpdateLocation(samples);

            if (_eventPosition >= _end)
            {
                PlayEvents(frame, startEventPosition, _end, 0);
                // TODO: Conditional for loop
                // get the offset from the start of the frame to the end. The end now becomes the beginning, and we 
                // start playing again from there. 
                var frameSampleOffset = (int)Math.Round(EventLocation.GetBeatsSpan(_end, startEventPosition, BeatsPerMeasure) * _samplesPerBeat, 0);
                Debug.Assert(frameSampleOffset >= 0);
                _eventPosition.Measure = 0;
                PlayEvents(frame, _start, _eventPosition, frameSampleOffset);
            }
            else
            {
                PlayEvents(frame, startEventPosition, _eventPosition, 0);
            }
        }

        private void PlayEvents(int frame, EventLocation startSpan, EventLocation endSpan, int sampleOffset)
        {
            foreach (var track in _tracks.Values)
                track.PlayEvents(frame, startSpan, endSpan, sampleOffset, _samplesPerBeat);
        }

        private void UpdateLocation(int samples)
        {
            if (_running)
            {
                var spanBeats = samples / _samplesPerBeat;
                _eventPosition.AddBeats(spanBeats, BeatsPerMeasure);
            }
        }

        public double Tempo { get; private set; }

        internal void Stop()
        {
            _running = false;
        }

        public void Start()
        {
            _running = true;
        }


        public void Clear()
        {
            foreach (var track in _tracks.Values)
                track.Clear();
        }

        public IDictionary<string, SequencerTrack> Tracks
        {
            get
            {
                return _tracks;
            }
        }

        public bool ReadXML(System.Xml.XmlReader reader)
        {
            if (!reader.ReadToDescendant("Sequencer"))
                return false;
            // redefine this to load tracks by name
            foreach (var track in _tracks.Values)
            {
                if (!track.ReadXML(reader))
                    return false;
            } 
            reader.ReadEndElement(); // Sequencer
            return true;
        }

        public void SaveXML(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("Sequencer");
            foreach (var track in _tracks.Values)
                track.SaveXML(writer);
            writer.WriteEndElement(); // Sequencer
        }

    }
}