using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniQR2.Models;
using X.PagedList;

namespace UniQR2.Controllers
{
    [Authorize("Administrator")]
    public class CourseController : Controller
    {
        private readonly ModelContext db;

        public CourseController(ModelContext db)
        {
            this.db = db;
        }

        public IActionResult Index(int page = 1, string search = "" )
        {
            var course = db.Course.AsQueryable();
            if(search != "")
            {
                course = course.Where(x => x.Name.Contains(search) || x.Code.Contains(search));

                ViewBag.search = search;
                ViewBag.searchCount = course.Count();
            }

            course = course.OrderByDescending(n => n.CourseID);
            ViewBag.page = page;
            return View(course.ToPagedList(page,10)); 
        }

        [HttpPost]
        public IActionResult AddCourse(Course course)
        {
            if(course.Name != null)
            {
                db.Course.Add(course);
                db.SaveChanges();
            }
            return RedirectToAction("index");
        }
    }
}
