using System;
using System.Net;
using System.Collections.Generic;


namespace TinCan
{
    public class Scale
    {
        IEnumerable<NoteValue> _notes;
        public Scale(IEnumerable<NoteValue> notes)
        {
            _notes = notes;
        }
        public IEnumerable<int> GetNoteIndices(int count, int offset)
        {
            int i = offset;
            int countReturned = 0;

            do
            {
                foreach (var noteIndex in _notes)
                {
                    yield return i + (int)noteIndex;
                    countReturned++;
                    if (countReturned >= count)
                        yield break;
                }
                i += (int)NoteValue.Octave;
            }
            while (true);
        }

        public IEnumerable<double> GetNoteRatios(int count)
        {
            float i = 0f;
            int countReturned = 0;
            do
            {
                foreach (var ratio in _notes)
                {
                    yield return i + MusicNote.GetNoteRatio(ratio) - 1.0d;
                    countReturned++;
                    if (countReturned >= count)
                        yield break;
                }
                i += 1.0f;
            }
            while (true);
        }

    }

}
