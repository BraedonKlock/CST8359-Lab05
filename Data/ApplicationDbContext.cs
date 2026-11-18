using Lab05WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab05WebApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }

    public DbSet<Attendee> Attendees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Attendee>()
            .HasOne(a => a.Event)
            .WithMany(e => e.Attendees)
            .HasForeignKey(a => a.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
