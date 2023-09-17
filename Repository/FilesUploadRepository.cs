using Bshare.Db;
using Bshare.Models;
using Microsoft.EntityFrameworkCore;

namespace Bshare.Repository
{
    public class FilesUploadRepository : IFilesUploadRepository
    {
        private readonly ApplicationDbContext _fileUploadContext;

        public FilesUploadRepository(ApplicationDbContext fileUploadContext)
        {
            _fileUploadContext = fileUploadContext;
        }

        public async Task<List<FileUpload>> GetAllAsync()
        {
           return await _fileUploadContext.FileUploads.ToListAsync();
        }

        public Task<FileUpload> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task FileUploadAsync(FileUpload fileUpload)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
