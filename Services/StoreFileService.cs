using Bshare.Models;

namespace Bshare.Services
{
    public class StoreFileService : IStoreFileService
    {
        public async Task<FileDetail> StoreFile(FileUpload fileUpload, List<IFormFile> files, string _localFilePath)
        {
            // Create file directory if it doesn't exist
            string directoryPath = Path.Combine(_localFilePath, fileUpload.ShortLink);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Saving each file to local storage, and adding FileDetails to list
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
        }
    }
}
