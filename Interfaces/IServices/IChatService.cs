using MindHeal.Models.DTOs;

namespace MindHeal.Interfaces.IServices
{
    public interface IChatService
    {
        Task<BaseResponse<ChatDto>> CreateChat(CreateChatRequestModel model, Guid loginId, Guid recieverId, string role);
        Task<BaseResponse<List<ChatDto>>> GetAllChatFromASender(Guid loginId, Guid senderId, string role);
        Task<BaseResponse<ChatDto>> MarkAllChatAsRead(Guid clientId, Guid therapistId);
    }
}
