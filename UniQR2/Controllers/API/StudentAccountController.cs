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
                if(db.Students.Any(n => n.Email == studentRegisterModel.Email || n.Number == studentRegisterModel.Number))
                {
                    ModelState.AddModelError("unique", "email or student number is not unique");
                    return BadRequest(ModelState);
                }
                db.Students.Add(mapper.Map<Student>(studentRegisterModel));
                db.SaveChanges();
                return Ok(studentRegisterModel);
            }

            return BadRequest(ModelState);
        }



        [HttpGet("LoginDeneme")]
        [JWTAuthorize]
        public IActionResult LoginDeneme()
        {

            return Ok("login olmuş bir student");
        }

    }
}
