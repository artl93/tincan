using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinCan
{
    public struct EventLocation : IComparable<EventLocation>, IEqualityComparer<EventLocation>, IEquatable<EventLocation>
    {
        public EventLocation(double div)
        {
            Measure = 0;
            Beat = 0;
            Div = div;
        }


        public int Measure;
        public int Beat;
        public double Div;

        public void AddBeats(double spanBeats, int beatsPerMeasure)
        {
            Div += spanBeats;
            if (Div >= 1)
            {
                var wholeBeats = (int)Div;
                Beat += wholeBeats;
                Div -= wholeBeats;
            }
            if (Beat >= beatsPerMeasure)
            {
                var newMeasures = Beat / 4;
                Beat = Beat % beatsPerMeasure;
                Measure += newMeasures;
            }

        }

        public static double GetBeatsSpan(EventLocation a, EventLocation b, int beatsPerMeasure)
        {
            var measBeats = (a.Measure - b.Measure) * beatsPerMeasure;
            var beats = a.Beat - b.Beat;
            var div = (a.Div - b.Div);

            return beats + div + measBeats;
        }

        public int CompareTo(EventLocation other)
        {
            if (this.Measure != other.Measure)
                return this.Measure > other.Measure ? 1 : -1;
            if (this.Beat != other.Beat)
                return this.Beat > other.Beat ? 1 : -1;
            if (this.Div != other.Div)
                return this.Div > other.Div ? 1 : -1;
            return 0;
        }

        public static bool operator ==(EventLocation a, EventLocation b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(EventLocation a, EventLocation b)
        {
            return !a.Equals(b);
        }

        public static bool operator >(EventLocation a, EventLocation b)
        {
            return (a.CompareTo(b) == 1);
        }
        public static bool operator <(EventLocation a, EventLocation b)
        {
            return (a.CompareTo(b) == -1);
        }
        public static bool operator >=(EventLocation a, EventLocation b)
        {
            var result = a.CompareTo(b);
            return (result == 1 || result == 0);
        }
        public static bool operator <=(EventLocation a, EventLocation b)
        {
            var result = a.CompareTo(b);
            return (result == -1 || result == 0);
        }

        public bool Equals(EventLocation x, EventLocation y)
        {
            return x.GetHashCode() == y.GetHashCode();
        }

        public int GetHashCode(EventLocation obj)
        {
            var hash = (obj.Measure * 8) + obj.Beat + obj.Div;
            return (int) (hash * 20500);
        }

        public bool Equals(EventLocation other)
        {
            return this.GetHashCode() == other.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals((EventLocation) obj);
        }

        public override int GetHashCode()
        {
            return GetHashCode(this);
        }

        private double ToDouble(int beatsPerMeasure)
        {
            return (Measure * beatsPerMeasure) + Beat + Div;

        }


        internal void ApplyModulo(EventLocation moduland, int beatsPerMeasure)
        {
            var modulo = ToDouble(beatsPerMeasure) % moduland.ToDouble(beatsPerMeasure);
            Div = Measure = Beat = 0;
            AddBeats(modulo, beatsPerMeasure);
        }

        public override string ToString()
        {
            return String.Format("{0}:{1}:{2}", this.Measure, this.Beat, this.Div);
        }

        static public EventLocation Parse(string value)
        {
            var values = value.Split(':');
            var newEvent = new EventLocation();
            newEvent.Measure = Int32.Parse(values[0]);
            newEvent.Beat = Int32.Parse(values[1]);
            newEvent.Div = Double.Parse(values[2]);
            return newEvent;

        }
    }
}
