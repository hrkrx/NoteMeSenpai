using System;
using MongoDB.Bson;
using NoteMeSenpai.Database;

namespace NoteMeSenpai.Models
{
    public class Note : IDatabaseObject
    {
        public ObjectId _id { get; set; }
        public string Guild { get; set; }
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

        public override bool Equals(object? obj)
        {
            if (obj.GetType() != typeof(Note)) return false;
            var note = (Note)obj;
            return note.CreatorID.Equals(CreatorID) && note.TargetID.Equals(TargetID) && note.NoteContent.Equals(NoteContent) && note.CreationDate.Equals(CreationDate);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}