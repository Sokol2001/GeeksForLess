using GeeksForLess.Data;
using GeeksForLess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static NuGet.Packaging.PackagingConstants;

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
            return View(new Models.File());
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

        public IActionResult GetDbFromJson()
        {
            return View(new Models.File());
        }

        [HttpPost]
        public IActionResult GetDbFromJson(Models.File file)
        {
            if (file.Name == null)
            {
                return NotFound(); //TODO: add show error "Incorrect path"
            }
            try
            {
                var data = System.IO.File.ReadAllText(file.Name);

                var folders = JsonConvert.DeserializeObject<IEnumerable<Folder>>(data);

                if (_db.Folder.Count() != 0)
                {
                    _db.Folder.RemoveRange(_db.Folder);
                }

                _db.Folder.AddRange(folders);

                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(new Models.File()); //TODO: add show error 
            }
        }

        public IActionResult GetDbFromSystemDir()
        {
            var rootPath = "D:";
            var currentFolderName = "Creating Digital Images";

            List<Folder> folders = new();

            int folderCounter = 0;

            var currentRootPath = $"{rootPath}\\{currentFolderName}";

            if (Directory.Exists(rootPath))
            {
                int currentKey = folderCounter++;

                folders.Add(new Folder
                {
                    Name = currentRootPath.Replace($"{rootPath}\\", string.Empty),
                    FolderKey = currentKey,

                });

                string[] childFolderPaths = Directory.GetDirectories(currentRootPath, "*", SearchOption.TopDirectoryOnly);

                if(childFolderPaths.Length != 0)
                {
                    folders = AddChildFolders(folders, folderCounter, currentKey, childFolderPaths, currentRootPath);
                }

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound(); //TODO: add show error "Incorrect path"
            }

        }

        private List<Folder> AddChildFolders(List<Folder> folders, int folderCounter, int rootFolderKey, string[] childFolderPaths, string rootPath)
        {
            List<int> currentKeys = new List<int>();
            foreach (var currentRootPath in childFolderPaths)
            {
                if (Directory.Exists(currentRootPath))
                {
                    int currentKey = folderCounter++;

                    folders.Add(new Folder
                    {
                        Name = currentRootPath.Replace($"{rootPath}\\", string.Empty),
                        FolderKey = currentKey,

                    });

                    folders.Single(x => x.FolderKey == rootFolderKey).ChildFoldersKeys += $" {currentKey}";
                    currentKeys.Add(currentKey);
                }

            }

            for (int childNumber = 0; childNumber < childFolderPaths.Length; childNumber++)
            {
                string[] currentChildFolderPaths = Directory.GetDirectories(childFolderPaths[childNumber], "*", SearchOption.TopDirectoryOnly);

                if (childFolderPaths.Length != 0)
                {
                    folders = AddChildFolders(folders, folderCounter, currentKeys[childNumber], currentChildFolderPaths, childFolderPaths[childNumber]);
                }
            }
                return folders;
        }
    }
}
