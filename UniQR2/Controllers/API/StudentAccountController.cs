using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniQR2.Models;
using UniQR2.Services;
using UniQR2.ViewModels;
using UniQR2.ViewModels.ApiDTO;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using UniQR2.Middlewares.Attributes;
using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace UniQR2.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAccountController : ControllerBase
    {
        private readonly IUserService UserService;
        private readonly ModelContext db;
        private readonly IMapper mapper;

        public StudentAccountController(
            IUserService userService,
            ModelContext db,
            IMapper mapper
            )
        {
            UserService = userService;
            this.db = db;
            this.mapper = mapper;
        }


        [HttpPost("login")]
        public IActionResult Login(AuthenticateRequest userLoginModel)
        {
            AuthenticateResponse response = UserService.Authenticate(userLoginModel);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }


        [HttpPost("register")]
        public IActionResult Register(StudentRegisterModel studentRegisterModel)
        {
            if (ModelState.IsValid)
            {
                if (db.Students.Any(n => n.Number == studentRegisterModel.Number))
                {
                    var user = db.Students.FirstOrDefault(n => n.Number == studentRegisterModel.Number);
                    user.Email = studentRegisterModel.Email;
                    user.Fullname = studentRegisterModel.Fullname;
                    user.Password = studentRegisterModel.Password;
                    return Ok(new { status = "updated" });
                }
                db.Students.Add(mapper.Map<Student>(studentRegisterModel));
                db.SaveChanges();
                return Ok(studentRegisterModel);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("MyCourses")]
        [JWTAuthorize]
        public IActionResult MyCourses()
        {
            Student user = (Student)HttpContext.Items["User"];
            var derslerim = db.CourseClassrooms
                .Where(n => n.CourseStudentRels.Any(x => x.StudentID == user.StudentID))
                .Select(n => new MyClass
                {
                    CourseClassroomID = n.CourseClassroomID,
                    CourseCode = n.Course.Code,
                    CourseName = n.Course.Name,
                    InstractorName = n.Instructor.FullName
                }).ToList();

            return Ok(derslerim);
        }


        [HttpPost("MyProfile")]
        [JWTAuthorize]
        public IActionResult MyProfile()
        {
            Student user = (Student)HttpContext.Items["User"];
            var dersler = db.CourseClassrooms
                .Where(n => n.CourseStudentRels.Any(x => x.StudentID == user.StudentID));



            var announcements = new List<ViewModels.ApiDTO.Announcement>();

            foreach (var item in dersler.Where(n => n.Announcements.Count() > 0).ToList())
            {
                var duyurular = db.Announcements.Where(n => n.CourseClassroomID == item.CourseClassroomID).ToList();
                var dersAd = db.Courses.FirstOrDefault(n => n.CourseID == item.CourseID).Name;
                foreach (var duyuru in duyurular)
                {
                    announcements.Add(new ViewModels.ApiDTO.Announcement()
                    {
                        Header = duyuru.Header,
                        Message = duyuru.Message,
                        CourseName = dersAd
                    });
                }


            }
            return Ok(new
            {
                CourseCount = dersler.Count(),
                UserName = user.Fullname,
                StudentNumber = user.Number,
                Duyurular = announcements
            });
        }


        [HttpGet("CourseDetails/{id}")]
        [JWTAuthorize]
        public IActionResult CourseDetails(int id)
        {
            Student user = (Student)HttpContext.Items["User"];

            var count = db.Participations
                .Where(n =>
                    n.AttendanceList.CourseClassroomID == id &&
                    n.StudentID == user.StudentID)
                .Count();

            var duyurular = db.Announcements.Where(n => n.CourseClassroomID == id).ToList();


            var dersim = db.CourseClassrooms
                .Select(n => new
                {
                    n.CourseClassroomID,
                    FloorNum = n.Classroom.Floor.FloorNum,
                    ClassRoomName = n.Classroom.Name,
                    InstractorName = n.Instructor.FullName,
                    CourseCode = n.Course.Code,
                    CourseName = n.Course.Name,
                    ParticipatedCount = count,
                    Files = n.Files,
                    Announcements = duyurular
                })
                .FirstOrDefault(x => x.CourseClassroomID == id);

            return Ok(dersim);
        }


        [HttpPost("Participate")]
        [JWTAuthorize]
        public IActionResult Participate(Participate participate)
        {
            Student user = (Student)HttpContext.Items["User"];

            var bytes = Convert.FromBase64String(participate.Code);
            var decoded = Encoding.UTF8.GetString(bytes);

            int attId = int.Parse(decoded.Split('-')[0]);

            if (!db.Participations.Any(n => n.AttendanceListID == attId && n.StudentID == user.StudentID))
            {
                db.Participations.Add(new Participation()
                {
                    AttendanceListID = attId,
                    StudentID = user.StudentID,
                    AttendTime = DateTime.Now
                });

                db.SaveChanges();
            } 
            else
            {
                return Ok(new { status = "patricipated"});
            }


            return Ok(new { status = "ok" });
        }


        [HttpGet("LoginDeneme")]
        [JWTAuthorize]
        public IActionResult LoginDeneme()
        {

            return Ok("login olmuş bir student");
        }

    }
}
