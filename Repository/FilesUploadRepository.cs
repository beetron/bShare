using System.Linq.Expressions;
using Bshare.Db;
using Bshare.Functions;
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
        public async Task CreateFileUploadAsync(FileUpload fileUpload)
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

        // Create new file detail
        public async Task CreateFileDetailAsync(FileDetail fileDetail)
        {
            try
            {
                await _fileUploadContext.FileDetails.AddAsync(fileDetail);
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

        // Short link generator with database check if link is unique
        public async Task<string> GenerateShortLink(int shortLinkLength)
        {
            string shortLink;
            FileUpload result;

            try
            {
                do
                {
                    Random random = new Random();
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz12345679";
                    shortLink = new string(Enumerable.Repeat(chars, shortLinkLength)
                        .Select(s => s[random.Next(s.Length)])
                        .ToArray());

                    // Test if generated shortlink is unique 
                    result =
                        await _fileUploadContext.FileUploads.FirstOrDefaultAsync(f => f.ShortLink == shortLink);
                } while (result != null);

                return shortLink;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
