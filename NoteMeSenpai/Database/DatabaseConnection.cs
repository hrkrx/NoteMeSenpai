using MongoDB.Driver.Core;
using MongoDB.Driver;
using NoteMeSenpai.Models;
using MongoDB.Bson;
using System.Linq.Expressions;
using System.Linq;
using System.Collections.Generic;
using MongoDB;
using System;

namespace NoteMeSenpai.Database
{
    public class DatabaseConnection {
        private const string DATABASE_NAME = "NoteMeSenpai";
        private MongoClient client;
        private IMongoDatabase database;


        public DatabaseConnection(string connectionString)
        {
            client = new MongoClient(connectionString);
            database = client.GetDatabase(DATABASE_NAME);
        }

        public void Save<T>(T obj)
        {
            var collection = database.GetCollection<T>(typeof(T).FullName);
            collection.InsertOne(obj);  
        }

        public T Get<T>(Expression<Func<T, bool>> filter)
        {
            var collection = database.GetCollection<T>(typeof(T).FullName);
            var result = collection.Find(filter).FirstOrDefault();
            return result;
        }

        public IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> filter = null)
        {
            var collection = database.GetCollection<T>(typeof(T).FullName);
            if (filter != null)
            {
                var result = collection.Find(filter).ToEnumerable();
                return result;
            }
            else
            {
                var result = collection.Find(_ => true).ToEnumerable();
                return result;
            }
        }

        public void Delete<T> (T obj) where T : IDatabaseObject
        {
            var collection = database.GetCollection<T>(typeof(T).FullName);
            var filter = Builders<T>.Filter.Eq("_id", obj._id); 
            var result = collection.DeleteOne(filter);
        }
    }   
}