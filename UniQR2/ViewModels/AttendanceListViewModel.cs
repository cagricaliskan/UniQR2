using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniQR2.Models;

namespace UniQR2.ViewModels
{
    public class AttendanceListViewModel
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StartHour { get; set; }
        public DateTime EndHour { get; set; }
        public int CourseClassroomID { get; set; }
    }
}
