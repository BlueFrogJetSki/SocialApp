using SocialApp.Helpers;
using SocialApp.Models;
using SocialApp.Services;

namespace SocialApp.Interfaces.Services
{
    public interface ITokenService
    {
        public string GenerateJwtToken(AppUser user);
        public TokenPayload ExtractPayload(string authHeader);

        public string? GetProfileIdFromToken(string authHeader);
      

    }
}
