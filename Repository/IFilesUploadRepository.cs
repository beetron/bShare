using Bshare.Models;

namespace Bshare.Repository
{
    public interface IFilesUploadRepository
    {
        Task<List<FileUpload>> GetAllAsync();
        Task<FileUpload> GetByIdAsync(int id);
        Task CreateFileUploadAsync(FileUpload fileUpload);
        Task CreateFileDetailAsync(FileDetail fileDetail);
        Task DeleteAsync(int id);
        Task<string> GenerateShortLink(int shortLinkLength);
    }
}
