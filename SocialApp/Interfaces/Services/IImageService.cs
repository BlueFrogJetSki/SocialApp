using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace SocialApp.Interfaces.Services
{
    public interface IImageService
    {
        public Task<ImageUploadResult> UploadImageAsync(IFormFile file);
        public Task<DeletionResult> DeleteImageAsync(string id);

        public Task<ListResourcesResult> ListAllImageAsync();
    }
}
