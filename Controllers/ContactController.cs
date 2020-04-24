using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mongo_leaf_validator_example.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace mongo_leaf_validator_example.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly MongoClient mongoClient;

        public ContactController(MongoClient mongoClient)
        {
            this.mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<object> GetContact()
        {
            var db = mongoClient.GetDatabase("stewie-test");
            return (await db
                    .GetCollection<object>("Contacts")
                    .FindAsync(Builders<object>.Filter.Eq("contactNumber", 0)))
                    .First();
        }

        [HttpPost]
        public async Task UpdateContact([FromBody] List<DiffRequest> diffs)
        {
            var db = mongoClient.GetDatabase("stewie-test");

            var audits = diffs.ToAudits();

            await db
                .GetCollection<BsonDocument>("Contacts")
                .UpdateOneAsync(
                    Builders<BsonDocument>.Filter.Eq("contactNumber", 0),
                    Builders<BsonDocument>.Update.Combine(audits.ToMongoUpdates()));

            string json = JsonConvert.SerializeObject(new { diffs = audits });
            await db.GetCollection<BsonDocument>("Audits").InsertOneAsync(BsonDocument.Parse(json));
        }
    }
}
