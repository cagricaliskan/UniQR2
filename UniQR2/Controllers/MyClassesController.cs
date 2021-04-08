using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniQR2.Models;
using X.PagedList;

namespace UniQR2.Controllers
{
    [Authorize("Instructor")]
    public class MyClassesController : Controller
    {
        private readonly ModelContext db;
        
        

        public MyClassesController(ModelContext db)
        {
            this.db = db;
            
        }


        public IActionResult Index(int page = 1, string search = "")
        {

            var myclass = db.CourseClassrooms.AsQueryable();
            
            if (search != "")
            {
                myclass = db.CourseClassrooms.Where(x => x.Course.Code.Contains(search) || x.Classroom.Name.Contains(search));

                ViewBag.search = search;
                ViewBag.count = myclass.Count();
            }

            myclass = myclass.OrderByDescending(n => n.InstructorID);
            ViewBag.page = page;

            ViewBag.classroom = db.Classrooms.ToList();
            ViewBag.course = db.Courses.ToList();

            return View(myclass.ToPagedList(page,10));
        }

        public IActionResult Files()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Files(IFormFile file)
        {
            if(file != null || file.Length > 0)
            {
                //gettin file name
                var fileName = Path.GetFileName(file.FileName);

                //gettin file type
                var fileType = Path.GetExtension(fileName);

                // dosya ismi ile uzantı birliştirme
                var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileType);


                var objfile = new Files()
                {
                    FileName = newFileName,
                    FileType = fileType
                };

                using (var target = new MemoryStream())
                {
                    file.CopyTo(target);
                   
                }

                db.Files.Add(objfile);
                db.SaveChanges();

                TempData["result"] = "Uploaded successfully";

            }
            return View();
        }
        
    }
}
