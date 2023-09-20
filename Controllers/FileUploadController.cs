using Bshare.Functions;
using Bshare.Models;
using Bshare.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Bshare.Controllers
{
    public class FileUploadController : Controller
    {
        readonly IFilesUploadRepository _iFilesUploadRepository;

        public FileUploadController(IFilesUploadRepository iFilesUploadRepository)
        {
            _iFilesUploadRepository = iFilesUploadRepository;
        }

        // void ShortLink()
        // {
        //     string linkCheck = _context.FileUploads.ToString();
        // }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                bool isNotUnique;
                string? shortLink = null;

                do
                {
                    shortLink = ShortLinkGenerator.LinkGenerate(6);
                    isNotUnique = await _iFilesUploadRepository.CheckShortLink(shortLink);
                } while (isNotUnique);

                if (!isNotUnique)
                {
                    fileUpload.ShortLink = shortLink;
                }

                // Create file upload and save
                await _iFilesUploadRepository.CreateFileUploadAsync(fileUpload);

                // Create file directory if it doesn't exist
                string directoryPath = Path.Combine("c:/dev/UPLOADS", shortLink);
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
                }

                

                
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
