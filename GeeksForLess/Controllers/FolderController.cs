using GeeksForLess.Data;
using GeeksForLess.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            if (newParentFolder.Id != 0)
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

        public IActionResult SaveDbToJson()
        {
            Models.File file = new Models.File
            {
                Name = $"{Guid.NewGuid()}.json"
            };

            return View(file);
        }

        [HttpPost]
        public IActionResult SaveDbToJson(Models.File file)
        {
            if(file.Name == null)
            {
                file.Name = $"{Guid.NewGuid()}.json";
            }
            
            IEnumerable<Folder> folders = _db.Folder;

            var json = JsonConvert.SerializeObject(folders);

            try
            {
                using StreamWriter creatingFile = new(file.Name);

                creatingFile.Write(json);

                return RedirectToAction("Index");
            }
            catch
            {
                return NotFound(); //TODO: add show error "Incorrect path"
            }

        }
    }
}
