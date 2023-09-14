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
                .HasForeignKey(b => b.FileDetailId);

        }
    }
}
