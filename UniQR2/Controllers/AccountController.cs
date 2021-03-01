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
            User u = db.Users.FirstOrDefault(x => x.Email == userLoginModel.Email && x.Password == userLoginModel.Password && x.isActive == true);
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
        public async Task<IActionResult> Register(UserRegisterModel userRegisterModel)
        {
            if (ModelState.IsValid)
            {
                User u = db.Users.FirstOrDefault(n => n.Email == userRegisterModel.Email);
                u.FullName = userRegisterModel.FullName;
                u.Password = userRegisterModel.Password;
                u.UserRole = UserRole.Instructor;
                u.activationCode = protector.Protect(u.Email + u.FullName); // activasyon kodu oluşturulacak (protector)
                u.isActive = false;
                db.Entry(u).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.Users.Update(u);

                if(db.SaveChanges() > 0)
                {
                    // activation code kullanıcıya mail ile gönderilecek
                    string body = $"Hi {u.FullName}, <a href='{MyHttpContext.AppBaseUrl}/Account/Activate?code={u.activationCode}'>Click here</a> to complate your registration.";
                    await emailSender.Send(u.Email, "UniQR Account Activation", body);
                    // tıkladığında yeni bir action ile hesabıa active olacak
                }
            }
            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Activate(string code)
        {
            var user = db.Users.FirstOrDefault(n => n.activationCode == code);
            if(user != null)
            {
                user.activationCode = "";
                user.isActive = true;
                db.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                if(await db.SaveChangesAsync() > 0)
                {
                    // active edildi maili gönder, sistemi kullanmaya başlayabilirsiniz şeklinde
                }
                return View(); // mesaj gösterildikten 3 saniye sonra login e atıyor
            }
            return RedirectToAction("Login"); // activation kod hatalı
        }




        public IActionResult Forbidden()
        {
            return Content("sayfaya erişiminiz kısıtlı");
        }
    }
}
