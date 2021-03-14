using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.Models
{
    public class Floor
    {
        public int FloorID { get; set; }
        public string FloorNum { get; set; }

        public virtual ICollection<Classroom> Classrooms { get; set; }

    }
}
