using System.ComponentModel.DataAnnotations;

namespace GeeksForLess.Models
{
    public class DirectoryEllement
    {
        public string? Name { get; set; }
        public string? RootPath { get; set; } = "D:";
    }
}
