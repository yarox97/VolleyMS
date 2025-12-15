using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VolleyMS.Core.Models;

namespace VolleyMS.BusinessLogic.Features.Users.Authorisation
{
    public class JwtService(IOptions<AuthConfiguration> options)
    {
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
                new Claim(ClaimTypes.Name, user.UserName),                  
                new Claim(ClaimTypes.GivenName, user.Name),                 
                new Claim(ClaimTypes.Surname, user.Surname),               
                new Claim(ClaimTypes.Role, user.UserType.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var jwtToken = new JwtSecurityToken(
                expires: DateTime.UtcNow.Add(options.Value.Expires),
                claims: claims,
                signingCredentials:
                new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(options.Value.SecretKey)),
                    SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
