using FashAI.Models;
using Microsoft.EntityFrameworkCore;
namespace FashAI.Data
{
    public class FashAIContext : DbContext
    {
        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<ClothingItem> ClothingItems { get; set; }
        public DbSet<SwapRequest> SwapRequests { get; set; }

        public FashAIContext(DbContextOptions<FashAIContext> options) : base(options) { }
    }
}
