namespace TestProject
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SocialApp.Data;
    using SocialApp.Models;
    using SocialApp.Repositories;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

   
        [TestClass]
        public class PostRepositoryTests
        {
            private Mock<ApplicationDbContext> _mockContext;
            private PostRepository _repository;
            private Mock<DbSet<Post>> _mockPostSet;

            [TestInitialize]
            public void Setup()
            {
                // Setup the mock context and DbSet
                _mockContext = new Mock<ApplicationDbContext>();

                _mockPostSet = new Mock<DbSet<Post>>();

                // Setting up the mock DbSet to return data for testing
                var posts = new List<Post>
            {
                new Post { Id = "1", AuthorProfile = new UserProfile(), LikedUsers = new List<UserProfile>(), Comments = new List<Comment>() },
                new Post { Id = "2", AuthorProfile = new UserProfile(), LikedUsers = new List<UserProfile>(), Comments = new List<Comment>() }
            }.AsQueryable();

                _mockPostSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.Provider);
                _mockPostSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.Expression);
                _mockPostSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.ElementType);
                _mockPostSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.GetEnumerator());

                _mockContext.Setup(c => c.Post).Returns(_mockPostSet.Object);

                _repository = new PostRepository(_mockContext.Object);
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
                var newPost = new Post { Id = "3", AuthorProfile = new UserProfile(), LikedUsers = new List<UserProfile>(), Comments = new List<Comment>() };

                // Act
                var result = await _repository.CreateAsync(newPost);

                // Assert
                Assert.IsTrue(result);
            }

            [TestMethod]
            public async Task UpdateAsync_ShouldReturnTrue_WhenPostExistsAndIsUpdated()
            {
                // Arrange
                var postToUpdate = new Post { Id = "1", AuthorProfile = new UserProfile(), LikedUsers = new List<UserProfile>(), Comments = new List<Comment>() };

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