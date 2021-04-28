using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniQR2.Models;
using UniQR2.ViewModels.ApiDTO;

namespace UniQR2.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        Student GetById(int id);
    }
}
