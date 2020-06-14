using MongoDB;
using MongoDB.Bson;
using NoteMeSenpai.Database;

namespace NoteMeSenpai.Models
{
    public class Permission : IDatabaseObject
    {
        public ObjectId _id { get; set; }
        public string Guild { get; set; }
        public string RoleName { get; set; }
        public string Command { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj.GetType() != typeof(Permission)) return false;
            var permission = (Permission)obj;
            return permission.Guild.Equals(Guild) && permission.RoleName.Equals(RoleName) && permission.Command.Equals(Command);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}