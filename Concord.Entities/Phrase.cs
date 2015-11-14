namespace Concord.Entities
{
    public class Phrase
    {
        public int Id { get; set; }
        public int WordSequence { get; set; }
        public int PhraseNumber { get; set; }
        public Word Word { get; set; }
    }
}