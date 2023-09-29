using Bshare.Models;
using Bshare.Repository;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using Bshare.Services;

namespace Bshare.Controllers
{
    public class FileUploadController : Controller
    {
        readonly IFilesUploadRepository _iFilesUploadRepository;
        readonly IStoreFileService _iStoreFileService;
        string _localFilePath = Environment.GetEnvironmentVariable("bshare_UploadLocation");

        public FileUploadController(IFilesUploadRepository iFilesUploadRepository, IStoreFileService iStoreFileService)
        {
            _iFilesUploadRepository = iFilesUploadRepository;
            _iStoreFileService = iStoreFileService;
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

                // Generate short link and check database if unique (6 characters specified)
                fileUpload.ShortLink = await _iFilesUploadRepository.GenerateShortLink(6);

                // Create new directory and save files to local storage
                fileUpload.FileDetails = await _iStoreFileService.StoreFile(fileUpload, files, _localFilePath);

                // Save database tables
                await _iFilesUploadRepository.CreateFileUploadAsync(fileUpload);

                return RedirectToAction(nameof(Upload));
                //return View("Upload");
            }
            return View("Upload");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upload()
        {
            return View();
        }


        // Redirects to short link URL if the short link exists in database
        [Route("/{shortLink}")]
        public async Task<IActionResult> ShortLink(string shortLink)
        {
            bool shortLinkExists = await _iFilesUploadRepository.CheckShortLink(shortLink);
            if (shortLinkExists)
            {
                FileUpload fileRecord = await _iFilesUploadRepository.GetByShortLink(shortLink);

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

        // Download single file, or multiple as Zip
        [Route("/file/Download")]
        public IActionResult DownloadFile(FileUpload fileUpload, string[] fileNames, string fileName)
        {
            if (!ModelState.IsValid)
            {
                // Handle ModelState errors here or log them for debugging
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    // Log or handle the error as needed
                }

                // You can also choose to return a view or an error message here
                // Example: return View("ErrorViewName");
            }

            string fileLocation = Path.Combine(_localFilePath + fileUpload.ShortLink);
            string fileNameZip = fileUpload.ShortLink + ".zip";
            string fileNameSingle = "/" + fileName;

            // Multiple file download as Zip
            if (fileNames.Length >= 2)
            {
                string fileDestination = Path.Combine(_localFilePath, fileNameZip);

                // Create zip file
                if (!System.IO.File.Exists(fileDestination))
                {
                    ZipFile.CreateFromDirectory(fileLocation, fileDestination);
                }

                using MemoryStream memoryStream = new MemoryStream();
                using (FileStream fileStream = new FileStream(fileDestination, FileMode.Open))
                {
                    fileStream.CopyTo(memoryStream);
                }

                return File(memoryStream.ToArray(), "application/zip", fileNameZip);
            }

            // Single file download
            if (!String.IsNullOrEmpty(fileName))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (FileStream fileStream = new FileStream(fileLocation + fileNameSingle, FileMode.Open))
                    {
                        fileStream.CopyTo(memoryStream);
                    }

                    return File(memoryStream.ToArray(), "image/*", fileNameSingle);
                }
            }
            return Ok();
        }
    }
}
