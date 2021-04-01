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
            var courseclassroom = db.CourseClassrooms.Include(x => x.Instructor ).AsQueryable();
            courseclassroom = db.CourseClassrooms.Include(n => n.Classroom).AsQueryable();
            courseclassroom = db.CourseClassrooms.Include(z => z.Course).AsQueryable();
            if(search != "")
            {
                courseclassroom = db.CourseClassrooms.Where(x => x.Instructor.FullName.Contains(search));

                ViewBag.search = search;
                ViewBag.count = courseclassroom.Count();
            }

            courseclassroom = courseclassroom.OrderByDescending(x => x.CourseClassroomID);
            ViewBag.page = page;


            ViewBag.classroom = db.Classrooms.ToList();
            ViewBag.course = db.Courses.ToList();
            ViewBag.ins = db.Users.Where(x => x.UserRole == UserRole.Instructor).ToList();


            return View(courseclassroom.ToPagedList(page,10));
        }


        [HttpPost]
        public IActionResult AddClass(CourseClassroom classController)
        {
            if (ModelState.IsValid)
            {
                db.CourseClassrooms.Add(classController);
                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        public JsonResult GetClass(int id)
        {
            CourseClassroom c = db.CourseClassrooms.Select(x => new CourseClassroom { ClassroomID = x.ClassroomID, CourseClassroomID = x.CourseClassroomID, InstructorID = x.InstructorID, CourseID = x.CourseID }).FirstOrDefault(n => n.CourseClassroomID == id);
            return Json(c);
        }

        [HttpPost]
        public async Task<IActionResult> EditClass(CourseClassroom classController)
        {
            CourseClassroom c = db.CourseClassrooms.FirstOrDefault(x => x.CourseClassroomID == classController.CourseClassroomID);
            if(c!=null)
            {
                c.CourseID = classController.CourseID;
                c.InstructorID = classController.InstructorID;
                c.ClassroomID = classController.ClassroomID;
                if (ModelState.IsValid)
                {
                    await db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClass(int id)
        {
            CourseClassroom c = db.CourseClassrooms.FirstOrDefault(x => x.CourseClassroomID == id);
            if(c != null)
            {
                db.CourseClassrooms.Remove(c);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
