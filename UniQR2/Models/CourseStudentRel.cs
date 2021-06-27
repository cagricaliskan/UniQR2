using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.Models
{
    public class CourseStudentRel
    {

        public int CourseStudentRelID { get; set; }


        public int StudentID { get; set; }
        public virtual Student Student { get; set; }


        [ForeignKey("CourseClassroom")]
        public int CourseClassroomID { get; set; }
        public virtual CourseClassroom CourseClassroom { get; set; }

    }
}
