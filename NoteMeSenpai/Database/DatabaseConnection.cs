using MongoDB.Driver.Core;
using MongoDB.Driver;
using NoteMeSenpai.Models;
using MongoDB.Bson;
using System.Linq.Expressions;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
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

        public void Save<T>(T obj) where T : IDatabaseObject
        {
            var collection = database.GetCollection<T>(typeof(T).FullName);
            collection.InsertOne(obj);  
        }

        public void Update<T>(T obj) where T : IDatabaseObject
        {
            var collection = database.GetCollection<T>(typeof(T).FullName);
            var filter = Builders<T>.Filter.Eq("_id", obj._id); 
            
            // sanitycheck for the filter
            var sanitycheck = collection.Find(filter).First();

            var properties = obj.GetType().GetProperties();
            var updateNeeded = false;
            foreach (var property in properties)
            {
                if (!property.GetValue(obj).Equals(property.GetValue(sanitycheck)))
                {
                    updateNeeded = true;
                }
            }
            if (updateNeeded)
            {
                var options = new FindOneAndReplaceOptions<T>
                {
                    ReturnDocument = ReturnDocument.After
                };
                var result = collection.FindOneAndReplace(filter, obj,options);
            }
        }

        public T Get<T>(Expression<Func<T, bool>> filter) where T : IDatabaseObject
        {
            var collection = database.GetCollection<T>(typeof(T).FullName);
            var result = collection.Find(filter).FirstOrDefault();
            return result;
        }

        public IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> filter = null) where T : IDatabaseObject
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