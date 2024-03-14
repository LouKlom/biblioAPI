namespace bibliotheque.Models
{
    public class MediaRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Reserved { get; set; }
        public string Edition {  get; set; }
        public string DateSortie { get; set; }
        public int AuteurId { get; set; }
    }
}
