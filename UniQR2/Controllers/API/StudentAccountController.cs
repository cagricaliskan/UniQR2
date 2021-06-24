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
                .Select(n => new MyClass {
                CourseClassroomID = n.CourseClassroomID,
                CourseCode = n.Course.Code,
                CourseName = n.Course.Name,
                InstractorName = n.Instructor.FullName
            }).ToList();


            return Ok(derslerim);
        }



        [HttpGet("LoginDeneme")]
        [JWTAuthorize]
        public IActionResult LoginDeneme()
        {

            return Ok("login olmuş bir student");
        }

    }
}
