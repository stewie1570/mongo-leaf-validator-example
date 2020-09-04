namespace Domain.Models
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
}
