using SocialApp.Models;

namespace SocialApp.Interfaces
{
    public interface ILikeService
    {
        public Like? LikeItem(ILikeable item, string authorProfileId);

        public Like? RemoveLike(ILikeable item, string authorProfileId);

        public string GetEntityId(ILikeable item);
    }
}
