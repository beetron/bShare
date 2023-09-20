using Bshare.Models;
using Microsoft.CodeAnalysis.Elfie.PDB;
using Microsoft.EntityFrameworkCore;

namespace Bshare.Db
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<FileUpload> FileUploads { get; set; }
        public DbSet<FileDetail> FileDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FileDetail>()
                .HasOne(a => a.FileUpload)
                .WithMany(b => b.FileDetails)
                .HasForeignKey(a => a.FileUploadId)
                .IsRequired();

            // Cascading delete behavior to remove any files tied with FileUploadID
            modelBuilder.Entity<FileUpload>()
                .HasMany(upload => upload.FileDetails)
                .WithOne(detail => detail.FileUpload)
                .HasForeignKey(detail => detail.FileUploadId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
