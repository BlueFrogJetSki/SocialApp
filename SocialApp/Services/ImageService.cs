using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using SocialApp.Helpers;
using SocialApp.Interfaces;
using System.Configuration;

namespace SocialApp.Services
{
    public class ImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;
        private readonly string CloudinaryURL;

        public ImageService(IOptions<CloudinarySettings> config)
        {
            CloudinaryURL = $"cloudinary://{config.Value.ApiKey}:{config.Value.ApiSecret}@{config.Value.CloudName}";

            _cloudinary = new Cloudinary(CloudinaryURL);

        }


        public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
        {
            // Ensure the result is initialized
            var uploadResult = new ImageUploadResult();

            // Check if the file is valid
            if (file != null && file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    // Prepare parameters for the image upload
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        UniqueFilename = false,
                        // TODO: Add image transformation parameters if needed
                    };

                    try
                    {
                        // Upload the image to Cloudinary
                        uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    }
                    catch (Exception ex)
                    {
                       Console.WriteLine(ex);
                    }
                }
            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeleteImageAsync(string id)
        {
            var deleteParams = new DeletionParams(id);
            var result = await _cloudinary.DestroyAsync(deleteParams);


            return result;
        }

        public async Task<ListResourcesResult>  ListAllImageAsync()
        {
            var listParams = new ListResourcesParams()
            {
                MaxResults = 10, // Adjust as needed
               
            };

            var resources = await _cloudinary.ListResourcesAsync(listParams);

            return resources;

            
        }
    }
}
