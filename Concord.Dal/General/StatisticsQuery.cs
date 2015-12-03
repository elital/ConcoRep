using System.Collections.Generic;
using Concord.Entities;
using Oracle.ManagedDataAccess.Client;

namespace Concord.Dal.General
{
    public class StatisticsQuery : QueryBase
    {
        private struct WordAndNumber
        {
            public string Word { get; set; }
            public int NumericField { get; set; }
        }

        #region Fields names

        private const string SongIdText = "SONG_ID";
        private const string WordText = "WORD";
        private const string WordLengthText = "WORD_LENGTH";
        private const string WordCountText = "WORD_COUNT";

        #endregion

        #region Statements

        private readonly string _getSongStatWordsCountStatement = $"select count(1) {WordCountText} " +
                                                                  $"from   SONG_WORDS SW " +
                                                                  $"where  SW.SONG_ID = :{SongIdText} ";

        private readonly string _getSongStatMaxLengthWordStatement = $"select W.WORD " +
                                                                     $"     , length(W.WORD)    {WordLengthText} " +
                                                                     $"from   WORDS      W " +
                                                                     $"     , SONG_WORDS SW " +
                                                                     $"where  SW.WORD_ID = W.ID " +
                                                                     $"and    SW.SONG_ID = :{SongIdText} " +
                                                                     $"and    length(W.WORD) = (select max(length(WI.WORD)) " +
                                                                     $"                         from   WORDS WI " +
                                                                     $"                              , SONG_WORDS SWI " +
                                                                     $"                         where  SWI.WORD_ID = WI.ID " +
                                                                     $"                         and    SWI.SONG_ID = SW.SONG_ID) " +
                                                                     $"and    rownum = 1 ";

        private readonly string _getSongStatMinLengthWordStatement = $"select W.WORD            {WordText} " +
                                                                     $"     , length(W.WORD)    {WordLengthText} " +
                                                                     $"from   WORDS      W " +
                                                                     $"     , SONG_WORDS SW " +
                                                                     $"where  SW.WORD_ID = W.ID " +
                                                                     $"and    SW.SONG_ID = :{SongIdText} " +
                                                                     $"and    length(W.WORD) = (select min(length(WI.WORD)) " +
                                                                     $"                         from   WORDS WI " +
                                                                     $"                              , SONG_WORDS SWI " +
                                                                     $"                         where  SWI.WORD_ID = WI.ID " +
                                                                     $"                         and    SWI.SONG_ID = SW.SONG_ID) " +
                                                                     $"and    rownum = 1 ";

        private readonly string _getSongStatMostRepeatedWordStatement = $"select * " +
                                                                        $"from   (select (select W.WORD " +
                                                                        $"                from   WORDS W " +
                                                                        $"                where  W.ID = SW.WORD_ID) {WordText} " +
                                                                        $"             , count(1)                   {WordCountText} " +
                                                                        $"        from   SONG_WORDS SW " +
                                                                        $"        where  SW.SONG_ID = :{SongIdText} " +
                                                                        $"        group by SW.WORD_ID " +
                                                                        $"        having count(1) = (select max(count(1)) " +
                                                                        $"                           from   SONG_WORDS SWI " +
                                                                        $"                           where  SWI.SONG_ID = :{SongIdText} " +
                                                                        $"                           group by SWI.WORD_ID)) " +
                                                                        $"where  rownum = 1 ";

        #endregion

        #region Singleton

        private static StatisticsQuery _instance;
        public static StatisticsQuery Instance => _instance ?? (_instance = new StatisticsQuery());

        private StatisticsQuery() { }

        #endregion

        public SongStatistics GetSongStatistics(int songId)
        {
            var songCount = OracleDataLayer.Instance.Select(ReadSongStatWordCount, _getSongStatWordsCountStatement, new KeyValuePair<string, object>(SongIdText, songId));
            var longestWord = FetchWordAndNumber(_getSongStatMaxLengthWordStatement, WordLengthText, songId);
            var shortestWord = FetchWordAndNumber(_getSongStatMinLengthWordStatement, WordLengthText, songId);
            var mostRepeatedWord = FetchWordAndNumber(_getSongStatMostRepeatedWordStatement, WordCountText, songId);
            
            return new SongStatistics
                {
                    SongId = songId,
                    WordsAmount = songCount,
                    LongestWord = longestWord.Word,
                    LongestWordLength = longestWord.NumericField,
                    ShortestWord = shortestWord.Word,
                    ShortestWordLength = shortestWord.NumericField,
                    MostRepeatedWord = mostRepeatedWord.Word,
                    MostRepeatedWordRepetitions = mostRepeatedWord.NumericField
                };
        }

        private int ReadSongStatWordCount(OracleDataReader reader)
        {
            if (!reader.Read())
                return 0;

            return (int) (decimal) reader[WordCountText];
        }

        private WordAndNumber FetchWordAndNumber(string statement, string numericColumn, int songId)
        {
            var songIdParameter = new KeyValuePair<string, object>(SongIdText, songId);
            return OracleDataLayer.Instance.Select(reader => ReadWordAndNumber(reader, numericColumn), statement, songIdParameter);
        }

        private WordAndNumber ReadWordAndNumber(OracleDataReader reader, string numericColumn)
        {
            if (!reader.Read())
                return new WordAndNumber {Word = string.Empty, NumericField = 0};
            
            return new WordAndNumber
                {
                    Word = (string) reader[WordText],
                    NumericField = (int) (decimal) reader[numericColumn]
                };
        }

    }
}