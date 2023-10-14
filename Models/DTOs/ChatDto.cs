namespace MindHeal.Models.DTOs
{
    public class ChatDto
    {
        public Guid SenderId { get; set; }
        public Guid RecieverId { get; set; }
        public string? Message { get; set; }
        public bool Seen { get; set; }
        public string? PostedTime { get; set; }
        public Guid LoggedInId { get; set; }
    }

    public class CreateChatRequestModel
    {
        public string Message { get; set; }
        public bool Seen { get; set; }
        public string PostedTime { get; set; }
    }
}
