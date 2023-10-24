using Microsoft.AspNetCore.Mvc;
using MindHeal.Interfaces.IServices;
using MindHeal.Models.Entities;
using System.Security.Claims;

namespace MindHeal.Controllers
{
    public class SuperAdminController : Controller
    {
        private readonly ISuperAdminService _superAdminService;
        public SuperAdminController(ISuperAdminService superAdminService)
        {
            _superAdminService = superAdminService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile(Guid id)
        {
            //var id = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var superAdmin = await _superAdminService.GetSuperAdmin(id);
            TempData["success"] = "SuperAdmin Profile";
            if (!superAdmin.Status)
            {
                return NotFound();
            }
            return View(superAdmin.Data);
        }
    }
}
