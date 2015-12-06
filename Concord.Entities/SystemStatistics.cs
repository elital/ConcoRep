namespace Concord.Entities
{
    public class SystemStatistics
    {
        public int TotalSongsAmount { get; set; }
        public int TotalSystemSongsWordsAmount { get; set; }
        public int SystemDifferentSongsWordsAmount { get; set; }
        public string LongestSongName { get; set; }
        public int LongestSongWordsAmount { get; set; }
        public string ShortestSongName { get; set; }
        public int ShortestSongWordsAmount { get; set; }
        public string MostRepeatedWord { get; set; }
        public int MostRepeatedWordRepetition { get; set; }
        public string LongestWord { get; set; }
        public int LongestWordLength { get; set; }
        public string ShortestWord { get; set; }
        public int ShortestWordLength { get; set; }
    }
}