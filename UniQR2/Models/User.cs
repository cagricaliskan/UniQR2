using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.Models
{
    public enum UserRole
    {
        Administrator, // kurum yöneticisi
        Instructor // öğretment
    }
    public class User
    {

        public int UserID { get; set; }


        [Required]
        public string FullName { get; set; }


        [Required, EmailAddress]
        public string Email { get; set; }


        [Required]
        public string Password { get; set; }



        public string ResetCode { get; set; }




        public bool isActive { get; set; }




        public string activationCode { get; set; }



        public UserRole UserRole { get; set; }





        public DateTime ResetCodeExpire { get; set; } = DateTime.Now.AddHours(1);






        public virtual ICollection<CourseClassroom> CourseClassrooms { get; set; }

    }
}
