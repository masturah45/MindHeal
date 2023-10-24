using MindHeal.Models.DTOs;

namespace MindHeal.Interfaces.IServices
{
    public interface IClientService
    {
        Task<BaseResponse<ClientDto>> Create(CreateClientRequestModel model);
        Task<BaseResponse<ClientDto>> Update(Guid id, UpdateClientRequestModel model);
        Task<BaseResponse<ClientDto>> GetClient(Guid id);
        Task<BaseResponse<ClientDto>> Delete(Guid id);
        Task<IEnumerable<ClientDto>> GetAll();
        Task<List<UserDto>> GetAllClientByChat();
        Task<BaseResponse<ClientDto>> GetClientForProfile(string id);
    }
}
