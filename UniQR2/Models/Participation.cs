using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.Models
{
    public class Participation
    {

        public int ParticipationID { get; set; }


        public DateTime AttendTime { get; set; }

        public int StudentID { get; set; }
        public virtual Student Student { get; set; }


        public int AttendanceListID { get; set; }
        public virtual AttendanceList AttendanceList { get; set; }


    }
}
