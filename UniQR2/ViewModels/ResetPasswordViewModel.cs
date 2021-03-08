using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string ResetCode { get; set; }


        [Required]
        public string Password { get; set; }


        [Compare("Password", ErrorMessage = "Passwords are not matching")]
        public string ConfirmPassword { get; set; }
        // resetCode, pass, repearPass


        public bool isActive { get; set; }
    }
}
