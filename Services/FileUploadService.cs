using Bshare.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Bshare.Services
{
    public class FileUploadService : IFileUploadService
    {

        // Upload File(s)
        public async Task<bool> UploadFile(MultipartReader reader, MultipartSection? section, string shortLink)
        {
            while (section != null)
            {
                bool hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(
                    section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (contentDisposition.DispositionType.Equals("form-data") &&
                        (!string.IsNullOrEmpty(contentDisposition.FileName.Value) ||
                         !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value)))
                    {
                        
                        string filePath = Path.Combine(Environment.GetEnvironmentVariable("bshare_UploadLocation"), shortLink);
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }

                        byte[] fileArray;
                        using (var memoryStream = new MemoryStream())
                        {
                            await section.Body.CopyToAsync(memoryStream);
                            fileArray = memoryStream.ToArray();
                        }

                        using (var fileStream = File.Create(Path.Combine(filePath, contentDisposition.FileName.Value)))
                        {
                            await fileStream.WriteAsync(fileArray);
                        }
                    }
                }

                section = await reader.ReadNextSectionAsync();
            }
            return true;
        }
    }
}
