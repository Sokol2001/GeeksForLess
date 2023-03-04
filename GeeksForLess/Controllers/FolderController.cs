using GeeksForLess.Data;
using GeeksForLess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace GeeksForLess.Controllers
{
    public class FolderController : Controller
    {
        private readonly ApplicationDbContext _db;

        public FolderController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(Folder newParentFolder = null)
        {
            if(newParentFolder.Id != 0 && newParentFolder.Id != null)
            {
                var oldParentFolder = _db.Folder.Where(x => x.IsSelected == true).First();
                newParentFolder = _db.Folder.Where(x => x.Id == newParentFolder.Id).First();

                oldParentFolder.IsSelected = false;
                newParentFolder.IsSelected = true;

                _db.Folder.Update(newParentFolder);
                _db.Folder.Update(oldParentFolder);

                _db.SaveChanges();
            }

            IEnumerable<Folder> folders = _db.Folder;

            return View(folders.OrderByDescending(x => x.IsSelected));
        }

    }
}
