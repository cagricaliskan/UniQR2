using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UniQR2.Models;
using UniQR2.Models.Configurations;
using UniQR2.ViewModels.ApiDTO;

namespace UniQR2.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly ModelContext db;

        public UserService(
            IOptions<AppSettings> appSettings,
            ModelContext db
            )
        {
            _appSettings = appSettings.Value;
            this.db = db;
        }


        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = db.Students.SingleOrDefault(x => x.Number == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public Student GetById(int id)
        {
            return db.Students.FirstOrDefault(x => x.StudentID == id);
        }

        // helper methods

        private string generateJwtToken(Student user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.StudentID.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
