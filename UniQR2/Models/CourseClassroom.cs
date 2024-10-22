﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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


        [ForeignKey("Instructor")]
        public int InstructorID { get; set; }
        public virtual User Instructor { get; set; }

        public virtual ICollection<CourseStudentRel> CourseStudentRels { get; set; }
        public virtual ICollection<AttendanceList> AttendanceLists { get; set; }
        public virtual ICollection<File> Files{ get; set; }
        public virtual ICollection<Announcement> Announcements { get; set; }

    }
}
