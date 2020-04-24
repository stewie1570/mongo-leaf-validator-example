using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace mongo_leaf_validator_example.Controllers
{
    public class Diff
    {
        public string Location { get; set; }
        public JsonElement UpdatedValue { get; set; }
    }

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
        public async Task UpdateContact([FromBody] List<Diff> diffs)
        {
            var db = mongoClient.GetDatabase("stewie-test");

            var updates = diffs
                .Select(diff =>
                {
                    switch (diff.UpdatedValue.ValueKind)
                    {
                        case JsonValueKind.False:
                        case JsonValueKind.True:
                            return Builders<BsonDocument>.Update.Set(diff.Location, diff.UpdatedValue.GetBoolean());
                        case JsonValueKind.Number:
                            return Builders<BsonDocument>.Update.Set(diff.Location, diff.UpdatedValue.GetDouble());
                        case JsonValueKind.Undefined:
                            return Builders<BsonDocument>.Update.Unset(diff.Location);
                        default:
                            return Builders<BsonDocument>.Update.Set(diff.Location, diff.UpdatedValue.GetString());
                    }
                });

            await db
                .GetCollection<BsonDocument>("Contacts")
                .UpdateOneAsync(
                    Builders<BsonDocument>.Filter.Eq("contactNumber", 0),
                    Builders<BsonDocument>.Update.Combine(updates));
        }
    }
}
