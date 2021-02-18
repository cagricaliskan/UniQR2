using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.Models
{
    public class CourseClassroom
    {

        public int CourseClassroomID { get; set; }


        public int ClassroomID { get; set; }
        public virtual Classroom Classroom { get; set; }


        public int CourseID { get; set; }
        public virtual Course Course { get; set; }


        public virtual ICollection<CourseStudentRel> CourseStudentRels { get; set; }
        public virtual ICollection<AttendanceList> AttendanceLists { get; set; }
    }
}
