using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinCan
{
    public class NoteEventMap
    {
        int[] _rowToNotes;
        int _offset;
        Dictionary<int, int> _noteToRow = new Dictionary<int, int>();

        public NoteEventMap(IEnumerable<NoteValue> scaleNotes, int rows, int offset)
        {
            _offset = offset;
            _rowToNotes = new Scale(scaleNotes).GetNoteIndices(rows, offset).Reverse().ToArray();
            int row = 0;
            foreach (var note in _rowToNotes)
            {
                _noteToRow[note] = row;
                row++;
            }
        }
        public int GetNote(int gridRow) 
        { 
            return _rowToNotes[gridRow]; 
        }

        public int GetGridRow(int note)
        {
            return _noteToRow[note];
        }

        public int[] GetNotes()
        {
            return _rowToNotes;
        }
    }
}
