using Microsoft.EntityFrameworkCore;
using music_api_gateway.Models;

namespace music_api_gateway.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
