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
        readonly IFileUploadService _fileUploadService;

        public FileUploadController(IFilesUploadRepository iFilesUploadRepository, IFileUploadService iFileUploadService)
        {
            _iFilesUploadRepository = iFilesUploadRepository;
            _fileUploadService = iFileUploadService;
        }

        // void ShortLink()
        // {
        //     string linkCheck = _context.FileUploads.ToString();
        // }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(MultipartBodyLengthLimit = 524288000)]
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

                /*bool isNotUnique;
                string? shortLink = null;

                do
                {
                    shortLink = ShortLinkHelper.LinkGenerate(6);
                    isNotUnique = await _iFilesUploadRepository.CheckShortLink(shortLink);
                } while (isNotUnique);

                if (!isNotUnique)
                {
                    fileUpload.ShortLink = shortLink;
                }*/

                // Create file upload and save
                await _iFilesUploadRepository.CreateFileUploadAsync(fileUpload);

                // Create file directory if it doesn't exist
                // string directoryPath = Path.Combine("c:/dev/UPLOADS", shortLink);
                // if (!Directory.Exists(directoryPath))
                // {
                //     Directory.CreateDirectory(directoryPath);
                // }


                // Process selected files

                /*foreach (var file in files)
                {
                    if (file != null & file.Length > 0)
                    {
                        // Get file size in MB format
                        double fileSizeBytes = file.Length;
                        double fileSizeKb = fileSizeBytes / 1024;
                        double filesizeMb = fileSizeKb / 1024;

                        string filePath = Path.Combine("c:/dev/UPLOADS", shortLink, file.FileName);

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
                }*/

                // NEW PART

                string boundary = HeaderUtilities.RemoveQuotes(
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
                }

                // NEW PART END
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
