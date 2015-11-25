namespace Concord.Entities
{
    public class Context
    {
        public string SongTitle { get; set; }
        public string Author { get; set; }
        public string Album { get; set; }
        public short MatchLineNumber { get; set; }
        public short MatchColumnNumber { get; set; }
        public string PreContextLine { get; set; }
        public string ContextLine { get; set; }
        public string PostContextLine { get; set; }
    }
}