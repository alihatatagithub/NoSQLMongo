using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace ConsoleAppAmTimCorey_Mongo_Db
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoCRUD db = new MongoCRUD("AddressBookLast");
            //db.InsertRecord("Users", new PersonModel { FirstName = "Amr",LastName = "Mousa"});
            var recs = db.LoadRecords<PersonModel>("User");
            foreach (var rec in recs)
            {
                Console.WriteLine($" {rec.FirstName}");
            }
          var onerec = db.LoadRecordById<PersonModel>("Users", new Guid("pZUJXlBAvkianYsVLqcebA=="));
            onerec.BirthDate = new DateTime(1998, 9, 17, 0, 0, 0, DateTimeKind.Utc);
            db.UpsertRecord("Users", onerec.Id, onerec);
            db.DeleteRecord<PersonModel>("Users", onerec.Id);

        }

    }

    public class NameModel
    {
        [BsonId] //_id
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public class PersonModel
    {
        [BsonId] //_id
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [BsonElement("DOB")]
        public DateTime BirthDate { get; set; }

    }

    public class MongoCRUD
    {

        IMongoDatabase db;
        public MongoCRUD(string database)
        {
            var client = new MongoClient();
            db = client.GetDatabase(database);
            

        }
        public void InsertRecord<T>(string table, T record)
        {
            var collections = db.GetCollection<T>(table);
            collections.InsertOne(record);

        }
        public List<T> LoadRecords<T>(string table)
        {
            var collection = db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();
        }


        public T LoadRecordById<T>(string table, Guid id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);
            return collection.Find(filter).First();

        }
        public void UpsertRecord<T>(string table,Guid id,T record)
        {
            var collection = db.GetCollection<T>(table);
            var result = collection.ReplaceOne(new BsonDocument("_id", id), record, new ReplaceOptions { IsUpsert = true });
        }

        public void DeleteRecord<T> (string table,Guid id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);
            collection.DeleteOne(filter);

        }


    }
}
