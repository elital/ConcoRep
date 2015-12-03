namespace Concord.Entities
{
    public class SongStatistics
    {
        public int SongId { get; set; }
        public int WordsAmount { get; set; }
        public string LongestWord { get; set; }
        public int LongestWordLength { get; set; }
        public string ShortestWord { get; set; }
        public int ShortestWordLength { get; set; }
        public string MostRepeatedWord { get; set; }
        public int MostRepeatedWordRepetitions { get; set; }
    }
}