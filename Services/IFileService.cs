using Bshare.Models;

namespace Bshare.Services
{
    public interface IFileService
    {
        Task<ICollection<FileDetail>> SaveFileAsync(FileUpload fileUpload, List<IFormFile> files, string _localFilePath);
        Task<ICollection<FileDetail>> DeleteFileAsync(FileUpload fileUpload, string _localFilePath);
    }
}
