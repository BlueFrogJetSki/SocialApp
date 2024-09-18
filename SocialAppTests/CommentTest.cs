using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SocialApp.Controllers;
using SocialApp.Data;
using SocialApp.Interfaces.Repositories;
using SocialApp.Interfaces.Services;
using SocialApp.Models;

namespace SocialAppTests
{
    [TestClass]
    public class CommentsControllerTests
    {
        private ApplicationDbContext _context;
        private Mock<ITokenService> _mockTokenService;
        private Mock<ILikeService> _mockLikeService;
        private Mock<ICommentRepository> _mockCommentRepository;
        private CommentsController _controller;
        private Mock<HttpContext> _mockHttpContext;

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SocialAppTestDb_" + Guid.NewGuid())
                .Options;

            _context = new ApplicationDbContext(options);
            SeedDatabase();

            _mockTokenService = new Mock<ITokenService>();
            _mockLikeService = new Mock<ILikeService>();
            _mockCommentRepository = new Mock<ICommentRepository>(); 

            _mockTokenService.Setup(t => t.GetProfileIdFromToken(It.IsAny<string>()))
                .Returns("user1");

            _mockHttpContext = new Mock<HttpContext>();
            _mockHttpContext.Setup(ctx => ctx.Request.Headers["Authorization"])
                .Returns("Bearer token");

            _controller = new CommentsController(_context, _mockTokenService.Object, _mockLikeService.Object, _mockCommentRepository.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _mockHttpContext.Object
                }
            };
        }

        private void SeedDatabase()
        {
            // Clear existing data
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var userProfiles = new[]
            {
                new UserProfile { Id = "user1", UserName = "User One" },
                new UserProfile { Id = "user2", UserName = "User Two" }
            };

            var posts = new[]
            {
                new Post { Id = "post1", Description = "Post content" }
            };

            var comments = new[]
            {
                new Comment { Id = "comment1", Text = "This is a comment.", PostId = "post1", AuthorProfileId = "user1", AuthorName = "User One" }
            };

            _context.UserProfile.AddRange(userProfiles);
            _context.Post.AddRange(posts);
            _context.Comment.AddRange(comments);
            _context.SaveChanges();
        }


        [TestMethod]
        public async Task Create_ShouldReturnOk_WhenValidData()
        {
            // Arrange
            var newComment = new Comment { Id = "comment2", Text = "This is a new comment." };
            var postId = "post1";

            // Act
            var result = await _controller.Create(postId, newComment) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var response = result.Value as dynamic;
            Assert.IsNotNull(response);
            Assert.AreEqual("comment2", response.Id);
        }

        [TestMethod]
        public async Task Create_ShouldReturnNotFound_WhenUserProfileNotFound()
        {
            // Arrange
            _mockTokenService.Setup(t => t.GetProfileIdFromToken(It.IsAny<string>())).Returns("invalidUserId");
            var newComment = new Comment { Id = "comment2", Text = "This is a new comment." };
            var postId = "post1";

            // Act
            var result = await _controller.Create(postId, newComment) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual("profile not found", result.Value);
        }

        [TestMethod]
        public async Task Reply_ShouldReturnOk_WhenValidData()
        {
            // Arrange
            var newComment = new Comment { Id = "comment3", Text = "This is a reply." };
            var parentCommentId = "comment1";

            // Act
            var result = await _controller.Reply(parentCommentId, newComment) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var response = result.Value as dynamic;

            Assert.AreEqual("comment3", response.Id);
            Assert.AreEqual(1, _context.Comment.Find("comment1").SubComments.Count);
        }


        [TestMethod]
        public async Task Like_ShouldReturnOk_WhenValidData()
        {
            // Arrange
            var commentId = "comment1";

            // Act
            var result = await _controller.Like(commentId) as OkObjectResult;
            Console.WriteLine(result);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
           
        }

        [TestMethod]
        public async Task Like_ShouldReturnNotFound_WhenCommentNotFound()
        {
            // Arrange
            var commentId = "nonexistentCommentId";

            // Act
            var result = await _controller.Like(commentId) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }
    }
}
