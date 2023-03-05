using GeeksForLess.Data;
using GeeksForLess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeeksForLess.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            if(_db.Folder.Count() != 0)
            {
                var activeFolders = _db.Folder.Where(x => x.IsSelected == true);

                foreach (var folder in activeFolders)
                {
                    folder.IsSelected = false;
                    _db.Folder.Update(folder);
                }

                var newParentFolder = _db.Folder.OrderBy(x => x.FolderKey).First();

                newParentFolder.IsSelected = true;

                _db.Folder.Update(newParentFolder);

                _db.SaveChanges();

                return View(newParentFolder);
            }
            return View();

        }

    }
}
