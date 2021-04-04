﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniQR2.Models;
using X.PagedList;

namespace UniQR2.Controllers
{
    [Authorize("Instructor")]
    public class MyClassesController : Controller
    {
        private readonly ModelContext db;

        public MyClassesController(ModelContext db)
        {
            this.db = db;
        }

        public IActionResult Index(int page = 1, string search = "")
        {
            var myclass = db.CourseClassrooms.Include(x => x.Classroom).AsQueryable();
            myclass = db.CourseClassrooms.Include(n => n.Course).AsQueryable();
            
            if(search != "")
            {
                myclass = db.CourseClassrooms.Where(x => x.Course.Code.Contains(search) || x.Classroom.Name.Contains(search));

                ViewBag.search = search;
                ViewBag.count = myclass.Count();
            }

            myclass = myclass.OrderByDescending(n => n.InstructorID);
            ViewBag.page = page;

            return View(myclass.ToPagedList(page,10));
        }
    }
}
