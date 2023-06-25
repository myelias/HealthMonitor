using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        // in .csproj file, to fix the nullable error, I switched <Nullable>disable</Nullable> to enable
        {
            
        }
        public DbSet<Platform> Platforms { get; set; }
    }
}