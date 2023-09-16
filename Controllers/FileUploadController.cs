using Microsoft.AspNetCore.Mvc;

namespace Bshare.Controllers
{
    public class FileUploadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
