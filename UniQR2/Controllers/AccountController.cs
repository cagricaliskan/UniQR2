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
using UniQR2.ViewString;

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
            if (u == null)
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

                if (await db.SaveChangesAsync() > 0)
                {
                    string str = await ViewToStringRenderer.RenderViewToStringAsync(HttpContext.RequestServices, $"~/Views/Emails/NoLinkEMailTemplate.cshtml", $"Hi {u.FullName}, your account is created.");
                    await emailSender.Send(u.Email, "UniQR Account", str);
                }
               
            }
            return RedirectToAction("login", "account");
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

                string str = await ViewToStringRenderer.RenderViewToStringAsync(HttpContext.RequestServices, $"~/Views/Emails/EMailTemplate.cshtml", new EmailViewModel { Text = "You have sumbitted a request for resetting your password. If you have, click", Link = MyHttpContext.AppBaseUrl + "/Account/reset?reset=" + protector.Protect(email) } );
                await emailSender.Send(u.Email, "Password Reset Request", str);
                return RedirectToAction("login", "account");
            }
            else
            {
                ViewBag.Message = "No users was found with the given e-mail";
                return View("ResetPassword");
            }

        }


        public IActionResult Reset(string reset)
        {


            reset = protector.Unprotect(reset);
            User u = db.Users.FirstOrDefault(x => x.Email == reset);


            ResetPasswordViewModel resetPasswordViewModel = mapper.Map<User, ResetPasswordViewModel>(u);


            return View(resetPasswordViewModel);
        }

        [HttpPost]
        public IActionResult Reset(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                User u = db.Users.FirstOrDefault(x => x.ResetCode == resetPasswordViewModel.ResetCode);

                if (u != null)
                {
                    u.Password = resetPasswordViewModel.Password;
                    db.Users.Update(u);
                    db.SaveChanges();
                }
                return RedirectToAction("login", "account");
            }
            
            return View(resetPasswordViewModel);
        }

        public IActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Test(string email, User User)
        {
            User u = db.Users.FirstOrDefault(x => x.Email == email);
            string str = await ViewToStringRenderer.RenderViewToStringAsync(HttpContext.RequestServices, $"~/Views/Emails/EmailTemplate.cshtml", new { });
            if (u != null && db.Users.Any(x => x.Email == email))
            {
                await emailSender.Send(u.Email, "Test", str);

            }
            return View();
        }



        public IActionResult Forbidden()
        {
            return Content("sayfaya erişiminiz kısıtlı");
        }
    }
}
