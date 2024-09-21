using SocialApp.Models;

namespace SocialApp.Interfaces.Services
{
    public interface ILikeService
    {
        public bool LikeItem(ILikeable item, string authorProfileId);

        public bool RemoveLike(ILikeable item, string authorProfileId);

        public string GetEntityId(ILikeable item);
    }
}
