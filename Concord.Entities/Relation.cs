using System.Collections.Generic;

namespace Concord.Entities
{
    public class Relation
    {
        public string Name { get; set; }
        
        public List<Pair> Pairs { get; set; }

        public Relation()
        {
            Pairs = new List<Pair>();
        }
    }

    public class Pair
    {
        public int Id { get; set; }
        public Word FirstWord { get; set; }
        public Word SecondWord { get; set; }
    }
}