using System.ComponentModel.DataAnnotations;

namespace GeeksForLess.Models
{
    public class Folder
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ChildFoldersIds { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}
