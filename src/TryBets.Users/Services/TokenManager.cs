using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TryBets.Users.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace TryBets.Users.Services
{
    public class TokenManager
    {
        private readonly TokenOptions _tokenOptions;
        public TokenManager()
        {
            _tokenOptions = new TokenOptions {
                Secret = "4d82a63bbdc67c1e4784ed6587f3730c",
                ExpiresDay = 1
            };
        }

        public string Generate(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = AddClaims(user),
                SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenOptions.Secret!)),
                        SecurityAlgorithms.HmacSha256Signature
                    ),
                Expires = DateTime.Now.AddDays(_tokenOptions.ExpiresDay)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private ClaimsIdentity AddClaims(User user)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Email, user.Email!));
            return claims;
        }
    }
}