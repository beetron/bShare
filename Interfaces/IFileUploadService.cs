using Microsoft.AspNetCore.WebUtilities;

namespace Bshare.Interfaces
{
    public interface IFileUploadService
    {
        Task<bool> UploadFile(MultipartReader reader, MultipartSection section, string shortLink);
    }
}
