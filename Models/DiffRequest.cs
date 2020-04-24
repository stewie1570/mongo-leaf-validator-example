using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace mongo_leaf_validator_example.Models
{
    public class DiffRequest
    {
        public string Location { get; set; }
        public JsonElement UpdatedValue { get; set; }
    }

    public static class DiffRequestExtensions
    {
        public static List<Audit> ToAudits(this IEnumerable<DiffRequest> diffs)
        {
            return diffs
                .Select(diff =>
                {
                    switch (diff.UpdatedValue.ValueKind)
                    {
                        case JsonValueKind.False:
                        case JsonValueKind.True:
                            return (Audit)new AuditUpdatedValue { Location = diff.Location, UpdatedValue = diff.UpdatedValue.GetBoolean() };
                        case JsonValueKind.Number:
                            return new AuditUpdatedValue { Location = diff.Location, UpdatedValue = diff.UpdatedValue.GetDouble() };
                        case JsonValueKind.Undefined:
                            return new AuditUndefinedValue { Location = diff.Location };
                        default:
                            return new AuditUpdatedValue { Location = diff.Location, UpdatedValue = diff.UpdatedValue.GetString() };
                    }
                })
                .ToList();
        }
    }
}
