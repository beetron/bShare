using Bshare.Models;

namespace Bshare.Repository
{
    public interface IFilesUploadRepository
    {
        Task<List<FileUpload>> GetAllAsync();
        Task<FileUpload?> GetByShortLinkAsync(string shortLink);
        Task CreateFileUploadAsync(FileUpload fileUpload);
        Task DeleteAsync(int fileUploadId);
        Task<bool> CheckShortLinkAsync(string shortLink);
        Task<string> GenerateShortLinkAsync(int shortLinkLength);
        Task<bool> CheckPasswordAsync(FileUpload fileUpload, string password);
    }
}
