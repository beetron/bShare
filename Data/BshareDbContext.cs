using Bshare.Models;
using Microsoft.EntityFrameworkCore;

namespace Bshare.Db
{
    public class BshareDbContext : DbContext
    {
        public BshareDbContext(DbContextOptions<BshareDbContext> options) : base(options)
        {
        }

        public DbSet<FileMain> BshareFiles { get; set; }
    }
}
