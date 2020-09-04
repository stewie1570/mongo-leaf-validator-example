using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Storage
{
    public static class AuditExtensions
    {
        public static IEnumerable<UpdateDefinition<BsonDocument>> ToMongoUpdates(this IEnumerable<Audit> audits)
        {
            return audits
                .OfType<AuditUpdatedValue>()
                .Select(audit => Builders<BsonDocument>
                    .Update
                    .Set(audit.Location, audit.UpdatedValue))
                .Concat(audits
                .OfType<AuditUndefinedValue>()
                .Select(audit => Builders<BsonDocument>
                    .Update
                    .Unset(audit.Location)));
        }
    }
}