using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class ClassroomController: Controller
    {
        private readonly ModelContext db;

        public ClassroomController(ModelContext db)
        {
            this.db = db;
        }

        public IActionResult Index(int page = 1, string search ="")
        {
            var classroom = db.Classrooms.Include(n => n.Floor).AsQueryable();
            if( search != "")
            {
                classroom = db.Classrooms.Where(x => x.Name.Contains(search) || x.Floor.FloorNum.Contains(search));

                ViewBag.search = search;
                ViewBag.count = classroom.Count();
            }

            classroom = classroom.OrderByDescending(n => n.ClassroomID);
            ViewBag.page = page;

            ViewBag.f = db.Floors.ToList();

            return View(classroom.ToPagedList(page, 10));
        }

        [HttpPost]
        public IActionResult AddClassroom(Classroom classroom)
        {
            if (ModelState.IsValid)
            {
                
                db.Classrooms.Add(classroom);
                db.SaveChanges();
            } else
            {
                ViewBag.Message = "Choose a valid floor";
                ViewBag.Status = "danger";

                return View();
            }
            return RedirectToAction("Index");
        }

        public JsonResult GetClassroom(int id)
        {
            Classroom c = db.Classrooms.Select(x => new Classroom { ClassroomID = x.ClassroomID, FloorID = x.FloorID, Name = x.Name}).FirstOrDefault(n => n.ClassroomID == id);
            return Json(c);
        }

        [HttpPost]
        public async Task<IActionResult> EditClassroom(Classroom classroom)
        {
            Classroom c = db.Classrooms.FirstOrDefault(x => x.ClassroomID == classroom.ClassroomID);
            if (c != null && db.Floors.Any(x => x.FloorNum == c.Floor.FloorNum))
            {
                c.Name = classroom.Name;
                c.FloorID = classroom.FloorID;
                if (ModelState.IsValid)
                {
                    await db.SaveChangesAsync();
                } else
                {
                    ViewBag.Message = "Choose a valid floor";
                    ViewBag.Status = "danger";
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
