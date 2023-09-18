using System.Runtime.CompilerServices;
using Bshare.Db;
using Bshare.Models;
using Bshare.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bshare.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IFilesUploadRepository _iFilesUploadRepository;

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

                // Process selected files
                foreach (var file in files)
                {
                    if (file != null & file.Length > 0)
                    {
                        // Get file size in MB format
                        var fileSizeBytes = file.Length;
                        var fileSizeKb = fileSizeBytes / 1024;
                        var filesizeMb = fileSizeKb / 1024;

                        var filePath = Path.Combine("c:/dev/UPLOADS", file.FileName);

                        FileDetail fileDetail = new FileDetail
                        {
                            FileName = file.FileName,
                            FileSize = Convert.ToString(filesizeMb),
                            FilePath = filePath,
                            FileUploadId = fileUpload.FileUploadId
                        };

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                }


                await _iFilesUploadRepository.CreateAsync(fileUpload);
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
