using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.Models
{
    public class CourseStudentRel
    {

        public int CourseStudentRelID { get; set; }


        public int StudentID { get; set; }
        public virtual Student Student { get; set; }


        public int ClassroomID { get; set; }
        public virtual CourseClassroom CourseClassroom { get; set; }

    }
}
