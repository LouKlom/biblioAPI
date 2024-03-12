using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace bibliotheque.Models
{
    public class ClientContext : DbContext
    {
        public ClientContext(DbContextOptions<ClientContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Client { get; set; } = null!;
    }
}
