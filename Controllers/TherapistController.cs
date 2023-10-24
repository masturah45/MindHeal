using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MindHeal.Implementations.Services;
using MindHeal.Interfaces.IServices;
using MindHeal.Models.DTOs;
using MindHeal.Models.Entities;
using System.Security.Claims;

namespace MindHeal.Controllers
{
    public class TherapistController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ITherapistService _therapistService;
        private readonly ITherapistIssuesService _therapistIssuesService;
        private readonly IIssuesService _issuesService;

        public TherapistController(IClientService clientService, ITherapistService therapistService, ITherapistIssuesService therapistIssuesService, IIssuesService issuesService)
        {
            _clientService = clientService;
            _therapistService = therapistService;
            _therapistIssuesService = therapistIssuesService;
            _issuesService = issuesService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var issues = await _issuesService.GetAllIssues();
            var selectedIissues = new SelectList(issues.Data, "Id", "Name");
            var requestModel = new CreateTherapistRequestModel
            {
                Issues = selectedIissues,
            };
            return View(requestModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTherapistRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _therapistService.Create(model);
                if(result.Status)
                {
                    TempData["success"] = "Therapist Created Successfully";
                    return RedirectToAction("Index", "Home");
                }
                TempData["error"] = "Therapist Created error";
                return View(model);
            }
            TempData["error"] = "Therapist Created error";
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {

            var therapist = await _therapistService.GetTherapistForProfile(id);
            if (therapist == null)
            {
                ViewBag.Error = "Therapist doesnt exist";
            }
            return View(therapist.Data);
        }
        public async Task<IActionResult> UpdateAvailability()
        {
            
                var Id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var t = await _therapistService.UpdateAvailability(Guid.Parse(Id));
                TempData["success"] = "Therapist edited successfully";
                return RedirectToAction("Dashboard", "User");
            
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, UpdateTherapistRequestModel model)
        {
            var t = await _therapistService.Update(Guid.Parse(id), model);
            TempData["success"] = "Therapist edited successfully";
            return RedirectToAction("Profile");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {

            var therapist = await _therapistService.GetTherapist(id);
            if (!therapist.Status)
            {
                return NotFound();
            }
            return View(therapist.Data);
        }

        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _therapistService.Delete(Guid.Parse(id));
            TempData["error"] = "Therapist deleted successfully";
            return RedirectToAction("GetAll", "Therapist");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var therapists = await _therapistService.GetAllAvailableTherapist();
           
            TempData["success"] = "All Therapist";
            return View(therapists);
        }

        [HttpGet]

        public async Task<IActionResult> Profile(string id)
       {
            var therapist = await _therapistService.GetTherapistForProfile(id);
            TempData["success"] = "Therapist Profile";
            if (!therapist.Status)
            {
                return NotFound();
            }
            return View(therapist.Data);
        }
        [HttpGet]
        public async Task<IActionResult> ViewUnApprovedTherapist()
        {
            var therapist = await _therapistService.ViewUnapprovedTherapist();
            if (therapist.Status == true)
            {
                return View(therapist.Data);
            }
            return NotFound();

        }
        [HttpGet]
        public async Task<IActionResult> ViewApprovedTherapist()
        {
            var therapist = await _therapistService.ViewapprovedTherapist();
            if (therapist.Status == true)
            {
                return View(therapist.Data);
            }
            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> ViewRejectedTherapist()
        {
            var therapist = await _therapistService.ViewRejectedTherapist();
            if (therapist.Status == true)
            {
                return View(therapist.Data);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAvailableTherapist()
        {
            var therapists = await _therapistService.GetAllAvailableTherapist();
            TempData["success"] = "All Available Therapist";
            return View(therapists);
        }

        [HttpGet]
        public async Task<IActionResult> GetTherapistByIssues(Guid IssueId)
        {
            var therapistIssues = await _therapistIssuesService.GetTherapistByIssues(IssueId);
            return View(therapistIssues);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTherapist()
        {
            var therapists = await _therapistService.GetAll();
            TempData["success"] = "All Therapist";
            return View(therapists);
        }

        [HttpGet]
        public async Task<IActionResult> RejectTherapist(Guid id)
        {
            var therapist = await _therapistService.RejectapprovedTherapist(id);
            return RedirectToAction("Dashboard", "User");
        }

        [HttpGet]

        public async Task<IActionResult> ApprovedTherapist(Guid id)
        {
            var therapist = await _therapistService.Approve(id);
            return RedirectToAction("Dashboard", "User");
        }
    }
}
