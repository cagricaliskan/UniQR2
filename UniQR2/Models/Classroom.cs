using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.Models
{
    public class Classroom
    {

        public int ClassroomID { get; set; }

        public string Name { get; set; }

        [ForeignKey("FloorID")]
        public int FloorID{ get; set; }
        public virtual Floor Floor{ get; set; }

        public virtual ICollection<CourseClassroom> CourseClassrooms { get; set; }


    }
}
