using Microsoft.AspNetCore.Mvc;
using MindHeal.Interfaces.IServices;
using MindHeal.Models.DTOs;

namespace MindHeal.Controllers
{
    public class IssuesController : Controller
    {
        private readonly IIssuesService _issuesService;

        public IssuesController(IIssuesService issuesService)
        {
            _issuesService = issuesService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateIssuesRequestModel model)
        {
            var issue = await _issuesService.Create(model);
            if (issue.Status == true)
            {
                TempData["success"] = "Issue Created Successfully";
                return RedirectToAction("GetAll");
            }
            return View();
        }

        public async Task<IActionResult> Delete(Guid id)
        {

            var issued = await _issuesService.GetIssues(id);
            if (issued == null)
            {
                ViewBag.Error = "doesnt exist";
            }
            return View(issued.Data);
        }

        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _issuesService.Delete(id);
            TempData["error"] = "Issue deleted Successfully";
            return RedirectToAction("GetAll", "Issues");
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var issues = await _issuesService.GetAllIssues();
            TempData["success"] = "All Issues";
            return View(issues.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Profile(Guid id)
        {
            var ho = await _issuesService.GetIssues(id);
            TempData["success"] = "Issue Profile";
            return View(ho.Data);
        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var issued = await _issuesService.GetIssues(id);
            if (issued == null)
            {
                ViewBag.Error = "Doesnt exist";
            }
            return View(issued.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Guid id, UpdateIssuesRequestModel model)
        {
            var issued = await _issuesService.Update(id, model);
            TempData["success"] = "Issue updated Successfully";
            return RedirectToAction("GetAll", "Issues");
        }
    }
}
