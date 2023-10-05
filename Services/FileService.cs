using Bshare.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis.Elfie.PDB;
using System.IO;

namespace Bshare.Services
{
    public class FileService : IFileService
    {
        // Save physical file on server
        public async Task<ICollection<FileDetail>> SaveFileAsync(FileUpload fileUpload, List<IFormFile> files, string _localFilePath)
        {
            string directoryPath = Path.Combine(_localFilePath, fileUpload.ShortLink);

            // Create file directory if it doesn't exist
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Saving each file to local storage, and adding FileDetails to list
            try
            {
                foreach (var file in files)
                {
                    if (file != null & file.Length > 0)
                    {
                        // Get file size in MB format
                        double fileSizeBytes = file.Length;
                        double fileSizeKb = fileSizeBytes / 1024;
                        double fileSizeMb = fileSizeKb / 1024;

                        string filePath = Path.Combine(_localFilePath, fileUpload.ShortLink, file.FileName);

                        fileUpload.FileDetails.Add(new FileDetail
                        {
                            FileName = file.FileName,
                            FileSize = fileSizeMb,
                            FilePath = filePath
                        });

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                }

                return fileUpload.FileDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Delete physical file from server
        public async Task<ICollection<FileDetail>> DeleteFileAsync(FileUpload fileUpload, string _localFilePath)
        {
            string directoryPath = Path.Combine(_localFilePath, fileUpload.ShortLink);

            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
                return null;
            }
            return null;
        }
    }
}
