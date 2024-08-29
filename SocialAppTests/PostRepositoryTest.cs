using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.Models;
using SocialApp.Repositories;

namespace SocialAppTests
{

    [TestClass]
    public class PostRepositoryTests
    {
        private ApplicationDbContext _context;
        private PostRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Seed the database with test data
            _context.Post.AddRange(
                new Post { Id = "1", AuthorProfile = new UserProfile() { UserName = "BlueFrog"}, },
                new Post { Id = "2", AuthorProfile = new UserProfile() { UserName = "GreenDog" } }
            );
            _context.SaveChanges();

            _repository = new PostRepository(_context);
        }


        [TestMethod]
        public async Task GetAsync_ShouldReturnPost_WhenPostExists()
        {
            // Arrange
            var postId = "1";

            // Act
            var result = await _repository.GetAsync(postId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(postId, result.Id);
        }

        [TestMethod]
        public async Task GetAsync_ShouldReturnNull_WhenPostDoesNotExist()
        {
            // Arrange
            var postId = "non-existent-id";

            // Act
            var result = await _repository.GetAsync(postId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldAddPost_WhenPostIsValid()
        {
            // Arrange
            var newPost = new Post { Id = "3", AuthorProfile = new UserProfile() { UserName = "YellowCat"} };

            // Act
            var result = await _repository.CreateAsync(newPost);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldReturnTrue_WhenPostExistsAndIsUpdated()
        {
            // Arrange
            var postToUpdate = new Post { Id = "1", AuthorProfile = new UserProfile()};

            // Act
            var result = await _repository.UpdateAsync(postToUpdate);

            // Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public async Task DeleteAsync_ShouldReturnTrue_WhenPostExistsAndIsDeleted()
        {
            // Arrange
            var postId = "1";

            // Act
            var result = await _repository.DeleteAsync(postId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldReturnFalse_WhenPostDoesNotExist()
        {
            // Arrange
            var postId = "non-existent-id";

            // Act
            var result = await _repository.DeleteAsync(postId);

            // Assert
            Assert.IsFalse(result);
        }
    }


}