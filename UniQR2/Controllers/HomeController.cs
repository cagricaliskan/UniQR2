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
        private readonly ModelContext db;


        public HomeController(
            ModelContext db
            )
        {
            this.db = db;
        }

        [Authorize()]
        public IActionResult Index()
        {
            return View();
        }


    }
}
