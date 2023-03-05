using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GeeksForLess.Models
{
    public class Folder
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public int FolderKey { get; set; }
        public string Name { get; set; }
        public string? ChildFoldersKeys { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}
