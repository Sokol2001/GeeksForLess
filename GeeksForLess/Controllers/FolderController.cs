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
            return View(new DirectoryEllement());
        }

        [HttpPost]
        public IActionResult SaveDbToJson(DirectoryEllement file)
        {
            if(file.Name == null)
            {
                file.Name = $"{Guid.NewGuid()}.json";
            }
            
            IEnumerable<Folder> folders = _db.Folder;

            var json = JsonConvert.SerializeObject(folders);

            try
            {
                string rootPath = string.Empty;

                if (file.RootPath != null)
                {
                    rootPath = file.RootPath.EndsWith("\\") ? file.RootPath : $"{file.RootPath}\\";
                }

                using StreamWriter creatingFile = new(rootPath + file.Name);

                creatingFile.Write(json);

                return RedirectToAction("Index");
            }
            catch
            {
                return NotFound();
            }

        }

        public IActionResult GetDbFromJson()
        {
            return View(new DirectoryEllement());
        }

        [HttpPost]
        public IActionResult GetDbFromJson(DirectoryEllement file)
        {
            if (file.Name == null)
            {
                return NotFound();
            }
            try
            {
                string rootPath = string.Empty;

                if (file.RootPath != null)
                {
                    rootPath = file.RootPath.EndsWith("\\") ? file.RootPath : $"{file.RootPath}\\";
                }

                var data = System.IO.File.ReadAllText(rootPath + file.Name);

                var folders = JsonConvert.DeserializeObject<IEnumerable<Folder>>(data);

                ChangeDataInDb(folders);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(new DirectoryEllement()); 
            }
        }

        public IActionResult GetDbFromSystemDir()
        {
            return View(new DirectoryEllement());
        }

        [HttpPost]
        public IActionResult GetDbFromSystemDir(Models.DirectoryEllement file)
        {
            var rootPath = file.RootPath.EndsWith("\\") ? file.RootPath : $"{file.RootPath}\\";
            var currentFolderName = file.Name;

            List<Folder> folders = new();

            var currentRootPath = $"{rootPath}\\{currentFolderName}";

            if (Directory.Exists(rootPath))
            {
                folders = AddChildFolders(folders, -1, new[] { currentRootPath }, rootPath);

                ChangeDataInDb(folders);

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }

        }

        private void ChangeDataInDb(IEnumerable<Folder> folders)
        {
            if (_db.Folder.Count() != 0)
            {
                _db.Folder.RemoveRange(_db.Folder);
            }

            _db.Folder.AddRange(folders);

            _db.SaveChanges();
        }

        private List<Folder> AddChildFolders(List<Folder> folders, int rootFolderKey, string[] childFolderPaths, string rootPath)
        {
            foreach(var currentRootPath in childFolderPaths)
            {
                if (Directory.Exists(currentRootPath))
                {
                    int currentKey = folders.Count();

                    folders.Add(new Folder
                    {
                        Name = currentRootPath.Replace($"{rootPath}\\", string.Empty),
                        FolderKey = currentKey,
                    });

                    if(rootFolderKey != -1)
                    {
                        folders.Single(x => x.FolderKey == rootFolderKey).ChildFoldersKeys += $" {currentKey}";
                        folders.Single(x => x.FolderKey == rootFolderKey).IsSelected = true;
                    }

                    string[] currentChildFolderPaths = Directory.GetDirectories(currentRootPath, "*", SearchOption.TopDirectoryOnly);

                    if (childFolderPaths.Length != 0)
                    {
                        folders = AddChildFolders(folders, currentKey, currentChildFolderPaths, currentRootPath);
                    }
                }
            }

            return folders;
        }
    }
}
