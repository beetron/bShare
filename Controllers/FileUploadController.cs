using Bshare.Db;
using Bshare.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bshare.Controllers
{
    public class FileUploadController : Controller
    {
        readonly ApplicationDbContext _context;

        public FileUploadController(ApplicationDbContext context)
        {
            _context = context;
        }

        // void ShortLink()
        // {
        //     string linkCheck = _context.FileUploads.ToString();
        // }

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(
                "FileUploadId", "ShortLink", "DateUpload", "DateExpire", "Password")]
            FileUpload fileUpload, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                // Generate unique short link
                fileUpload.ShortLink = ShortGuid.NewGuid().ToString();

            }
        }*/

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
