using MindHeal.Models.DTOs;

namespace MindHeal.Interfaces.IServices
{
    public interface INotificationMessage
    {
        Task SendWhatsappMessageAsync(WhatsappMessageSenderRequestModel model);
    }
}
