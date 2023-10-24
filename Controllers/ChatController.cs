using Microsoft.AspNetCore.Mvc;
using MindHeal.Interfaces.IServices;
using MindHeal.Models.DTOs;
using System.Security.Claims;

namespace MindHeal.Controllers
{
    public class ChatController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IChatService _chatService;
        private readonly IClientService _clientService;
        private readonly ITherapistService _therapistService;

        public ChatController(IConfiguration config, IChatService chatService, IClientService clientService, ITherapistService therapistService)
        {
            _config = config;
            _chatService = chatService;
            _clientService = clientService;
            _therapistService = therapistService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateChatRequestModel model, Guid senderId, string role)
        {
            var Id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var chat = await _chatService.CreateChat(model, Guid.Parse(Id), senderId, role);
            return RedirectToAction("Chat", "GetAllChats");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllChats()
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (role == "Therapist")
            {
                var clients = await _clientService.GetAllClientByChat();
                return View(clients);
            }
            else if (role == "Client")
            {
                var therapists = await _therapistService.GetAllTherapistByChat();
                return View(therapists);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Chat([FromRoute] Guid id, CreateChatRequestModel model)
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (HttpContext.Request.Method == "POST")
            {
                var Id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var chat = await _chatService.CreateChat(model, Guid.Parse(Id), id, role);
            }
               var loginid = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           //if(role == "Client")
           // {
           //     var client = _clientService.GetClientForProfile(loginid);
           //     ViewBag.ClientName = client.Result.Data.FirstName;
           // }
           //else
           // {
           //     var therapist = _therapistService.GetTherapistForProfile(loginid);
           //     ViewBag.ClientName = therapist.Result.Data.FirstName;
           // }
            var chats = await _chatService.GetAllChatFromASender(Guid.Parse(loginid), id, role);
            return View(chats.Data);
        }
    } 
}
