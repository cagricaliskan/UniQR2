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
        private readonly ModelContext db;
        public HomeController(ILogger<HomeController> logger, ModelContext db)
        {
            _logger = logger;
            this.db = db;
        }

        [Authorize()]
        public IActionResult Index()
        {
            return View(db.Users.Where(n=> n.UserRole == UserRole.Instructor).ToList());
        }

        [Authorize("Administrator")]
        public IActionResult Privacy()
        {
            return View();
        }

    }
}
