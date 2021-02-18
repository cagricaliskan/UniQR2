using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.Models
{
    public class AttendanceList
    {

        public int AttendanceListID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


        public int CourseClassroomID { get; set; }
        public virtual CourseClassroom CourseClassroom { get; set; }


        public virtual ICollection<Participation> Participations { get; set; }
    }
}
