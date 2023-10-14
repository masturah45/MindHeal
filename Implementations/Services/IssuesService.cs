using MindHeal.Interfaces.IRepositories;
using MindHeal.Interfaces.IServices;
using MindHeal.Models.DTOs;
using MindHeal.Models.Entities;

namespace MindHeal.Implementations.Services
{
    public class IssuesService : IIssuesService
    {
        private readonly IIssuesRepository _issuesRepository;

        public IssuesService(IIssuesRepository issuesRepository)
        {
            _issuesRepository = issuesRepository;
        }
        public async Task<BaseResponse<IssuesDto>> Create(CreateIssuesRequestModel model)
        {
            var issueExist = await _issuesRepository.GetIssues(b => b.Name == model.Name);
            if (issueExist != null) return new BaseResponse<IssuesDto>
            {
                Message = "Issue already exist",
                Status = false,
            };

            var issue = new Issues
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                IsDeleted = false,
            };

            await _issuesRepository.Add(issue);

            return new BaseResponse<IssuesDto>
            {
                Status = true,
                Message = "Created Successfully",
                Data = new IssuesDto
                {
                    Name = model.Name
                }
            };
        }

        public async Task<BaseResponse<IssuesDto>> Delete(Guid id)
        {
            var issues = await _issuesRepository.Get<Issues>(id);
            if (issues == null) return new BaseResponse<IssuesDto>
            {
                Message = "Issue Not Found",
                Status = false,
            };

            issues.IsDeleted = true;
            await _issuesRepository.save();

            return new BaseResponse<IssuesDto>
            {
                Message = "Deleted Successfully",
                Status = true,
            };
        }

        public async Task<BaseResponse<IEnumerable<IssuesDto>>> GetAllIssues()
        {
            var issues = await _issuesRepository.GetAllIssues();
            var listOfIssues = issues.ToList().Select(b => new IssuesDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
            });
            return new BaseResponse<IEnumerable<IssuesDto>>
            {
                Message = "ok",
                Status = true,
                Data = listOfIssues,
            };
        }

        public async Task<BaseResponse<IssuesDto>> GetIssues(Guid id)
        {
            var issues = await _issuesRepository.Get<Issues>(id);
            if (issues == null) return new BaseResponse<IssuesDto>
            {
                Message = "Issue Not Found",
                Status = false,
            };

            return new BaseResponse<IssuesDto>
            {
                Message = "Successfull",
                Status = true,
                Data = new IssuesDto
                {
                    Id = issues.Id,
                    Name = issues.Name,
                    Description = issues.Description,
                },
            };
        }

        public async Task<BaseResponse<IssuesDto>> Update(Guid id, UpdateIssuesRequestModel model)
        {
            var issue = await _issuesRepository.Get<Issues>(id);
            if (issue == null) return new BaseResponse<IssuesDto>
            {
                Message = "issue not found",
                Status = false,
            };
            issue.Name = model.Name;
            issue.Description = model.Description;
            await _issuesRepository.Update(issue);

            return new BaseResponse<IssuesDto>
            {
                Message = "Successfully Updated",
                Status = true,
                Data = new IssuesDto
                {
                    Name = model.Name
                }
            };
        }
    }
}
