using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace mongo_leaf_validator_example.Controllers
{
    public class Audit
    {
        public string Location { get; set; }
    }

    public static class AuditExtensions
    {
        public static IEnumerable<UpdateDefinition<BsonDocument>> ToMongoUpdates(this IEnumerable<Audit> audits)
        {
            return audits
                .OfType<AuditUpdatedValue>()
                .Select(audit => Builders<BsonDocument>.Update.Set(audit.Location, audit.UpdatedValue))
                .Concat(audits
                .OfType<AuditUndefinedValue>()
                .Select(audit => Builders<BsonDocument>.Update.Unset(audit.Location)));
        }
    }
}
