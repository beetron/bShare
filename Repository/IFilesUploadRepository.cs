using Bshare.Models;

namespace Bshare.Repository
{
    public interface IFilesUploadRepository
    {
        Task<List<FileUpload>> GetAllAsync();
        Task<FileUpload> GetByIdAsync(int id);
        Task CreateAsync(FileUpload fileUpload);
        Task DeleteAsync(int id);
        Task<bool> CheckShortLink(string shortLink);
    }
}
