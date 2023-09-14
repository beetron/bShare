using Bshare.Models;
using Microsoft.CodeAnalysis.Elfie.PDB;
using Microsoft.EntityFrameworkCore;

namespace Bshare.Db
{
    public class BshareDbContext : DbContext
    {
        public BshareDbContext(DbContextOptions<BshareDbContext> options) : base(options)
        {
        }

        public DbSet<FileUpload> FileUploads { get; set; }
        public DbSet<FileDetail> FileDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FileUpload>()
                .HasMany(a => a.FileDetails)
                .WithOne(b => b.FileUpload)
                .HasForeignKey(b => b.FileUploadId);


            // Cascading delete behavior to remove any files tied with FileUploadID
            modelBuilder.Entity<FileUpload>()
                .HasMany(upload => upload.FileDetails)
                .WithOne(detail => detail.FileUpload)
                .HasForeignKey(detail => detail.FileUploadId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
