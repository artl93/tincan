using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinCan
{
    public class NoteStartedEventArg : EventArgs
    {
        public int Note { get; private set; }
        public EventLocation Location { get; private set; }

        public NoteStartedEventArg(EventLocation location, int note) 
        {
            this.Location = location;
            this.Note = note;
        }
    }
}
