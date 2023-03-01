using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RPG_Game.Data.Models;

namespace RPG_Game.Data
{
    public class RPG_GameDbContext : DbContext
    {
        public RPG_GameDbContext()
        {

        }

        public RPG_GameDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Player> Players {get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);


            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}