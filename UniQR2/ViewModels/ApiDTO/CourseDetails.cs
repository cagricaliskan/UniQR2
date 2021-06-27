using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.ViewModels.ApiDTO
{
    public class CourseDetails
    {
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public int StudentCount { get; set; }
        public int AbsentCount { get; set; }
        public List<Announcement> Announcements { get; set; }
        public List<Files> Files { get; set; }
    }
}
