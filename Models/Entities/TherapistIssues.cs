namespace MindHeal.Models.Entities
{
    public class TherapistIssues : BaseEntity
    {
        public Guid TherapistId { get; set; }
        public Guid IssuesId { get; set; }
        public Therapist Therapist { get; set; }
        public Issues Issues { get; set; }
    }
}
