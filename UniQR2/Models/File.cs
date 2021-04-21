using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.Models
{
    public class File
    {

        [Key]
        public int FileID { get; set; }

        
        public string FileName { get; set; }
        
        
        public string FileType { get; set; }

        public string DataPath { get; set; }


        public int CourseClassroomID { get; set; }
        public virtual CourseClassroom CourseClassroom { get; set; }

    }
}
