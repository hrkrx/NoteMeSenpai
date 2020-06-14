using System;

namespace NoteMeSenpai.Models
{
    public class Note {
        public DateTime CreationDate { get; private set; }
        public string CreatorID { get; private set; }
        public string TargetID { get; private set; }
        public string NoteContent { get; private set; }

        // Will be set when written to the Database
        public long NoteID { get; set; }     
        
        public Note(string creatorID, string targetID, string noteContent)
        {
            CreationDate = DateTime.UtcNow;
            CreatorID = creatorID;
            TargetID = targetID;
            NoteContent = noteContent;
        }
    }
}