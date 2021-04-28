using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniQR2.Models;

namespace UniQR2.ViewModels.ApiDTO
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string StudentNumber { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(Student user, string token)
        {
            Id = user.StudentID;
            Fullname = user.Fullname;
            StudentNumber = user.Number;
            Token = token;
        }
    }
}
