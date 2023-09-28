using Bshare.Models;

namespace Bshare.Services
{
    public interface IStoreFileService
    {
        Task<ICollection<FileDetail>> StoreFile(FileUpload fileUpload, List<IFormFile> files, string _localFilePath);
    }
}
