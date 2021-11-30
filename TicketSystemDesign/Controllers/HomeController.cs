using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TicketSystemDesign.Models;

namespace TicketSystemDesign.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private TickerSystemContext _context;

        public HomeController(ILogger<HomeController> logger , TickerSystemContext context , IConfiguration config)
        {
            _logger = logger;
            _context = context;
        }

        [AutoValidateAntiforgeryToken]
        [ResponseCache(NoStore = true)]
        [Authorize]

        public IActionResult Index()
        {
            List<TicketInfoModel> ticketModel = new List<TicketInfoModel>();

            string userName = HttpContext.User.Claims.FirstOrDefault(m => m.Type == "UserName").Value;
            string status = HttpContext.User.Claims.FirstOrDefault(m => m.Type == "RoleStatus").Value;
            string userId = HttpContext.User.Claims.FirstOrDefault(m => m.Type == "UserId").Value;

            ticketModel = TicketDac.GetTicketList().ToList();
            
            return View(ticketModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult NoAuth()
        {
            return View();
        }
    }
}
