using MindHeal.Interfaces.IRepositories;
using MindHeal.Interfaces.IServices;
using MindHeal.Models.DTOs;
using MindHeal.Models.Entities;

namespace MindHeal.Implementations.Services
{
    public class TherapistIssuesService : ITherapistIssuesService
    {
        private readonly ITherapistIssuesRepository _therapistissuesRepository;

        public TherapistIssuesService(ITherapistIssuesRepository therapistissuesRepository)
        {
            _therapistissuesRepository = therapistissuesRepository;
        }
        public async Task<IEnumerable<TherapistIssuesDto>> GetAll()
        {
            var therapistissues = await _therapistissuesRepository.GetAllTherapistIssues();
            var listOftherapistIssues = therapistissues.Select(a => new TherapistIssuesDto
            {
                IssuesId = a.IssuesId,
                TherapistId = a.TherapistId,
            }).ToList();
            return listOftherapistIssues;
        }

        public async Task<BaseResponse<IEnumerable<TherapistDto>>> GetTherapistByIssues(Guid IssuesId)
        {
            var therapistIssues = await _therapistissuesRepository.GetTherapistByIssues(IssuesId);
            if (therapistIssues == null) return new BaseResponse<IEnumerable<TherapistDto>>
            {
                Message = "TherapistIssue Not Found",
                Status = false,
            };

            return new BaseResponse<IEnumerable<TherapistDto>>
            {
                Message = "Successfull",
                Status = true,
                Data = therapistIssues.Select(x => new TherapistDto
                {
                    Id = IssuesId
                }).ToList(),
            };
        }

        public async Task<BaseResponse<TherapistIssuesDto>> GetTherapistIssues(Guid id)
        {
            var therapistIssues = await _therapistissuesRepository.Get<TherapistIssues>(id);
            if (therapistIssues == null) return new BaseResponse<TherapistIssuesDto>
            {
                Message = "Issue Not Found",
                Status = false,
            };

            return new BaseResponse<TherapistIssuesDto>
            {
                Message = "Successfull",
                Status = true,
                Data = new TherapistIssuesDto
                {
                    IssuesId = id,
                    TherapistId = id,
                },
            };
        }
    }
}
