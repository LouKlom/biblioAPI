namespace bibliotheque.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public Client Client { get; set; }
        public Media Media { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public bool Rendu { get; set; }
    }
}
