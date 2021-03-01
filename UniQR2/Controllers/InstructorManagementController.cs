using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniQR2.Extensions;
using UniQR2.Models;
using UniQR2.Services;

namespace UniQR2.Controllers
{
    [Authorize("Administrator")]
    public class InstructorManagementController : Controller
    {
        private readonly ModelContext db;
        private readonly IEmailService emailSender;
        private readonly IDataProtector protector;

        public InstructorManagementController(
            ModelContext db,
            IEmailService emailSender,
            IDataProtectionProvider provider
            )
        {
            this.db = db;
            this.emailSender = emailSender;
            this.protector = provider.CreateProtector("UniQR system....!!!");
        }

        public IActionResult Index()
        {
            return View(db.Users.Where(n => n.UserRole == UserRole.Instructor).ToList());
        }


        [HttpPost]
        public async Task<IActionResult> AddUser(string email)
        {
            if (email != null && !db.Users.Any(n => n.Email == email))
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

    }
}
