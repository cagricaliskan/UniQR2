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
using UniQR2.ViewModels;
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

        public JsonResult GetAnnouncement(int id)
        {
            Announcement ann = db.Announcements.Select(x => new Announcement { AnnouncementID = x.AnnouncementID, Header = x.Header, Message = x.Message }).FirstOrDefault(n => n.AnnouncementID == id);
            return Json(ann);
        }

        public IActionResult Files(int? courseId)
        {
            if (courseId == null)
                return RedirectToAction("Index");
            ViewBag.courseId = courseId;
            return View(db.Files.Where(n => n.CourseClassroomID == courseId).ToList());
        }

        [HttpPost]
        public IActionResult Files(IFormFile file, Models.File f)
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
                    FileName = fileName,
                    FileType = fileType,
                    DataPath = newFileName,
                    CourseClassroomID = f.CourseClassroomID
                };

                db.Files.Add(objfile);
                db.SaveChanges();

                TempData["message"] = new NotificationViewModel
                {
                    Title = "Success!",
                    Content = "File is uploaded",
                    NotificationType = NotificationType.success
                }.SerializeNotification();

            }
            return RedirectToAction("Files", new { courseId = f.CourseClassroomID });
        }

        public IActionResult Attendance(int? courseId, int page = 1, string search = "")
        {
            if (courseId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var attendance = db.AttendanceLists.Where(n => n.CourseClassroomID == courseId).AsQueryable();

            if (search != "")
            {
                attendance = db.AttendanceLists.Where(x => x.Name.Contains(search));

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

                var week1 = new AttendanceList
                {
                    Name = "1. Hafta",
                    StartDate = attendance.StartDate,
                    EndDate = attendance.EndDate,
                    CourseClassroomID = attendance.CourseClassroomID,
                    StartHour = attendance.StartHour,
                    EndHour = attendance.EndHour
                };

                db.AttendanceLists.Add(week1);
                db.SaveChanges();

                for (int i = 2; i <= 14; i++)
                {
                    ViewBag.name = i.ToString();
                    DateTime nextweek = attendance.StartDate.AddDays(7 * (i - 1));


                    var entry = new AttendanceList
                    {
                        Name = ViewBag.name + ". Hafta",
                        StartDate = nextweek,
                        EndDate = attendance.EndDate,
                        CourseClassroomID = attendance.CourseClassroomID,
                        StartHour = attendance.StartHour,
                        EndHour = attendance.EndHour

                    };

                    db.AttendanceLists.Add(entry);
                    db.SaveChanges();
                }

            }
            return RedirectToAction("Attendance", new { courseId = attendance.CourseClassroomID });
        }

        public IActionResult Announcement(int? courseId, int page = 1, string search = "")
        {
            if (courseId == null)
            {
                return RedirectToAction("Index", "MyClasses");
            }

            var announcement = db.Announcements.Where(n => n.CourseClassroomID == courseId).AsQueryable();

            if (search != "")
            {
                announcement = db.Announcements.Where(x => x.Header.Contains(search) || x.Message.Contains(search));

                ViewBag.search = search;
                ViewBag.count = announcement.Count();
            }
            announcement = announcement.OrderByDescending(n => n.AnnouncementID);
            ViewBag.page = page;
            ViewBag.courseId = courseId;
            return View(announcement.ToPagedList(page, 20));
        }


        [HttpPost]
        public IActionResult Announcement(Announcement ann)
        {

            if (ann != null)
            {
                db.Announcements.Add(ann);
                db.SaveChanges();
            }
            return View("Index", "MyClasses");
        }

        [HttpPost]
        public IActionResult EditAnnouncement(Announcement announcement)
        {
            var ann = db.Announcements.FirstOrDefault(x => x.AnnouncementID == announcement.AnnouncementID);

            if (ann != null)
            {
                ann.Header = announcement.Header;
                ann.Message = announcement.Message;

                if (ModelState.IsValid)
                {
                    db.SaveChanges();
                }
            }

            return View("Announcement", "MyClasses");
        }


        [HttpPost]
        public IActionResult DeleteAnnouncement(int? id)
        {
            var ann = db.Announcements.FirstOrDefault(x => x.AnnouncementID == id);

            if (ann != null)
            {
                db.Remove(ann);
                db.SaveChanges();
            }
            return View("Announcement", "MyClasses");
        }

    }
}
