namespace bibliotheque.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public Client? Client { get; set; }
        public Media? Media { get; set; }
        public string? DateDebut { get; set; }
    }
}
