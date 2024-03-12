namespace bibliotheque.Models
{
    public class Medias
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Reserved { get; set; }
        public string Edition {  get; set; }
        public string DateSortie { get; set; }
        public int Auteur_id { get; set; }

    }
}