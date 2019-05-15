using Microsoft.EntityFrameworkCore;

namespace GameOn.Web.Entities
{
    public class GameOnContext : DbContext
    {
        public DbSet<Player>      Players     { get; set; }
        public DbSet<RankHistory> RankHistory { get; set; }
        public DbSet<Match>       Matches     { get; set; }
        public DbSet<MatchSet>    MatchSets   { get; set; }
        public DbSet<Tournament>  Tournaments { get; set; }

        public DbSet<TournamentMatchSchedule> TournamentMatchSchedules { get; set; }
        public DbSet<TournamentPlayer>        TournamentPlayers        { get; set; }
        public DbSet<TournamentGroup>         TournamentGroups         { get; set; }

        public GameOnContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TournamentPlayer>()
                        .HasKey(bc => new { bc.PlayerId, bc.TournamentId });
            modelBuilder.Entity<TournamentPlayer>()
                        .HasOne(bc => bc.Player)
                        .WithMany(b => b.Tournaments)
                        .HasForeignKey(bc => bc.PlayerId);
            modelBuilder.Entity<TournamentPlayer>()
                        .HasOne(bc => bc.Tournament)
                        .WithMany(c => c.Players)
                        .HasForeignKey(bc => bc.TournamentId);

            modelBuilder.Entity<TournamentGroupPlayer>()
                        .HasKey(bc => new { bc.PlayerId, bc.TournamentGroupId });
            modelBuilder.Entity<TournamentGroupPlayer>()
                        .HasOne(bc => bc.Player)
                        .WithMany(b => b.TournamentGroups)
                        .HasForeignKey(bc => bc.PlayerId);
            modelBuilder.Entity<TournamentGroupPlayer>()
                        .HasOne(bc => bc.TournamentGroup)
                        .WithMany(c => c.Players)
                        .HasForeignKey(bc => bc.TournamentGroupId);

        }
    }
}