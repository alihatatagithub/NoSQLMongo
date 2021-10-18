using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoDb
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoCRUD db = new MongoCRUD("AddressBook");
            //PersonModel person = new PersonModel
            //{
            //    FirstName = "Joe ",
            //    LastName = "Smith",
            //    PrimaryAddress = new AddressModel

            //    {
            //        StreetAddress = "101 oak street",
            //        City = "Scranton",
            //        State="SA",
            //        ZipCode="18512"

            //    }
            //};
            //db.InsertRecord("Users",person);
            var recs = db.LoadRecord<PersonModel>("Users");
            foreach (var rec in recs)
            {
                Console.WriteLine($"{rec.Id}:{rec.FirstName}:{rec.LastName}");
            }

            Console.ReadLine(); ;
        }

    }
    public class PersonModel
    {
        [BsonId] //_id
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AddressModel PrimaryAddress { get; set; }
    }
    public class AddressModel
    {
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
    public class MongoCRUD
    {
       private IMongoDatabase db;
        public MongoCRUD(string database)
        {
            var client = new MongoClient();
            db = client.GetDatabase(database);

        }
        public void InsertRecord<T>(string table, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }
        public List<T> LoadRecord<T>(string table)
        {
            var collection = db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();
        }
    }

}
