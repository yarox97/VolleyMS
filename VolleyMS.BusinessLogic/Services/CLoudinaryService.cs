using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using VolleyMS.Core.Services;

namespace VolleyMS.BusinessLogic.Services
{

    public class CloudinaryService : IFileService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration config)
        {
            var account = new Account(
                config["Cloudinary:CloudName"],
                config["Cloudinary:ApiKey"],
                config["Cloudinary:SecretKey"]
            );
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folder)
        {
            if (file.Length <= 0) return string.Empty;

            using var stream = file.OpenReadStream();

            var isImage = file.ContentType.StartsWith("image");

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = $"VolleyMS/{folder}"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return uploadResult.SecureUrl.ToString();
        }

        public async Task DeleteFileAsync(string publicUrl)
        {
            // Логика извлечения PublicId из URL и удаления
        }

        public static string GetPublicIdFromUrl(string url)
        {
            return Path.GetFileNameWithoutExtension(url);
        }
    }
}
