namespace MindHeal.Models.Entities
{
    public class Issues : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IList<TherapistIssues> TherapistIssues { get; set; } = new List<TherapistIssues>();
    }
}
