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
using AutoMapper;

namespace UniQR2.Controllers
{
    public class AccountController : Controller
    {
        private readonly ModelContext db;
        private readonly IEmailService emailSender;
        private readonly IDataProtector protector;
        private readonly IMapper mapper;

        public AccountController(
            ModelContext db,
            IEmailService emailSender,
            IDataProtectionProvider provider,
            IMapper mapper
            )
        {
            this.db = db;
            this.emailSender = emailSender;
            this.protector = provider.CreateProtector("UniQR system....!!!");
            this.mapper = mapper;

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
            User u = db.Users.FirstOrDefault(x => x.Email == userLoginModel.Email && x.Password == userLoginModel.Password && x.IsActive == true);
            if (u != null)
            {
                string role = u.UserRole == UserRole.Administrator ? "Administrator" : "Instructor";
                ClaimsIdentity identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, u.Email),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.Name, u.FullName)
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
            email = protector.Unprotect(email);
            User u = db.Users.FirstOrDefault(n => n.Email == email);
            ModelState.Clear();
            if(u == null)
            {
                return RedirectToAction("Index");
            }

            UserRegisterModel userRegisterModel = mapper.Map<User, UserRegisterModel>(u);

            return View(userRegisterModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterModel userRegisterModel)
        {
            if (ModelState.IsValid)
            {
                User u = db.Users.FirstOrDefault(x => x.Email == userRegisterModel.Email);
                u.FullName = userRegisterModel.FullName;
                u.Password = userRegisterModel.Password;
                u.UserRole = UserRole.Instructor;
                u.IsActive = true;
                db.Entry(u).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.Users.Update(u);

                if(await db.SaveChangesAsync() > 0)
                {
                    string body = $"Hi {u.FullName}, your account is created.";
                    await emailSender.Send(u.Email, "UniQR Account", body);
                }
            }
            return RedirectToAction("index", "home");
        }

        public IActionResult ResetPassword()
        {
          return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordMail(string email, User user)
        {
            User u = db.Users.FirstOrDefault(x => x.Email == email);
            if (u != null && db.Users.Any(x => x.Email == user.Email))
            {
                u.ResetCode = protector.Protect(email);
                db.SaveChanges();
                string body = "You have sumbitted a request for resetting your password. If you have, click" + "<a href=\"" + MyHttpContext.AppBaseUrl + "/Account/reset?reset=" + u.ResetCode + " \" > here: </a>";
                await emailSender.Send(u.Email, "Password Reset Request", body);
                return RedirectToAction("login", "account");
            } else
            {
                ViewBag.Message = "No users was found with the given e-mail";
                return View("ResetPassword");
            }
            
        }
        

        public IActionResult Reset()
        {
            return View();
        }

         [HttpPost]
        public IActionResult Reset(ResetPasswordViewModel resetPassword)
        {
            string email = protector.Unprotect(resetPassword.ResetCode);
            User u = db.Users.FirstOrDefault(x => x.ResetCode == resetPassword.ResetCode);
            if(u != null)
            {
                u.Password = resetPassword.Password;
                db.Users.Update(u);
            }

            return RedirectToAction("login", "account");

        }

        



        public IActionResult Forbidden()
        {
            return Content("sayfaya erişiminiz kısıtlı");
        }
    }
}
