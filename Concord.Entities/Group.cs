using System.Collections.Generic;

namespace Concord.Entities
{
    public class Group
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public List<Word> Words { get; set; }

        public Group()
        {
            Words = new List<Word>();
        }
    }
}