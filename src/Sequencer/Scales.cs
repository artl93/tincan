using System;
using System.Net;
using System.Collections.Generic;


namespace TinCan
{
    public static class Scales
    {
        public static IEnumerable<NoteValue> MajorScale = new List<NoteValue>() {
                    NoteValue.Unison,
                    NoteValue.Major2nd,
                    NoteValue.Major3rd,
                    NoteValue.Perfect4th,
                    NoteValue.Perfect5th,
                    NoteValue.Major6th,
                    NoteValue.Major7th
            };

        public static IEnumerable<NoteValue> MelodicMinorScale = new List<NoteValue>() {
                    NoteValue.Unison,
                    NoteValue.Major2nd,
                    NoteValue.Minor3rd,
                    NoteValue.Perfect4th,
                    NoteValue.Perfect5th,
                    NoteValue.Minor6th,
                    NoteValue.Minor7th
            };

        public static IEnumerable<NoteValue> HappinessScale = new List<NoteValue>() {
                    NoteValue.Unison,
                    NoteValue.Perfect4th,
                    NoteValue.Perfect5th,
                    NoteValue.Minor7th
            };

        public static IEnumerable<NoteValue> IntrigueScale = new List<NoteValue>() {
                    NoteValue.Unison,
                    NoteValue.Minor3rd,
                    NoteValue.Perfect4th,
                    NoteValue.Minor7th
            };

        public static IEnumerable<NoteValue> IntrigueScale2 = new List<NoteValue>() {
                    NoteValue.Unison,
                    NoteValue.Minor3rd,
                    NoteValue.Perfect5th,
                    NoteValue.Minor6th
            };

        public static IEnumerable<NoteValue> Chromatic = new List<NoteValue>(){
                    NoteValue.Unison, 
                    NoteValue.Minor2nd, 
                    NoteValue.Major2nd, 
                    NoteValue.Minor3rd, 
                    NoteValue.Major3rd, 
                    NoteValue.Perfect4th, 
                    NoteValue.Augmented4th, 
                    NoteValue.Perfect5th, 
                    NoteValue.Minor6th, 
                    NoteValue.Major6th, 
                    NoteValue.Minor7th, 
                    NoteValue.Major7th
        };
    }

 }
