using Microsoft.Extensions.Options;
using MindHeal.Interfaces.IServices;
using MindHeal.Models.DTOs;
using RestSharp;

namespace MindHeal.Implementations.Services
{
    public class NotificationMessage : INotificationMessage
    {
        private WhatsappMessageSettings _settings;

        public NotificationMessage(IOptions<WhatsappMessageSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task SendWhatsappMessageAsync(WhatsappMessageSenderRequestModel model)
        {
            var client = new RestClient(_settings.url);

            var request = new RestRequest(_settings.url, Method.Post);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("token", _settings.Token);
            request.AddParameter("to", model.ReciprantNumber);
            request.AddParameter("body", model.MessageBody);

            RestResponse response = await client.ExecuteAsync(request);
            var output = response.Content;
        }
    }
}
