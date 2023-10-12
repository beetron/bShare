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

        // Get single upload record by short link
        public async Task<FileUpload?> GetByShortLinkAsync(string shortLink)
        {
            try
            {
                FileUpload fileUpload = await _fileUploadContext.FileUploads
                    .Include(f => f.FileDetails)
                    .FirstOrDefaultAsync(x => x.ShortLink == shortLink);
                Console.WriteLine(fileUpload);
                return fileUpload;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
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

        // Delete a single upload record by FileUploadId
        public async Task DeleteAsync(int fileUploadId)
        {
            try
            {
                var record = await _fileUploadContext.FileUploads.FindAsync(fileUploadId);
                if (record != null)
                {
                    _fileUploadContext.FileUploads.Remove(record);
                    await _fileUploadContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        
        // Check database for short link
        public async Task<bool> CheckShortLinkAsync(string shortLink)
        {
            try
            {
                FileUpload? result = await _fileUploadContext.FileUploads.FirstOrDefaultAsync(x => x.ShortLink == shortLink);
                if (result != null)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Short link generator with database check if link is unique
        public async Task<string> GenerateShortLinkAsync(int shortLinkLength)
        {
            string shortLink = "";
            FileUpload? result;

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
                return shortLink;
            }
        }

        public async Task<bool> CheckPasswordAsync(FileUpload fileUpload, string password)
        {
            try
            {
                if (String.IsNullOrEmpty(password))
                {
                    return false;
                }

                FileUpload? matchingFileUploadData =
                    await _fileUploadContext.FileUploads.FirstOrDefaultAsync(f =>
                        f.FileUploadId == fileUpload.FileUploadId);

                if (matchingFileUploadData != null)
                {
                    return matchingFileUploadData.Password == password;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return false;
        }
    }
}
