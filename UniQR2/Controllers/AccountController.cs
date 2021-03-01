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
            this.protector = provider.CreateProtector(GetType().FullName);
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
        public IActionResult Register(UserRegisterModel userRegisterModel)
        {
            if (ModelState.IsValid)
            {
                User u = db.Users.FirstOrDefault(n => n.Email == userRegisterModel.Email);
                u.FullName = userRegisterModel.FullName;
                u.Password = userRegisterModel.Password;
                u.UserRole = UserRole.Instructor;
                u.activationCode = "activation code"; // activasyon kodu oluşturulacak (protector)
                u.isActive = false;
                db.Entry(u).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.Users.Update(u);

                if(db.SaveChanges() > 0)
                {
                    // activation code kullanıcıya mail ile gönderilecek
                    // tıkladığında yeni bir action ile hesabıa active olacak
                }
            }
            return RedirectToAction("index", "home");
        }


        [HttpPost]
        public async Task<IActionResult> AddUser(string email)
        {
            if (email != null)
            {
                string body = "You have been invited to UniQR system. To register, please follow the" + "<a target=\"_blank\" href=\"" + MyHttpContext.AppBaseUrl + "/Account/Register?email=" + protector.Protect(email) + " \">Tıkla </a>";
                User u = new User
                {
                    Email = email,
                    isActive = false,
                    ResetCodeExpire = DateTime.Now
                };
                db.Users.Add(u);
                await db.SaveChangesAsync();
                await emailSender.Send(email, "UniQR Invite", body);
            }
            return RedirectToAction("index", "home");
        }


        public IActionResult Forbidden()
        {
            return Content("sayfaya erişiminiz kısıtlı");
        }
    }
}
