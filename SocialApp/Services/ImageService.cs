using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using SocialApp.Helpers;
using SocialApp.Interfaces.Services;


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


        public async Task<ImageUploadResult> UploadPostImageAsync(IFormFile file)
        {
            // Ensure the result is initialized
            var uploadResult = new ImageUploadResult();

            // Check if the file is valid
            if (file != null && file.Length > 0)
            {

                using (var inputStream = file.OpenReadStream())
                {
                    var outputStream = new MemoryStream();

                    resizeImage(inputStream, outputStream, 1080, 1080);

                    // Prepare parameters for the image upload
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, outputStream),
                        UniqueFilename = false

                    };


                    // Upload the image to Cloudinary
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    //throw errors if there are any
                    if (uploadResult.Error != null)
                    {
                        throw new Exception(uploadResult.Error.Message);
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

        public async Task<ListResourcesResult> ListAllImageAsync()
        {
            var listParams = new ListResourcesParams()
            {
                MaxResults = 10, // Adjust as needed

            };

            var resources = await _cloudinary.ListResourcesAsync(listParams);

            return resources;


        }

        public async Task<ImageUploadResult> UploadProfileImageAsync(IFormFile file)
        {
            // Ensure the result is initialized
            var uploadResult = new ImageUploadResult();

            // Check if the file is valid
            if (file != null && file.Length > 0)
            {

                using (var inputStream = file.OpenReadStream())
                {
                    var outputStream = new MemoryStream();
                   

                    resizeImage(inputStream, outputStream,110,110);

                    // Prepare parameters for the image upload
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, outputStream),
                        UniqueFilename = false

                    };


                    // Upload the image to Cloudinary
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    //throw errors if there are any
                    if (uploadResult.Error != null)
                    {
                        throw new Exception(uploadResult.Error.Message);
                    }


                }
            }

            return uploadResult;
        }

        private void resizeImage(Stream inputStream, Stream outputStream, int width, int height)
        {
            //ImageFactory imgFactory = new ImageFactory().Load(inputStream).Constrain(size);
            using (Image image = Image.Load(inputStream))
            {
                image.Mutate(x => x.Resize(width, height));
                
                image.Save(outputStream, new PngEncoder());

            }

        }
    }
}
