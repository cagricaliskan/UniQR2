using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UniQR2.Models;

namespace UniQR2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize()]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize("admin")]
        public IActionResult Privacy()
        {
            return View();
        }

    }
}
