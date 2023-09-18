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
        public async Task <IActionResult> Create(FileUpload fileUpload)
        {
            if (ModelState.IsValid)
            {
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
