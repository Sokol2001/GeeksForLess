using Microsoft.AspNetCore.Mvc;

namespace GeeksForLess.Controllers
{
    public class FolderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
