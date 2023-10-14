namespace MindHeal.Models.DTOs
{
    public class IssuesDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreateIssuesRequestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateIssuesRequestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
