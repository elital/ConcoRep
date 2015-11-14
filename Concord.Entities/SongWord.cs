namespace Concord.Entities
{
    public class SongWord
    {
        public int Id { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public Word Word { get; set; }
    }
}