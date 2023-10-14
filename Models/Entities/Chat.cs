namespace MindHeal.Models.Entities
{
    public class Chat : BaseEntity
    {
        public Guid SenderId { get; set; }
        public Guid RecieverId { get; set; }
        public string Message { get; set; }
        public bool Seen { get; set; }
        public string PostedTime { get; set; }
    }
}
