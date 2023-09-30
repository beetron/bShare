using Bshare.Models;

namespace Bshare.Repository
{
    public interface IFilesUploadRepository
    {
        Task<List<FileUpload>> GetAllAsync();
        Task<FileUpload> GetByShortLink(string shortLink);
        Task CreateFileUploadAsync(FileUpload fileUpload);
        Task CreateFileDetailAsync(FileDetail fileDetail);
        Task DeleteAsync(int fileUploadId);
        Task<bool> CheckShortLink(string shortLink);
        Task<string> GenerateShortLink(int shortLinkLength);
    }
}
