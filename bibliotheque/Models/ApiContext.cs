using Microsoft.EntityFrameworkCore;

namespace bibliotheque.Models;

public class ApiContext : DbContext
{
    public ApiContext(DbContextOptions<ApiContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>()
            .HasIndex(p => new { p.Name, p.Mail })
            .IsUnique();
    }

    public DbSet<Auteur> Auteurs { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Media> Medias { get; set; } = null!;
    public DbSet<Reservation> Reservations { get; set; } = null!;
}
