using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniQR2.Models;
using X.PagedList;

namespace UniQR2.Controllers
{
    [Authorize("Administrator")]
    public class ClassController: Controller
    {
        private readonly ModelContext db;

        public ClassController(ModelContext db)
        {
            this.db = db;
        }

        public IActionResult Index(int page = 1, string search = "")
        {
            var courseclassroom = db.CourseClassrooms.Include(x => x.Instructor).AsQueryable();
            if(search != "")
            {
                courseclassroom = db.CourseClassrooms.Where(x => x.Instructor.FullName.Contains(search));

                ViewBag.search = search;
                ViewBag.count = courseclassroom.Count();
            }

            courseclassroom = courseclassroom.OrderByDescending(x => x.CourseClassroomID);
            ViewBag.page = page;
            return View(courseclassroom.ToPagedList(page,10));
        }

    }
}
