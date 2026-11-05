using Microsoft.EntityFrameworkCore;
using VoiceChat.Server.Models;

public class VoiceChatDbContext : DbContext
{
    
    public VoiceChatDbContext(
        DbContextOptions<VoiceChatDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomMember> RoomMember { get; set; }
    public DbSet<RoomMessage> RoomMessages { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=VoiceChatDb;Username=postgres;Password=password");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserSession>()
        .HasOne<User>()
        .WithMany()
        .HasForeignKey(x => x.UserId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<UserSession>()
            .HasIndex(s => s.Token)
            .IsUnique();

        modelBuilder.Entity<RoomMember>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<RoomMember>()
            .HasOne<Room>()
            .WithMany()
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RoomMember>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}