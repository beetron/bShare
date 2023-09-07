using Microsoft.EntityFrameworkCore;

namespace Bshare.Db
{
    public class BshareContext : DbContext
    {
        public BshareContext(DbContextOptions<BshareContext> options) : base(options)
        {
        }
    }
}
