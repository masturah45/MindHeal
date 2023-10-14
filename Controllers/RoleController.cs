using Microsoft.AspNetCore.Mvc;
using MindHeal.Interfaces.IServices;
using MindHeal.Models.DTOs;

namespace MindHeal.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
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
        public async Task<IActionResult> Create(CreateRoleRequestModel model)
        {
            var role = await _roleService.Create(model);
            if (role.Status == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
