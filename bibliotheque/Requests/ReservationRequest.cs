namespace bibliotheque.Requests;

public class ReservationRequest
{
    public int? ClientId { get; set; }
    public int? MediaId { get; set; }
    public DateTime? DateDebut { get; set; }
    public DateTime? DateFin { get; set; }
}
