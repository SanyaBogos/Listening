using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using WebListening.Models;

namespace WebListening.Repositories
{
    public class TextsMongoRepository : IRepository<Text>
    {
        string _collectionName;
        IMongoClient _client;
        IMongoDatabase _dataBase;
        IMongoCollection<Text> _collection;

        public TextsMongoRepository(
            string url = "mongodb://localhost:27017",
            string dbName = "EnglishListeningDb",
            string collectionName = "Texts")
        {
            _client = new MongoClient(url);
            _dataBase = _client.GetDatabase(dbName);
            _collectionName = collectionName;
            _collection = _dataBase.GetCollection<Text>(_collectionName);
        }

        public void Delete(string id)
        {
            var filter = Builders<Text>.Filter.Eq("TextId", ObjectId.Parse(id));
            _collection.DeleteOne(filter);
        }

        //[DatabaseExceptionWrapper]
        public IQueryable<Text> GetAll()
        {
            //var collection = _dataBase.GetCollection<Text>(_collectionName);
            return _collection.Find(new BsonDocument()).ToList().AsQueryable();
        }

        public Text GetById(string id)
        {
            //var collection = _dataBase.GetCollection<Text>(_collectionName);
            var filter = Builders<Text>.Filter.Eq("TextId", ObjectId.Parse(id));
            return _collection.Find(filter).ToList().First();
        }

        public void Insert(Text item)
        {
            //var collection = _dataBase.GetCollection<Text>(_collectionName);
            _collection.InsertOne(item);
        }

        public void Update(Text item)
        {
            //var collection = _dataBase.GetCollection<Text>(_collectionName);
            var filter = Builders<Text>.Filter.Eq("TextId", item.TextId);
            var update = Builders<Text>.Update
                            .Set("Title", item.Title)
                            .Set("SubTitle", item.SubTitle)
                            .Set("WordsInParagraphs", item.WordsInParagraphs)
                            .Set("AudioName", item.AudioName);
            _collection.UpdateOne(filter, update);
        }
    }
}
