namespace bibliotheque.Requests;

public class ReservationRequest
{
    public int? ClientId { get; set; }
    public int? MediaId { get; set; }
    public string? DateDebut { get; set; }
    public string? DateFin { get; set; }
}
