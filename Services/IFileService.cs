using Bshare.Models;

namespace Bshare.Services
{
    public interface IFileService
    {
        Task<ICollection<FileDetail>> SaveFileAsync(UploadViewModel uploadViewModel, List<IFormFile> files, string _localFilePath);
        Task<ICollection<FileDetail>> DeleteFile(DeleteViewModel deleteViewModel, string _localFilePath);
        
    }
}
