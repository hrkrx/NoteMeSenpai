using MongoDB;
using MongoDB.Bson;

namespace NoteMeSenpai.Models
{
    public class Permission
    {
        public ObjectId _id { get; set; }
        public string Guild { get; set; }
        public string RoleName { get; set; }
        public string Command { get; set; }
    }
}