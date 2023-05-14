using Microsoft.EntityFrameworkCore;

namespace S3AccessProviderAPI.Models
{
    public class FileDbContext : DbContext
    {
        public FileDbContext(DbContextOptions<FileDbContext> options) : base(options)
        {  
        }

        public DbSet<File> Files { get; set; }
    }
}
