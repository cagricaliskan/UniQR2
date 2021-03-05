using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.Models
{
    public class Classroom
    {

        public int ClassroomID { get; set; }

        public string Name { get; set; }


        public virtual ICollection<Floor> Floors{ get; set; }

        public virtual ICollection<CourseClassroom> CourseClassrooms { get; set; }


    }
}
