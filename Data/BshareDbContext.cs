using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using File = Bshare.Models.File;

namespace Bshare.Db
{
    public class BshareDbContext : DbContext
    {
        public BshareDbContext(DbContextOptions<BshareDbContext> options) : base(options)
        {
        }

        public DbSet<File> Files { get; set; }
    }
}
