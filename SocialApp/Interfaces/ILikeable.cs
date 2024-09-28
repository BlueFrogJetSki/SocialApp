using SocialApp.Models;

namespace SocialApp.Interfaces
{
    public interface ILikeable
    {
        public string Id { get; set; }
        public int LikesCount { get; set; }

        public string Type { get; set; }

        //public bool AddLike(Like like);
        //public bool RemoveLike(Like like);
    }
}
