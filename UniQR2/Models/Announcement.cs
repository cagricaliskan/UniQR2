using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.Models
{
    public class Announcement
    {
        public int AnnouncementID { get; set; }
        public string Header { get; set; }
        public string Message { get; set; }

        public int CourseClassroomID { get; set; }
        public virtual CourseClassroom CourseClassroom { get; set; }
    }
}
