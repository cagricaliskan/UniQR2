using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            var myclass = db.CourseClassrooms.AsQueryable();
            
            if (search != "")
            {
                myclass = db.CourseClassrooms.Where(x => x.Course.Code.Contains(search) || x.Classroom.Name.Contains(search));

                ViewBag.search = search;
                ViewBag.count = myclass.Count();
            }

            myclass = myclass.OrderByDescending(n => n.InstructorID);
            ViewBag.page = page;

            ViewBag.classroom = db.Classrooms.ToList();
            ViewBag.course = db.Courses.ToList();

            return View(myclass.ToPagedList(page,10));
        }

        public JsonResult GetClass(int id)
        {
            CourseClassroom c = db.CourseClassrooms.Select(x => new CourseClassroom { ClassroomID = x.ClassroomID, CourseClassroomID = x.CourseClassroomID, InstructorID = x.InstructorID, CourseID = x.CourseID }).FirstOrDefault(n => n.InstructorID == id);
            return Json(c);
        }

        public async Task<IActionResult> EditClass (CourseClassroom classController)
        {
            CourseClassroom c = db.CourseClassrooms.FirstOrDefault(x => x.CourseClassroomID == classController.CourseClassroomID);
            if (c != null)
            {
                c.CourseID = classController.CourseID;
                c.ClassroomID = classController.ClassroomID;
                if (ModelState.IsValid)
                {
                    await db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
