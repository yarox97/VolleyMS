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

        public async Task DeleteFileAsync(string publicUrlOrId)
        {
            var publicId = GetPublicIdFromUrl(publicUrlOrId);

            if (string.IsNullOrWhiteSpace(publicId)) return;

            var deletionParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Image
            };

            var result = await _cloudinary.DestroyAsync(deletionParams);

            if (result.Result != "ok" && result.Result != "not found")
            {
                // LOGG
            }
        }

        public static string GetPublicIdFromUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return string.Empty;

            if (!url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return url;

            try
            {
                var uri = new Uri(url);
                var pathSegments = uri.Segments;

                int uploadIndex = -1;
                for (int i = 0; i < pathSegments.Length; i++)
                {
                    if (pathSegments[i].Equals("upload/", StringComparison.OrdinalIgnoreCase))
                    {
                        uploadIndex = i;
                        break;
                    }
                }

                if (uploadIndex == -1 || uploadIndex == pathSegments.Length - 1)
                    return Path.GetFileNameWithoutExtension(url);

                var relevantSegments = pathSegments.Skip(uploadIndex + 1).ToList();

                if (relevantSegments.Any() && relevantSegments[0].StartsWith("v") && relevantSegments[0].Length > 1 && char.IsDigit(relevantSegments[0][1]))
                {
                    relevantSegments.RemoveAt(0);
                }

                var fullPath = string.Join("", relevantSegments);

                var extensionIndex = fullPath.LastIndexOf('.');
                if (extensionIndex > 0)
                {
                    fullPath = fullPath.Substring(0, extensionIndex);
                }

                return fullPath;
            }
            catch
            {
                return Path.GetFileNameWithoutExtension(url);
            }
        }
    }
}

