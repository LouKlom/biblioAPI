using Microsoft.EntityFrameworkCore;

namespace bibliotheque.Models;

public class ApiContext : DbContext
{
    public ApiContext(DbContextOptions<ApiContext> options)
        : base(options)
    {
    }

    public DbSet<Auteur> Auteurs { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Media> Medias { get; set; } = null!;
    public DbSet<Reservation> Reservations { get; set; } = null!;
}
