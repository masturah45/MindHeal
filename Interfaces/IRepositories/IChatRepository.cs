using MindHeal.Models.Entities;

namespace MindHeal.Interfaces.IRepositories
{
    public interface IChatRepository : IBaseRespository
    {
        Task<List<Chat>> GetAllUnSeenChat(Guid therapistId);
        Task<List<Chat>> GetAllUnSeenChat(Guid clientId, Guid therapistId);
        Task<List<Chat>> GetAllChatFromASender(Guid loginId, Guid senderId);
        Task<List<Chat>> GetChatByChatId(Guid loginId, Guid senderId, Guid chatId);
    }
}
