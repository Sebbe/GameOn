using Microsoft.EntityFrameworkCore;

namespace GameOn.Web.Entities
{
    public class GameOnContext : DbContext
    {
        public DbSet<AdminConfiguration> AdminConfigurations { get; set; }
        public DbSet<Player>             Players             { get; set; }
        public DbSet<RankHistory>        RankHistory         { get; set; }
        public DbSet<Match>              Matches             { get; set; }
        public DbSet<MatchSet>           MatchSets           { get; set; }
        public DbSet<Tournament>         Tournaments         { get; set; }
        public DbSet<Team> Teams { get; set; }

        public DbSet<TournamentMatchSchedule> TournamentMatchSchedules { get; set; }
        public DbSet<TournamentTeam>        TournamentTeams        { get; set; }
        public DbSet<TournamentGroup>         TournamentGroups         { get; set; }

        public GameOnContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TournamentTeam>()
                        .HasKey(bc => new {bc.TeamId, bc.TournamentId});
            modelBuilder.Entity<TournamentTeam>()
                        .HasOne(bc => bc.Team)
                        .WithMany(b => b.Tournaments)
                        .HasForeignKey(bc => bc.TeamId);
            modelBuilder.Entity<TournamentTeam>()
                        .HasOne(bc => bc.Tournament)
                        .WithMany(c => c.Teams)
                        .HasForeignKey(bc => bc.TournamentId);

            modelBuilder.Entity<TournamentGroupTeam>()
                        .HasKey(bc => new {bc.TeamId, bc.TournamentGroupId});
            modelBuilder.Entity<TournamentGroupTeam>()
                        .HasOne(bc => bc.Team)
                        .WithMany(b => b.TournamentGroups)
                        .HasForeignKey(bc => bc.TeamId);
            modelBuilder.Entity<TournamentGroupTeam>()
                        .HasOne(bc => bc.TournamentGroup)
                        .WithMany(c => c.Teams)
                        .HasForeignKey(bc => bc.TournamentGroupId);
        }
    }
}