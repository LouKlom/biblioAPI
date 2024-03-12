namespace bibliotheque.Models
{
    public class Media
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Reserved { get; set; }
        public string Edition {  get; set; }
        public string DateSortie { get; set; }
        public Auteur Auteur { get; set; }

    }
}