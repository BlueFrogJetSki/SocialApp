using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.Models;
using SocialApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialAppTests
{
    [TestClass]
    public class UserProfileRepositoryTests
    {
        private ApplicationDbContext _context;
        private UserProfileRepository _repository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SocialAppTestDb")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new UserProfileRepository(_context);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Seed with initial data
            _context.UserProfile.Add(new UserProfile
            {
                Id = "1",
                UserName = "testuser",
                Biography = "Test biography",
                IconURL = "http://example.com/icon.png"
            });
            _context.SaveChanges();
        }

        [TestMethod]
        public async Task GetAsync_ShouldReturnUserProfile_WhenIdExists()
        {
            // Arrange
            string testId = "1";

            // Act
            var result = await _repository.GetAsync(testId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testId, result.Id);
        }

        [TestMethod]
        public async Task GetAsync_ShouldReturnNull_WhenIdDoesNotExist()
        {
            // Arrange
            string testId = "999";

            // Act
            var result = await _repository.GetAsync(testId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetListAsync_ShouldReturnAllUserProfiles()
        {
            // Arrange & Act
            var result = await _repository.GetListAsync();

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldUpdateUserProfile_WhenIdExists()
        {
            // Arrange
            var userProfileToUpdate = new UserProfile
            {
                Id = "1",
                UserName = "updateduser",
                Biography = "Updated biography",
                IconURL = "http://example.com/newicon.png"
            };

            // Act
            var result = await _repository.UpdateAsync(userProfileToUpdate);

            // Assert
            Assert.IsTrue(result);

            var updatedProfile = await _repository.GetAsync("1");
            Assert.AreEqual("updateduser", updatedProfile?.UserName);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldReturnFalse_WhenIdDoesNotExist()
        {
            // Arrange
            var userProfileToUpdate = new UserProfile
            {
                Id = "999",
                UserName = "nonexistentuser"
            };

            // Act
            var result = await _repository.UpdateAsync(userProfileToUpdate);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task UserProfileExists_ShouldReturnTrue_WhenIdExists()
        {
            // Arrange
            string testId = "1";

            // Act
            var result = await _repository.ExistsAsync(testId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task UserProfileExists_ShouldReturnFalse_WhenIdDoesNotExist()
        {
            // Arrange
            string testId = "999";

            // Act
            var result = await _repository.ExistsAsync(testId);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
