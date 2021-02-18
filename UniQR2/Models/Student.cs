using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.Models
{
    public class Student
    {

        public int StudentID { get; set; }

        public string Fullname { get; set; }

        public string Number { get; set; }


        public virtual ICollection<CourseStudentRel> CourseStudentRels { get; set; }

        public virtual ICollection<Participation> Participations { get; set; }

    }
}
