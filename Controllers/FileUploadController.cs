using System.Net.Http.Headers;
using Bshare.Functions;
using Bshare.Interfaces;
using Bshare.Models;
using Bshare.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using MediaTypeHeaderValue = Microsoft.Net.Http.Headers.MediaTypeHeaderValue;

namespace Bshare.Controllers
{
    public class FileUploadController : Controller
    {
        readonly IFilesUploadRepository _iFilesUploadRepository;

        public FileUploadController(IFilesUploadRepository iFilesUploadRepository)
        {
            _iFilesUploadRepository = iFilesUploadRepository;
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(510 * 1024 * 1024)] // Total 510mb
        [RequestFormLimits(MultipartBodyLengthLimit = 505 * 1024 * 1024)] // Form data 505mb
        public async Task <IActionResult> Create(FileUpload fileUpload, string dropdownSelection, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                fileUpload.DateUpload = DateTime.Now;

                // Set DateExpired from dropdown selection
                switch (dropdownSelection)
                {
                    case "12":
                        fileUpload.DateExpire = DateTime.Now.AddHours(12);
                        break;
                    case "24":
                        fileUpload.DateExpire = DateTime.Now.AddHours(24);
                        break;
                    case "48":
                        fileUpload.DateExpire = DateTime.Now.AddHours(48);
                        break;

                    default:
                        break;
                }

                // Generate short link and check database if unique
                fileUpload.ShortLink = await _iFilesUploadRepository.GenerateShortLink(6);

                // Create new file upload DB (not the actual file) and save
                await _iFilesUploadRepository.CreateFileUploadAsync(fileUpload);

                // Create file directory if it doesn't exist
                string directoryPath = Path.Combine(Environment.GetEnvironmentVariable("bshare_UploadLocation"), fileUpload.ShortLink);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Process selected files
                foreach (var file in files)
                {
                    if (file != null & file.Length > 0)
                    {
                        // Get file size in MB format
                        double fileSizeBytes = file.Length;
                        double fileSizeKb = fileSizeBytes / 1024;
                        double filesizeMb = fileSizeKb / 1024;

                        string filePath = Path.Combine(Environment.GetEnvironmentVariable("bshare_UploadLocation"), fileUpload.ShortLink, file.FileName);

                        FileDetail fileDetail = new FileDetail
                        {
                            FileName = file.FileName,
                            FileSize = filesizeMb,
                            FilePath = filePath,
                            FileUploadId = fileUpload.FileUploadId
                        };

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        await _iFilesUploadRepository.CreateFileDetailAsync(fileDetail);
                    }
                }

                // MultipartReader START

                /*string boundary = HeaderUtilities.RemoveQuotes(
                    MediaTypeHeaderValue.Parse(Request.ContentType).Boundary).Value;

                MultipartReader reader = new MultipartReader(boundary, Request.Body);
                MultipartSection section = await reader.ReadNextSectionAsync();

                string response = string.Empty;

                try
                {
                    if (await _fileUploadService.UploadFile(reader, section, fileUpload.ShortLink))
                    {
                        ViewBag.Message = "File(s) uploaded successfully.";
                    }
                    else
                    {
                        ViewBag.Message = "Upload failed";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Exception";
                }*/

                // MultipartReader END
                return RedirectToAction(nameof(Upload));
            }
            return View(Upload);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upload()
        {
            return View();
        }
    }
}
