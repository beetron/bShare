using Bshare.Models;

namespace Bshare.Repository
{
    public interface IFilesUploadRepository
    {
        Task<List<FileUpload>> GetAllAsync();
        Task<FileUpload> GetByIdAsync(int id);
        Task FileUploadAsync(FileUpload fileUpload);
        Task DeleteAsync(int id);
    }
}
