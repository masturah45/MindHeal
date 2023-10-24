using Microsoft.AspNetCore.Mvc;
using MindHeal.Interfaces.IServices;
using MindHeal.Models.DTOs;
using MindHeal.Models.Entities;

namespace MindHeal.Controllers
{
    public class ClientController : Controller
    {
        private readonly ITherapistService _therapistService;
        private readonly IClientService _clientService;

        public ClientController(ITherapistService therapistService, IClientService clientService)
        {
            _therapistService = therapistService;
            _clientService = clientService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateClientRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _clientService.Create(model);
                if(result.Status)
                {
                    TempData["success"] = "Client Created Successfully";
                    return RedirectToAction("Index", "Home");
                }

                TempData["error"] = "Client Created error";
                return View(model);
            }

            TempData["error"] = "Client Created error";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var client = await _clientService.GetClientForProfile(id);
            if (client == null)
            {
                ViewBag.Error = "Client doesnt exist";
            }
            return View(client.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, UpdateClientRequestModel model)
        {
            await _clientService.Update(Guid.Parse(id), model);
            TempData["success"] = "Client updated successfully";
            return RedirectToAction("Dashboard", "User");
        }

        public async Task<IActionResult> Delete(Guid id)
        {

            var client = await _clientService.GetClient(id);
            if (!client.Status)
            {
                return NotFound();
            }
            return View(client.Data);
        }

        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _clientService.Delete(Guid.Parse(id));
            TempData["error"] = "Client deleted successfully";
            return RedirectToAction("GetAll", "Client");
        }
        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {
            var client = await _clientService.GetClientForProfile(id);
            TempData["sucess"] = "Client Profile";
            if (!client.Status)
            {
                return NotFound();
            }
            return View(client.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _clientService.GetAll();
            TempData["success"] = "All Client";
            return View(clients);
        }
    }
}
