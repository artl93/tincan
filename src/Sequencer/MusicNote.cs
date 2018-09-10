using System;
using System.Net;
using System.Collections.Generic;


namespace TinCan
{
    public enum NoteValue
    {
        Unison = 0,
        Minor2nd,
        Major2nd,
        Minor3rd,
        Major3rd,
        Perfect4th,
        Augmented4th,
        Perfect5th,
        Minor6th,
        Major6th,
        Minor7th,
        Major7th,
        Octave
    };

    public enum MusicKey
    {
        C, Db, D, Eb, E, F, Gb, G, Ab, A, Bb, B
    };


    public class MusicNote
    {

        private NoteValue _note;
        public MusicNote(NoteValue note)
        {
            _note = note;
        }
        static public double GetNoteRatio(NoteValue note)
        {
            switch (note)
            {
                case NoteValue.Unison:
                    return 1.000000d;
                case NoteValue.Minor2nd:
                    return 1.059463d;
                    //return 466.164f / A440;
                case NoteValue.Major2nd:
                    return 1.122462d;
                    // return 493.883f / A440;
                // Minor third (D♯/E♭)
                case NoteValue.Minor3rd:
                    return 1.189207d;
                    // return 523.251f / A440;
                // Major third (E)
                case NoteValue.Major3rd:
                    return 1.259921d;
                    // return 554.365f / A440;
                // Perfect fourth (F)
                case NoteValue.Perfect4th:
                    return 1.334840d;
                // Augmented fourth (F♯/G♭)
                case NoteValue.Augmented4th:
                    return 1.414214d;
                    // return 622.254f / A440;
                // Perfect fifth (G)
                case NoteValue.Perfect5th:
                    return 1.498307d;
                    // return 659.255f / A440;
                // Minor sixth (G♯/A♭)
                case NoteValue.Minor6th:
                    return 1.587401d;
                    // return 698.456f / A440;
                // Major sixth (A)
                case NoteValue.Major6th:
                    return 1.681793d;
                    // return 739.989f / A440;
                // Minor seventh (A♯/B♭)
                case NoteValue.Minor7th:
                    return 1.781797d;
                    // return 783.991f / A440;
                // Major seventh (B)
                case NoteValue.Major7th:
                    return 1.887749d;
                    // return 830.609f / A440;
                // Octave (C)
                case NoteValue.Octave:
                    return 2.000000d;
                default:
                    throw new ArgumentException("Note value is not valid", "name");
            }
        }

    }
}
