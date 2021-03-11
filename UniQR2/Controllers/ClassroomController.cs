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
    public class ClassroomController: Controller
    {
        private readonly ModelContext db;

        public ClassroomController(ModelContext db)
        {
            this.db = db;
        }

        public IActionResult Index(int page = 1, string search ="")
        {
            var classroom = db.Classrooms.AsQueryable();
            if( search != "")
            {
                classroom = db.Classrooms.Where(x => x.Name.Contains(search) || x.Floors.Contains(search));

                ViewBag.search = search;
                ViewBag.count = classroom.Count();
            }

            classroom = classroom.OrderByDescending(n => n.ClassroomID);
            ViewBag.page = page;
            
            return View(classroom.ToPagedList(page, 10));
        }

        [HttpPost]
        public IActionResult AddClassroom(Classroom classroom)
        {
            if (classroom.Floors != null)
            {
                db.Classrooms.Add(classroom);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public JsonResult GetClassroom(int id)
        {
            Classroom c = db.Classrooms.Select(x => new Classroom { ClassroomID = x.ClassroomID, Floors = x.Floors, Name = x.Name}).FirstOrDefault(n => n.ClassroomID == id);
            return Json(c);
        }

        [HttpPost]
        public async Task<IActionResult> EditClassroom(Classroom classroom)
        {
            Classroom c = db.Classrooms.FirstOrDefault(x => x.ClassroomID == classroom.ClassroomID);
            if (c != null)
            {
                c.Name = classroom.Name;
                c.Floors = classroom.Floors;
                if (ModelState.IsValid)
                {
                    await db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClassroom(int id)
        {
            Classroom c = db.Classrooms.FirstOrDefault(x => x.ClassroomID == id);
            if(c != null)
            {
                db.Classrooms.Remove(c);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        } 
    }
}
