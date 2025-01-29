using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GameAPI.Models;

    public class GameAPIRestDbContext : DbContext
    {
        public GameAPIRestDbContext (DbContextOptions<GameAPIRestDbContext> options)
            : base(options)
        {
        }

        public DbSet<GameAPI.Models.Game> Game { get; set; } = default!;

        public DbSet<GameAPI.Models.Platform> Platform { get; set; } = default!;

        public DbSet<GameAPI.Models.Developer> Developer { get; set; } = default!;

        public DbSet<GameAPI.Models.Genre> Genre { get; set; } = default!;

        public DbSet<GameAPI.Models.GamePlatform> GamePlatform { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<GamePlatform>()
                    .HasKey(gp => new { gp.GameId, gp.PlatformId });

                modelBuilder.Entity<GamePlatform>()
                    .HasOne(gp => gp.Game)
                    .WithMany(g => g.GamePlatforms)
                    .HasForeignKey(gp => gp.GameId);

                modelBuilder.Entity<GamePlatform>()
                    .HasOne(gp => gp.Platform)
                    .WithMany(p => p.GamePlatforms)
                    .HasForeignKey(gp => gp.PlatformId);
            }
}
