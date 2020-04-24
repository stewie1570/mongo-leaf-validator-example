using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace mongo_leaf_validator_example.Models
{
    public class Audit
    {
        public string Location { get; set; }
    }

    public class AuditUpdatedValue : Audit
    {
        public object UpdatedValue { get; set; }
    }

    public class AuditUndefinedValue : Audit
    {
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
