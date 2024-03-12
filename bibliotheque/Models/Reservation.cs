namespace bibliotheque.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int IdClient { get; set; }
        public int IdMedias { get; set; }
        public string? DateDebut { get; set; }

    }
}
