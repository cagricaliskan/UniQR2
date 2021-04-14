using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment webHostEnvironment;


        public MyClassesController(ModelContext db, IWebHostEnvironment webHostEnvironment)
        {
            this.db = db;
            this.webHostEnvironment = webHostEnvironment;

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

            return View(myclass.ToPagedList(page, 10));
        }

        public IActionResult Files(int courseId)
        {
            ViewBag.courseId = courseId;
            return View();
        }

        [HttpPost]
        public IActionResult Files(IFormFile file)
        {

            string contentPath = webHostEnvironment.ContentRootPath;

            if (file != null)
            {
                //gettin file name
                var fileName = Path.GetFileName(file.FileName);

                //gettin file type
                var fileType = Path.GetExtension(fileName);

                // dosya ismi ile uzantı birliştirme
                var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileType);

                var dataPath = Path.Combine(contentPath, "UploadedFiles/", newFileName);

                using (var stream = System.IO.File.Create(dataPath))
                {
                    file.CopyTo(stream);
                }




                var objfile = new Models.File()
                {
                    FileName = newFileName,
                    FileType = fileType,
                    DataPath = dataPath


                };

                db.Files.Add(objfile);
                db.SaveChanges();

                TempData["result"] = "Uploaded successfully";

            }
            return View();
        }

        public IActionResult Attendance(int? courseId, int page = 1, string search = "")
        {
            if(courseId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var attendance = db.AttendanceLists.Where(n => n.CourseClassroomID == courseId).AsQueryable();

            if (search != "")
            {
                attendance= db.AttendanceLists.Where(x => x.Name.Contains(search));

                ViewBag.search = search;
                ViewBag.count = attendance.Count();
            }

            attendance = attendance.OrderBy(n => n.AttendanceListID);
            ViewBag.page = page;
            ViewBag.courseId = courseId;

            return View(attendance.ToPagedList(page, 20));
        }


        [HttpPost]
        public IActionResult Attendance(AttendanceList attendance)
        {


            if (attendance != null)
            {
                if (attendance.Repeat == true)
                {
                    var week1 = new AttendanceList
                    {
                        Name = "1. Hafta",
                        StartDate = attendance.StartDate,
                        EndDate = attendance.EndDate,
                        CourseClassroomID = attendance.CourseClassroomID
                    };

                    db.AttendanceLists.Add(week1);
                    db.SaveChanges();

                    for (int i = 2; i <= 14; i++)
                    {
                        ViewBag.name = i.ToString();
                        DateTime nextweek = attendance.StartDate.AddDays(7 *(i-1));
                        

                        var entry = new AttendanceList
                        {
                            Name = ViewBag.name + ". Hafta",
                            StartDate = nextweek,
                            EndDate = attendance.EndDate,
                            CourseClassroomID = attendance.CourseClassroomID
                        };

                        db.AttendanceLists.Add(entry);
                        db.SaveChanges();
                    }
                }
                else
                {
                    db.AttendanceLists.Add(attendance);
                    db.SaveChanges();
                }

            }
            return RedirectToAction("Attendance", new { courseId = attendance.CourseClassroomID});
        }

    }
}
