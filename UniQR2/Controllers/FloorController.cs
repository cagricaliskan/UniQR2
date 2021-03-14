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
    public class FloorController : Controller
    {
        private readonly ModelContext db;

        public FloorController(ModelContext db)
        {
            this.db = db;
        }
        public IActionResult Index(int page = 1, string search ="")
        {
            var floor = db.Floors.AsQueryable();
            if (search != "")
            {
                floor = db.Floors.Where(x => x.FloorNum.Contains(search));

                ViewBag.search = search;
                ViewBag.count = floor.Count();
            }

            floor = floor.OrderByDescending(n => n.FloorID);
            ViewBag.page = page;

            return View(floor.ToPagedList(page,10));
        }

        [HttpPost]
        public IActionResult AddFloor(Floor floor)
        {
            if(floor != null)
            {
                db.Floors.Add(floor);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public JsonResult GetFloor(int id)
        {
            Floor f = db.Floors.Select(x => new Floor { FloorID = x.FloorID, FloorNum = x.FloorNum }).FirstOrDefault(n => n.FloorID == id);
            return Json(f);
        }

        [HttpPost]
        public async Task<IActionResult> EditFloor(Floor floor)
        {
            Floor f = db.Floors.FirstOrDefault(x => x.FloorID == floor.FloorID);
            if(f != null)
            {
                f.FloorNum = floor.FloorNum;

                if (ModelState.IsValid)
                {
                    await db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
