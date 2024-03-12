using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace bibliotheque.Models
{
    public class ReservationContext : DbContext
    {
        public ReservationContext(DbContextOptions<ReservationContext> options)
            : base(options)
        {
        }

        public DbSet<Reservation> Reservation { get; set; } = null!;
    }
}
