using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
            //ViewBag.x = User.Claims.Where(p => p.Type == System.Security.Claims.ClaimTypes.Name);

            var myclass = db.CourseClassrooms.AsQueryable();

            if (search != "")
            {
                myclass = db.CourseClassrooms.Where(x => x.Course.Code.Contains(search) || x.Classroom.Name.Contains(search));

                ViewBag.search = search;
                ViewBag.count = myclass.Count();
            }

            myclass = myclass.OrderByDescending(n => n.InstructorID).Where(x => x.Instructor.FullName == User.Identity.Name);
            ViewBag.page = page;

            ViewBag.classroom = db.Classrooms.ToList();
            ViewBag.course = db.Courses.ToList();

            return View(myclass.ToPagedList(page, 10));
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

            return View(attendance.ToPagedList(page, 60));
        }


        [HttpPost]
        public IActionResult Attendance(AttendanceList attendance, DateTime StartTime)
        {

            string SDate = attendance.StartDate.ToShortDateString();
            string SHour = StartTime.ToShortTimeString();
            string EHour = attendance.EndDate.ToShortTimeString();

            // time ve date i birleştirildiği değişken
            DateTime newStart = Convert.ToDateTime(SDate + " " + SHour);
            DateTime newEnd = Convert.ToDateTime(SDate + " " + EHour);
            
            
            

            if (attendance != null)
            {

                //var week1 = new AttendanceList
                //{
                //    Name = "1. Hafta",
                //    StartDate = newStart,
                //    EndDate = newEnd,
                //    CourseClassroomID = attendance.CourseClassroomID

                //};


                //db.AttendanceLists.Add(week1);
                //db.SaveChanges();
                //var qr1 = week1.AttendanceListID.ToString();

                for (int i = 1; i <= 14; i++)
                {
                    ViewBag.name = i.ToString();
                    DateTime nextweek = attendance.StartDate.AddDays(7 * (i - 1));
                    string nextSDate = nextweek.ToShortDateString();

                    DateTime nextNewStart = Convert.ToDateTime(nextSDate + " " + SHour);
                    DateTime nextNewEnd = Convert.ToDateTime(nextSDate + " " + EHour);


                    var entry = new AttendanceList
                    {
                        Name = ViewBag.name + ". Hafta",
                        StartDate = nextNewStart,
                        EndDate = nextNewEnd,
                        CourseClassroomID = attendance.CourseClassroomID
                        //StartHour = attendance.StartHour,
                        //EndHour = attendance.EndHour

                    };

                    db.AttendanceLists.Add(entry);
                    db.SaveChanges();
                    
                    var qr = entry.AttendanceListID.ToString() + "-" + entry.CourseClassroomID.ToString() + "-" + entry.StartDate.ToShortDateString();

                    byte[] bytes = Encoding.UTF8.GetBytes(qr);
                    var encodedqr = Convert.ToBase64String(bytes);

                    AttendanceList a = db.AttendanceLists.FirstOrDefault(x => x.AttendanceListID == entry.AttendanceListID);

                    if(qr != null && a!=null)
                    {
                        entry.QrString = encodedqr;

                        db.SaveChanges();
                    }
                    
                    
                    

                }

            }
            return RedirectToAction("Attendance", new { courseId = attendance.CourseClassroomID });
        }

        public JsonResult GetAttendance(int id)
        {
            AttendanceList a = db.AttendanceLists.Select(x => new AttendanceList {  StartDate= x.StartDate, EndDate = x.EndDate, /*StartHour= x.StartHour, EndHour = x.EndHour,*/ AttendanceListID = x.AttendanceListID }).FirstOrDefault(n => n.AttendanceListID == id);
            return Json(a);
        }

        public IActionResult EditAttendance(AttendanceList a)
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetQr(AttendanceList a)
        {
            AttendanceList ann = db.AttendanceLists.FirstOrDefault(x => x.AttendanceListID == a.AttendanceListID);

            if(ann != null)
            {
                QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(ann.QrString, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                Bitmap qrCodeImage = qrCode.GetGraphic(15);

                
                return View(BitmapToBytes(qrCodeImage));
            }

            ViewBag.x = ann.AttendanceListID;
            
            return RedirectToAction("Index", "MyClasses");
        }

        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
               
                return stream.ToArray();
            }
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

        public JsonResult GetAnnouncement(int id)
        {
            Announcement ann = db.Announcements.Select(x => new Announcement { AnnouncementID = x.AnnouncementID, Header = x.Header, Message = x.Message }).FirstOrDefault(n => n.AnnouncementID == id);
            return Json(ann);
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
