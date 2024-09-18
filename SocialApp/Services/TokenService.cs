using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using SocialApp.Helpers;
using SocialApp.Interfaces.Services;
using SocialApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialApp.Services
{
    public class TokenService:ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(AppUser user)
        {
            var userProfile = user.UserProfile;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, userProfile.UserName),
                new Claim(ClaimTypes.NameIdentifier, userProfile.Id)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        //accept a authHeader containing a jwt token
        //returna TokenPayload object
        public TokenPayload ExtractPayload(string authHeader)
        {
            Console.WriteLine("extracting payload");
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();

                // Now you can decode the token
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

               foreach(var c in jwtToken.Claims)
                {
                    Console.WriteLine(c);
                    Console.WriteLine(c.Type);
                }

                // Access the token payload (claims)
                //var profileId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                //for whatever reason the Claim type was changed ::skull
                var profileId = jwtToken.Claims.FirstOrDefault(c => c.Type =="nameid")?.Value;

                return new TokenPayload { userProfileId = profileId };
            }
            else
            {
                throw (new Exception("Argument is null or does not start with 'Bearer '"));
            }
        }

        public string? GetProfileIdFromToken(string token)
        {
            
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                foreach (var c in jwtToken.Claims)
                {
                    Console.WriteLine(c);
                    Console.WriteLine(c.Type);
                }

                // Access the token payload (claims)
                //var profileId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                //for whatever reason the Claim type was changed ::skull
                var profileId = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;


                return profileId;

            
        }

    }
}
