using Bshare.Models;

namespace Bshare.Services
{
    public interface IStoreFileService
    {
        Task<FileDetail> StoreFile(FileUpload fileUpload, List<IFormFile> files, string _localFilePath);
    }
}
