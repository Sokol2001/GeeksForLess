using GeeksForLess.Models;
using Microsoft.EntityFrameworkCore;

namespace GeeksForLess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Folder> Folder { get; set; }
    }

}
