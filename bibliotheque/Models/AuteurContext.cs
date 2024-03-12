using Microsoft.EntityFrameworkCore;

namespace bibliotheque.Models;

public class AuteurContext : DbContext
{
    public AuteurContext(DbContextOptions<AuteurContext> options)
        : base(options)
    {
    }

    public DbSet<Auteur> Auteur { get; set; } = null!;
}