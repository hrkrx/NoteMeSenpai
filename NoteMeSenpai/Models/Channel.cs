using System;
using DSharpPlus.Entities;
using MongoDB.Bson;
using NoteMeSenpai.Database;

namespace NoteMeSenpai.Models
{
    public class Channel : IDatabaseObject
    {
        public ObjectId _id { get; set; }
        public string ChannelId { get; set; }
        public string ChannelName { get; set; }
        public bool IsDefault { get; set; }
        public string Guild { get; set; }
    }

}