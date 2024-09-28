using SocialApp.Data;
using SocialApp.Interfaces;
using SocialApp.Interfaces.Services;
using SocialApp.Models;
using System;

namespace SocialApp.Services
{
    public class LikeService : ILikeService
    {
        private readonly ApplicationDbContext _context;
        public LikeService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Increment LikeCount by 1 in item
        //Create a like in the database
        //add the like to the Collection<Like> in item
        public bool LikeItem(ILikeable item, string authorProfileId)
        {
            
            if (item == null) { return false; }

            var existingLike = _context.Like.FirstOrDefault(like => like.AuthorProfileId == authorProfileId && like.EntityId == item.Id);

            if (existingLike != null) {  return true; }

     
            item.LikesCount++;

            Like newLike = new Like()
            {
                AuthorProfileId = authorProfileId,
                DateTime = DateTime.Now.ToUniversalTime(),
                EntityType = item.Type,
                EntityId = item.Id
            };

            _context.Like.Add(newLike);
            _context.SaveChanges();

           

           
            return true;

        }

        //Decrement LikeCount by 1 in item
        //remove the associated like in the database
        //remove the like to the Collection<Like> in item

        public bool RemoveLike(ILikeable item, string authorProfileId)
        {

            if (item == null) { return false; }

            var existingLike = _context.Like.FirstOrDefault(like => like.AuthorProfileId == authorProfileId && like.EntityId == GetEntityId(item));

            if (existingLike == null) { return false; }

            item.LikesCount--;

          
            _context.Like.Remove(existingLike);
            _context.SaveChanges();

           

            return true;

        }



        /// <summary>
        /// Helper method to retrieve the ID of the item being liked.
        /// </summary>
        /// <param name="item">The item to retrieve the ID from.</param>
        /// <returns>The ID of the item.</returns>
        public string GetEntityId(ILikeable item)
        {
            // Get the property info for the "Id" property
            var idProperty = item.GetType().GetProperty("Id");
            if (idProperty != null)
            {
                // Retrieve the value of the "Id" property and convert it to a string
                var idValue = idProperty.GetValue(item);
                return idValue?.ToString() ?? string.Empty;
            }
            throw new InvalidOperationException("Item does not have an 'Id' property.");
        }
    }
}
