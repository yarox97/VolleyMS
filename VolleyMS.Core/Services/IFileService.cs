using Microsoft.AspNetCore.Http;

namespace VolleyMS.Core.Services
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string folder);
        Task DeleteFileAsync(string publicUrl);
    }
}
