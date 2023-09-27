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

        
        // Create method to create & upload new file
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(510 * 1024 * 1024)] // Total 510mb
        [RequestFormLimits(MultipartBodyLengthLimit = 505 * 1024 * 1024)] // Form data 505mb
        [Route ("/file/create")]
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

                    //default:
                    //    break;
                }

                // Generate short link and check database if unique
                fileUpload.ShortLink = await _iFilesUploadRepository.GenerateShortLink(6);

                // Create file directory if it doesn't exist
                string directoryPath = Path.Combine(Environment.GetEnvironmentVariable("bshare_UploadLocation"), fileUpload.ShortLink);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Saving each file to storage, and adding FileDetails to list
                foreach (var file in files)
                {
                    if (file != null & file.Length > 0)
                    {
                        // Get file size in MB format
                        double fileSizeBytes = file.Length;
                        double fileSizeKb = fileSizeBytes / 1024;
                        double fileSizeMb = fileSizeKb / 1024;

                        string filePath = Path.Combine(Environment.GetEnvironmentVariable("bshare_UploadLocation"), fileUpload.ShortLink, file.FileName);

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

                // Save database tables
                await _iFilesUploadRepository.CreateFileUploadAsync(fileUpload);

                return RedirectToAction(nameof(Upload));
                //return View("Upload");
            }
            return View("Upload");
        }

        // Get method to retrieve single data from UploadId
        // [HttpGet]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> GetById(FileUpload fileUpload, int uploadId)
        // {
        //     FileUpload fileRecord = await _iFilesUploadRepository.GetById(uploadId);
        //     if (fileRecord != null)
        //     {
        //     }
        //
        // }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upload()
        {
            return View();
        }


        [Route("/{shortLink}")]
        public async Task<IActionResult> ShortLink(string shortLink)
        {
            bool shortLinkExists = await _iFilesUploadRepository.CheckShortLink(shortLink);
            if (shortLinkExists)
            {
                FileUpload fileRecord = await _iFilesUploadRepository.GetByShortLink(shortLink);

                ViewBag.Title = shortLink;
                return View(fileRecord);
            }
            else
            {
                return View("BadLink");
            }
        }

        public IActionResult BadLink()
        {
            return View();
        }

    }
}
