using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MindHeal.Interfaces.IServices;
using MindHeal.Models.DTOs;
using MindHeal.Models.Entities;

namespace MindHeal.Controllers
{
    public class UserController : Controller
    {
        private readonly ITherapistService _therapistService;

        private readonly IUserService _userService;
        private readonly UserManager<User> _manager;
        private readonly SignInManager<User> _signInManager;

        public UserController(IUserService userService, ITherapistService therapistService, UserManager<User> manager, SignInManager<User> signInManager)
        {
            _userService = userService;
            _therapistService = therapistService;
            _manager = manager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return View(users);
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogInUserRequestModel model)
        {
            var user = await _userService.Login(model);
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("DashBoard", "Home");
            }
            ViewBag.error = "Invalid Email or password entered";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult SuperAdminBoard()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ClientBoard()
        {
            var therapists = await _therapistService.GetAll();
            return View(therapists);
            //return View();
        }
        [HttpGet]
        public IActionResult TherapistBoard()
        {
            return View();
        }
    }
}
