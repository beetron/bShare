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

        // Get full list of uploads (admin only)
        public async Task<List<FileUpload>> GetAllAsync()
        {
           return await _fileUploadContext.FileUploads.ToListAsync();
        }

        // Get single upload record by id (used for link sharing and edits)
        public async Task<FileUpload> GetByIdAsync(int id)
        {
            return _fileUploadContext.FileUploads.FirstOrDefault(p => p.FileUploadId == id);
        }

        // Create new file upload
        public async Task CreateAsync(FileUpload fileUpload)
        {
            try
            {
                await _fileUploadContext.FileUploads.AddAsync(fileUpload);
                await _fileUploadContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            
            //return Task.CompletedTask;
        }

        // Delete a single upload record by id
        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
