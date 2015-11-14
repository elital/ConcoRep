namespace Concord.Entities
{
    public class Relation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Word FirstWord { get; set; }
        public Word SecondWord { get; set; }
    }
}