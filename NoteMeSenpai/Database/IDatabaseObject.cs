using MongoDB.Bson;

namespace NoteMeSenpai.Database
{
    public interface IDatabaseObject
    {
        ObjectId _id { get; set; }
    }
}