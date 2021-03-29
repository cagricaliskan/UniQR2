using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var course = db.Courses.AsQueryable();
            if(search != "")
            {
                course = course.Where(x => x.Name.Contains(search) || x.Code.Contains(search));

                ViewBag.search = search;
                ViewBag.searchCount = course.Count();
            }

            course = course.OrderByDescending(n => n.CourseID);
            ViewBag.page = page;

            ViewBag.floors = db.Floors.ToList();

            return View(course.ToPagedList(page,10)); 
        }

        [HttpPost]
        public IActionResult AddCourse(Course course)
        {
            if(course.Name != null)
            {
                db.Courses.Add(course);
                db.SaveChanges();
            }
            return RedirectToAction("index");
        }

        public JsonResult GetCourse(int id)
        {
            Course c = db.Courses.Select(x => new Course { CourseID = x.CourseID, Code = x.Code, Name = x.Name }).FirstOrDefault(n => n.CourseID == id);
            return Json(c);
        }

        [HttpPost]
        public async Task<IActionResult> EditCourse(Course course)
        {
            Course c = db.Courses.FirstOrDefault(x => x.CourseID == course.CourseID);
            if(c != null)
            {
                c.Name = course.Name;
                c.Code = course.Code;

                if (ModelState.IsValid)
                {
                    await db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index", "Course");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            Course c = db.Courses.FirstOrDefault(x => x.CourseID == id);
            if (c != null)
            {
                db.Courses.Remove(c);
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Course");
        }
        
    }
}
