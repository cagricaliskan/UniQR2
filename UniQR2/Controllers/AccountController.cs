﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniQR2.Models;
using UniQR2.Services;

namespace UniQR2.Controllers
{
    public class AccountController : Controller
    {
        private readonly ModelContext db;
        private readonly IEmailService emailSender;

        public AccountController(ModelContext db, IEmailService emailSender)
        {
            this.db = db;
            this.emailSender = emailSender;

        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User userLoginModel)
        {
            User u = db.Users.FirstOrDefault(x => x.Email == userLoginModel.Email && x.Password == userLoginModel.Password);
            if (u != null)
            {

                string body = "You have logged in";
                await emailSender.Send(userLoginModel.Email,"Giriş İşlemi",body);
                return RedirectToAction("privacy", "Home");
            }
            return RedirectToAction("register", "account");
        }

        public IActionResult Register(string? email)
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
            if(email != null)
            {
                string body = "You have been invited to UniQR system. To register, please follow the" + "<a href=\"" + "https://localhost:44305" + "/Account/Register?email=" + email + " \">Tıkla </a>";
                await emailSender.Send(email, "UniQR Invite", body);
                
            }
            return RedirectToAction("index", "home");
        }
    }
}