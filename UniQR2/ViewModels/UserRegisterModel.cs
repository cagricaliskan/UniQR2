using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.ViewModels
{
    public class UserRegisterModel
    {
        [Required]
        public string FullName { get; set; }


        [Required, EmailAddress]
        public string Email { get; set; }

        [Compare("Email", ErrorMessage = "Emails are not matching")]
        public string ConfirmEmail { get; set; }


        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords are not matching")]
        public string ConfirmPassword { get; set; }

    }
}
