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
        public string CreatorName { get; private set; }
        public string TargetID { get; private set; }
        public string TargetName { get; private set; }
        public string NoteContent { get; private set; }

        // Will be set when written to the Database
        public long NoteID { get; set; } 
        
        public Note(string creatorID, string targetID, string targetName, string noteContent, string guild)
        {
            CreationDate = DateTime.UtcNow;
            CreatorName = creatorID;
            TargetID = targetID;
            TargetName = targetName;
            NoteContent = noteContent;
            Guild = guild;
        }

        public override bool Equals(object? obj)
        {
            if (obj.GetType() != typeof(Note)) return false;
            var note = (Note)obj;
            return note.CreatorName.Equals(CreatorName) && note.TargetID.Equals(TargetID) && note.NoteContent.Equals(NoteContent) && note.CreationDate.Equals(CreationDate);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public string ToDiscordString()
        {
            // Header
            var displayString = "Note #" + NoteID + " by **" + CreatorName + "** for  **" + TargetName + " (" + TargetID + ")** on " + CreationDate.ToShortDateString() + "\n";
            
            // Actual note content in code block
            displayString += "```\n";
            displayString += NoteContent;
            displayString += "```\n";

            return displayString;
        }
    }
}