using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.Models
{
    public partial class Course
    {
        public int CourseID { get; set; }


        [Required]
        public string Code { get; set; }


        [Required]
        public string Name { get; set; }



        public virtual ICollection<CourseClassroom> CourseClassrooms { get; set; }

    }
    public partial class Course
    {
        [NotMapped]
        public string EncryptedID { get; set; }
    }
}
