using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Pokemon()
        {
            return View();
        }
        [Authorize(Roles = "Manager")]
        public IActionResult Employee()
        {
            return View();
        }

        [HttpGet("/NotFound")]
        public IActionResult NotFound_404()
        {
            return View();
        }

        [HttpGet("/Forbidden")]
        public IActionResult Forbidden_403()
        {
            return View();
        }

        [HttpGet("/Unauthorize")]
        public IActionResult Unauthorize_401()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
