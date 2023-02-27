using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using P02_FootballBetting.Data.Models;

namespace P02_FootballBetting.Data 
{
    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext(DbContextOptions options) : base(options)
        {

        }

        public FootballBettingContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=FootballBookmakerSystem;Trusted_Connection=True;Integrated Security=True;");
            }
        }

        public virtual DbSet<Bet> Bets { get; set; } = null!;

        public virtual DbSet<Color> Colors { get; set; } = null!;

        public virtual DbSet<Country> Countries { get; set; } = null!;

        public virtual DbSet<Game> Games { get; set; } = null!;

        public virtual DbSet<Player> Players { get; set; } = null!;
        public virtual DbSet<PlayerStatistic> PlayersStatistics { get; set; } = null!;
        public virtual DbSet<Position> Positions { get; set; } = null!;
        public virtual DbSet<Team> Teams { get; set; } = null!;
        public virtual DbSet<Town> Towns { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasOne(e => e.PrimaryKitColor)
                .WithMany(e => e.PrimaryKitTeams)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasForeignKey(e => e.PrimaryKitColorId);

                entity.HasOne(e => e.SecondaryKitColor)
                .WithMany(e => e.SecondaryKitTeams)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasForeignKey(e => e.SecondaryKitColorId);

            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasOne(e => e.HomeTeam)
                .WithMany(t => t.HomeGames)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasForeignKey(e => e.HomeTeamId);

                entity.HasOne(e => e.AwayTeam)
                .WithMany(e => e.AwayGames)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasForeignKey(e => e.AwayTeamId);

            });

            modelBuilder.Entity<PlayerStatistic>(entity =>
            {
                entity.HasKey(pk => new { pk.PlayerId, pk.GameId });

                entity.HasOne(s=>s.Game)
                .WithMany(g=>g.PlayersStatistics)
                .HasForeignKey(s=>s.GameId);

                entity.HasOne(s => s.Player)
                .WithMany(g => g.PlayersStatistics)
                .HasForeignKey(s => s.PlayerId);
            });

            
        }
    }
}
