using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.Controllers
{
    [Authorize("Administrator")]
    public class Classroom: Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
