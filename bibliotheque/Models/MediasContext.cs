using bibliotheque.Models;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque.Models;

public class MediasContext : DbContext
{
    public MediasContext(DbContextOptions<MediasContext> options)
        : base(options)
    {
    }

    public DbSet<Medias> Medias { get; set; } = null!;
}