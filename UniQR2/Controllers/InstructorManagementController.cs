using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniQR2.Extensions;
using UniQR2.Models;
using UniQR2.Services;
using UniQR2.ViewModels;
using X.PagedList;

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

        public IActionResult Index(int page = 1, string search = "")
        {
            var instructors = db.Users.Where(x => x.UserRole == UserRole.Instructor);
            if (search != "")
            {
                instructors = instructors.Where(x => x.Email.Contains(search) || x.FullName.Contains(search));

                ViewBag.search = search;
                ViewBag.searchCount = instructors.Count();
            }


            ViewBag.page = page;
            return View(instructors.ToPagedList(page, 10));
        }


        [HttpPost]
        public async Task<IActionResult> AddUser(string email)
        {
            if (db.Users.Any(x => x.Email == email))
            {
                TempData["message"] = new NotificationViewModel
                {
                    Title = "Error!",
                    Content = "Email address has already been taken",
                    NotificationType = NotificationType.danger
                }.SerializeNotification();
                 
                return RedirectToAction("index", "instructormanagement");
            }
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
            return RedirectToAction("index", "instructormanagement");
        }

        public JsonResult GetUser(int id)
        {
            GetUserViewModel u = db.Users.Select(n => new GetUserViewModel { Email = n.Email, FullName = n.FullName, UserID = n.UserID }).FirstOrDefault(x => x.UserID == id);

            return Json(u);
        }


        [HttpPost]
        public async Task<IActionResult> EditUser(GetUserViewModel getUserViewModel)
        {
            User u = db.Users.FirstOrDefault(x => x.UserID == getUserViewModel.UserID);
            if (u != null)
            {
                u.FullName = getUserViewModel.FullName;
                u.Email = getUserViewModel.Email;

                if (ModelState.IsValid)
                {
                    await db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index", "InstructorManagement");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int UserID)
        {
            User u = db.Users.FirstOrDefault(x => x.UserID == UserID);
            if (u != null)
            {
                db.Users.Remove(u);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "InstructorManagement");
        }



    }
}
