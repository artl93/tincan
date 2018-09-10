using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace TinCan
{
    public class SequencerTrack 
    {
        List<TrackEvent> _events = new List<TrackEvent>();

        private IInstrument _device;
        private int _beatsPerMeasure;
        private EventLocation _end;
        private EventLocation _start = new EventLocation();

        private SequencerTrack() { }
        public static SequencerTrack Create(IInstrument device, int beatsPerMeasure, int measures)
        {
            var track = new SequencerTrack();
            track._device = device;
            track._beatsPerMeasure = beatsPerMeasure;
            track._end.AddBeats(measures * beatsPerMeasure, beatsPerMeasure);
            return track;
        }

        public void Reset()
        {
        }

        public IEnumerable<TrackEvent> GetEvents()
        {
            return _events;
        }

        public void RemoveEvent(EventLocation location, int noteValue)
        {
            var eventsToRemove = from e in _events
                                 where e.Location == location && e.Note == noteValue
                                 select e;
            var events = eventsToRemove.ToArray();
            foreach (var evt in events)
                _events.Remove(evt);
        }

        public void AddEvent(EventLocation location, int note)
        {
            int index = -1;

            foreach (var evt in _events)
            {
                if (evt.Location > location)
                    break;
                index++;
            }
            _events.Insert(index + 1, new TrackEvent(location, note));
        }



        internal void PlayEvents(int frame, EventLocation startSpan, EventLocation endSpan, int sampleOffset, double samplesPerBeat) 
        {
            // not straddling the _end - just modulo and go!
            if (startSpan >= _end && endSpan >= _end)
            {
                EventLocation startInLoop = startSpan;
                EventLocation endInLoop = endSpan;

                startInLoop.ApplyModulo(_end, _beatsPerMeasure);
                endInLoop.ApplyModulo(_end, _beatsPerMeasure);

                PlayEventsInSubFrame(frame, startInLoop, endInLoop, sampleOffset, samplesPerBeat);
            }
            // we're straddling the _end - split into two calls 
            else if (endSpan >= _end)
            {
                PlayEventsInSubFrame(frame, startSpan, _end, sampleOffset, samplesPerBeat);
                EventLocation endInLoop = endSpan;
                endInLoop.ApplyModulo(_end, _beatsPerMeasure);
                int subFrameOffset = (int)Math.Round(EventLocation.GetBeatsSpan(_end, startSpan, _beatsPerMeasure) * samplesPerBeat, 0);
                PlayEventsInSubFrame(frame, _start, endInLoop, subFrameOffset, samplesPerBeat);
            }
            else
            {
                PlayEventsInSubFrame(frame, startSpan, endSpan, sampleOffset, samplesPerBeat);
            }
        }

        EventLocation _lastLocation = new EventLocation();
        private ThreadPool ThreadPool;

        private void PlayEventsInSubFrame(int frame, EventLocation startSpan, EventLocation endSpan, int sampleOffset, double samplesPerBeat)
        {
            if (startSpan < _lastLocation)
                Reset();
            foreach (var e in _events)
            {
                if (!(e.Location < endSpan && e.Location >= startSpan))
                    continue;
                var noteStartLocation = e.Location;
                AudioEventInfo info;
                info.frame = frame;
                info.note = e.Note;
                info.sampleOffset = (int)Math.Round(EventLocation.GetBeatsSpan(noteStartLocation, startSpan, _beatsPerMeasure) * samplesPerBeat, 0) + sampleOffset;
                _device.Play(info);
                FireNoteStarted(e.Location, e.Note);
            }
            _lastLocation = startSpan;
        }

        private void FireNoteStarted(EventLocation location, int note)
        {
            var handlers = NoteStarted;
            var eventArg = new NoteStartedEventArg(location, note);
            if (handlers != null)
            {
                handlers(this, eventArg);
            }
        }

        internal void Clear()
        {
            _events.Clear();
        }


        public event EventHandler<NoteStartedEventArg> NoteStarted;
        public event EventHandler PreloadStarted;
        public event EventHandler PreloadFinished;

        private void FirePreloadStarted()
        {
            var handlers = PreloadStarted;
            var eventArg = new EventArgs();
            if (handlers != null)
            {
                handlers(this, eventArg);
            }
        }

        private void FirePreloadFinished()
        {
            var handlers = PreloadFinished;
            var eventArg = new EventArgs();
            if (handlers != null)
            {
                handlers(this, eventArg);
            }
        }
        
        public void PreloadPatch(int[] notes)
        {
            FirePreloadStarted();
            ThreadPool.QueueUserWorkItem(() => AsyncContentLoad(notes)); 
        }

        private void AsyncContentLoad(int[] notes)
        {
            if (_device is IPreloadableInstrument)
            {
                ((IPreloadableInstrument)_device).PreloadPatch(notes);
            }
            FirePreloadFinished();
        }

        internal bool ReadXML(System.Xml.XmlReader reader)
        {
            
            reader.ReadToDescendant("Events");
            if (reader.ReadToDescendant("Event"))
            {
                do
                {
                    if (reader.NodeType == System.Xml.XmlNodeType.Element &&
                        reader.Name == "Event")
                    {
                        var location = EventLocation.Parse(reader.GetAttribute("timestamp"));
                        var note = Int32.Parse(reader.GetAttribute("note"));
                        _events.Add(new TrackEvent(location, note));
                    }
                    else
                        break;
                }
                while (reader.ReadToNextSibling("Event"));
                reader.ReadEndElement();
                return true;
            }
            return false;
        }

        internal void SaveXML(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("Events");
            foreach (var noteEvent in _events)
            {
                writer.WriteStartElement("Event");
                writer.WriteAttributeString("timestamp", noteEvent.Location.ToString());
                writer.WriteAttributeString("note", noteEvent.Note.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}
