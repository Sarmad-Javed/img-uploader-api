using ImageUploader.Model;
using Microsoft.EntityFrameworkCore;
namespace ImageUploader.Data
{
    public class UploaderDbContext:DbContext
    {
        public UploaderDbContext(DbContextOptions<UploaderDbContext> options):base(options) {
        }
        public DbSet<Image> Images { get; set;}
    }
}
