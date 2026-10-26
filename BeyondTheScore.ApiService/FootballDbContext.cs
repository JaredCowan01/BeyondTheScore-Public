using Microsoft.EntityFrameworkCore;

namespace BeyondTheScore.ApiService;

public partial class FootballDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    
    public FootballDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public FootballDbContext(DbContextOptions<FootballDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<AttackSequence> AttackSequences { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<MatchEvent> MatchEvents { get; set; }

    public virtual DbSet<MatchInsight> MatchInsights { get; set; }

    public virtual DbSet<PersonalisedCommentary> PersonalisedCommentaries { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Possession> Possessions { get; set; }

    public virtual DbSet<Team> Teams { get; set; }
    
    public virtual DbSet<UserPromptUsage> UserPromptUsages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => 
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AttackSequence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AttackSe__3214EC0792C6E192");

            entity.Property(e => e.ExpectedGoals).HasColumnType("decimal(9, 4)");
            entity.Property(e => e.Outcome).HasMaxLength(50);

            entity.HasOne(d => d.Match).WithMany(p => p.AttackSequences)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AttackSequences_Match");

            entity.HasOne(d => d.Team).WithMany(p => p.AttackSequences)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AttackSequences_Team");
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Matches__3214EC07A09B3A9E");

            entity.HasIndex(e => e.MatchIdentifier, "UQ__Matches__0E042C226D5B70A5").IsUnique();

            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.MatchIdentifier).HasMaxLength(50);

            entity.HasOne(d => d.AwayTeam).WithMany(p => p.MatchAwayTeams)
                .HasForeignKey(d => d.AwayTeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Matches_AwayTeam");

            entity.HasOne(d => d.HomeTeam).WithMany(p => p.MatchHomeTeams)
                .HasForeignKey(d => d.HomeTeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Matches_HomeTeam");
        });

        modelBuilder.Entity<MatchEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MatchEve__3214EC076DF48749");

            entity.HasIndex(e => e.AttackSequenceId, "IX_MatchEvents_AttackSequence");

            entity.HasIndex(e => e.EventType, "IX_MatchEvents_EventType");

            entity.HasIndex(e => e.MatchId, "IX_MatchEvents_MatchId");

            entity.HasIndex(e => e.Minute, "IX_MatchEvents_Minute");

            entity.HasIndex(e => e.PlayerId, "IX_MatchEvents_PlayerId");

            entity.HasIndex(e => e.PossessionId, "IX_MatchEvents_Possession");

            entity.HasIndex(e => e.TeamId, "IX_MatchEvents_TeamId");

            entity.Property(e => e.BallSpeed).HasColumnType("decimal(9, 2)");
            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.EndXcoord)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("EndXCoord");
            entity.Property(e => e.EndYcoord)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("EndYCoord");
            entity.Property(e => e.EventType).HasMaxLength(50);
            entity.Property(e => e.ExpectedGoals).HasColumnType("decimal(9, 4)");
            entity.Property(e => e.MomentumDelta).HasColumnType("decimal(9, 2)");
            entity.Property(e => e.Outcome).HasMaxLength(50);
            entity.Property(e => e.PassDifficulty).HasColumnType("decimal(9, 2)");
            entity.Property(e => e.PassDistance).HasColumnType("decimal(9, 2)");
            entity.Property(e => e.PlayerSpeed).HasColumnType("decimal(9, 2)");
            entity.Property(e => e.StoryArc).HasMaxLength(50);
            entity.Property(e => e.Xcoord)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("XCoord");
            entity.Property(e => e.Ycoord)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("YCoord");

            entity.HasOne(d => d.Match).WithMany(p => p.MatchEvents)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchEvents_Match");

            entity.HasOne(d => d.Player).WithMany(p => p.MatchEvents)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchEvents_Player");

            entity.HasOne(d => d.Team).WithMany(p => p.MatchEvents)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchEvents_Team");
        });

        modelBuilder.Entity<MatchInsight>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MatchIns__3214EC071295C688");

            entity.Property(e => e.AgentName).HasMaxLength(50);
            entity.Property(e => e.ConfidenceScore).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.InsightType).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(300);

            entity.HasOne(d => d.Match).WithMany(p => p.MatchInsights)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchInsights_Match");
        });

        modelBuilder.Entity<PersonalisedCommentary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Personal__3214EC0787A96084");

            entity.ToTable("PersonalisedCommentary");

            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.GeneratedBy).HasMaxLength(50);
            entity.Property(e => e.LanguageCode).HasMaxLength(10);
            entity.Property(e => e.TargetAudience).HasMaxLength(50);

            entity.HasOne(d => d.Match).WithMany(p => p.PersonalisedCommentaries)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonalisedCommentary_Match");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Players__3214EC07E38205FA");

            entity.HasIndex(e => new { e.TeamId, e.Name }, "UQ_Player_Team").IsUnique();

            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Team).WithMany(p => p.Players)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Players_Teams");
        });

        modelBuilder.Entity<Possession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Possessi__3214EC073E24D665");

            entity.HasOne(d => d.Match).WithMany(p => p.Possessions)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Possessions_Match");

            entity.HasOne(d => d.Team).WithMany(p => p.Possessions)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Possessions_Team");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Teams__3214EC079962C1F9");

            entity.HasIndex(e => e.Name, "UQ__Teams__737584F689330D2C").IsUnique();

            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Name).HasMaxLength(100);
        });
        
        modelBuilder.Entity<UserPromptUsage>()
            .HasIndex(u => u.Auth0UserId)
            .IsUnique();
        
        modelBuilder.Entity<UserPromptUsage>()
            .Property(u => u.PromptCount)
            .HasDefaultValue(0);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
