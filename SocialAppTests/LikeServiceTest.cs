using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SocialApp.Data;
using SocialApp.Interfaces;
using SocialApp.Models;
using SocialApp.Services;

namespace SocialAppTests
{
    [TestClass]
    public class LikeServiceTests
    {
        private ApplicationDbContext _context;
        private LikeService _likeService;
        private Mock<ILikeable> _mockItem;
        private Post _post;
        private Comment _comment;
        private Story _story;

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _likeService = new LikeService(_context);

            _post = new Post() { Id = "postId1"};
            _comment = new Comment() { Id = "commentId1" };
            _story = new Story() { Id = "storyId1" };
        }

        [TestMethod]
        public void LikeItem_NewLike_AddsLikeAndIncrementsCount()
        {
            // Arrange
            var authorProfileId = "author1";
            var initialLikesCount = _post.LikesCount;

            // Act
            var result = _likeService.LikeItem(_post, authorProfileId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(authorProfileId, result.AuthorProfileId);
            Assert.AreEqual(_post.LikesCount, initialLikesCount + 1);
            Assert.IsTrue(_post.Likes.Any(like => like.AuthorProfileId == authorProfileId));

            var dbLike = _context.Like.FirstOrDefault(like => like.AuthorProfileId == authorProfileId);
            Assert.IsNotNull(dbLike);
            Assert.AreEqual(result.AuthorProfileId, dbLike.AuthorProfileId);
        }

        [TestMethod]
        public void LikeItem_ExistingLike_ReturnsExistingLike()
        {
            // Arrange
            var authorProfileId = "author1";
            var existingLike = new Like
            {
                AuthorProfileId = authorProfileId,
                EntityId = "postId1",
                DateTime = DateTime.Now,
                EntityType = "Post"
            };

            _context.Like.Add(existingLike);
            _context.SaveChanges();

            // Act
            var result = _likeService.LikeItem(_post, authorProfileId);
           

            // Assert
            Assert.IsTrue(existingLike.Equals(result));
            Assert.AreEqual(1, _context.Like.Count(like => like.AuthorProfileId == authorProfileId));
        }

    

        [TestMethod]
        public void RemoveLike_DecrementLikeCount_ReturnsExistingLike()
        {
            // Arrange
            var authorProfileId = "author1";
           
            //Returns the like instance
            var likeResult = _likeService.LikeItem(_post, authorProfileId);

            // Act
            var removalResult = _likeService.RemoveLike(_post, authorProfileId);

            // Assert
            Assert.AreEqual(likeResult, removalResult);
            Assert.AreEqual(0, _context.Like.Count(like => like.AuthorProfileId == authorProfileId));
            Assert.AreEqual(0, _post.LikesCount);
        }
    }
}
