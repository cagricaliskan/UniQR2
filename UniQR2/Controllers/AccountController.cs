using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniQR2.Models;
using UniQR2.Services;
using UniQR2.ViewModels;
using UniQR2.Extensions;

namespace UniQR2.Controllers
{
    public class AccountController : Controller
    {
        private readonly ModelContext db;
        private readonly IEmailService emailSender;
        private readonly IDataProtector protector;

        public AccountController(ModelContext db, IEmailService emailSender, IDataProtector protector)
        {
            this.db = db;
            this.emailSender = emailSender;
            this.protector = protector;

        }
        public IActionResult Login(int error = 0)
        {
            if (error == 1)
            {
                ViewBag.error = "Giriş başarısız";
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginModel userLoginModel)
        {
            User u = db.Users.FirstOrDefault(x => x.Email == userLoginModel.Email && x.Password == userLoginModel.Password);
            if (u != null)
            {
                string role = u.UserRole == UserRole.Administrator ? "Administrator" : "Instructor";
                ClaimsIdentity identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, u.Email),
                    new Claim(ClaimTypes.Role, role)
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Account", new { error = 1 });
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult Register(string email)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User userRegisterModel)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(userRegisterModel);
                db.SaveChanges();
            }
            return RedirectToAction("index", "home");
        }

        public IActionResult AddUser()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddUser(string email)
        {
            if (email != null)
            {
                string body = "You have been invited to UniQR system. To register, please follow the" + "<a href=\"" + MyHttpContext.AppBaseUrl + "/Account/Register?email=" + email + " \">Tıkla </a>";
                await emailSender.Send(email, "UniQR Invite", body);

            }
            return RedirectToAction("index", "home");
        }
    }
}
