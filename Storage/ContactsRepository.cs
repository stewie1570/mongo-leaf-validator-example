using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Storage
{
    public interface IContactsRepository
    {
        Task<object> GetContact();
        Task UpdateContact(List<DiffRequest> diffs);
    }

    public class ContactsRepository : IContactsRepository
    {
        private readonly MongoClient mongoClient;

        public ContactsRepository(MongoClient mongoClient)
        {
            this.mongoClient = mongoClient;
        }

        public async Task<object> GetContact()
        {
            var db = mongoClient.GetDatabase("stewie-test");
            return (await db
                    .GetCollection<object>("Contacts")
                    .FindAsync(Builders<object>.Filter.Eq("contactNumber", 0)))
                    .First();
        }

        public async Task UpdateContact(List<DiffRequest> diffs)
        {
            var db = mongoClient.GetDatabase("stewie-test");

            var audits = diffs.ToAudits();

            await db
                .GetCollection<BsonDocument>("Contacts")
                .UpdateOneAsync(
                    Builders<BsonDocument>.Filter.Eq("contactNumber", 0),
                    Builders<BsonDocument>.Update.Combine(audits.ToMongoUpdates()));

            await db
                .GetCollection<BsonDocument>("Audits")
                .InsertOneAsync(BsonDocument
                    .Parse(JsonConvert
                        .SerializeObject(new { diffs = audits })));
        }
    }
}