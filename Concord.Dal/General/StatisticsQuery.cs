using System.Collections.Generic;
using Concord.Entities;
using Oracle.ManagedDataAccess.Client;

namespace Concord.Dal.General
{
    public class StatisticsQuery : QueryBase
    {
        private struct TextAndNumber
        {
            public string TextField { get; set; }
            public int NumericField { get; set; }
        }

        #region Fields names

        private const string SongIdText = "SONG_ID";
        private const string WordText = "WORD";
        private const string WordLengthText = "WORD_LENGTH";
        private const string WordCountText = "WORD_COUNT";
        private const string SongsCountText = "SONGS_COUNT";
        private const string SongTitleText = "TITLE";
        private const string RepetitionText = "REPETITION";
        private const string TotalWordsCount = "TOTAL_WORDS";

        #endregion

        #region Statements

        #region Song

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

        #region System

        private readonly string _getSysStatTotalSongsCountStatement = $"select count(1) {SongsCountText} " +
                                                                      $"from   SONGS ";

        private readonly string _getSysStatTotalSongsWordsCountStatement = $"select case when sum(W.REPETITION) is null then 0 else sum(W.REPETITION) end {TotalWordsCount} " +
                                                                           $"from   WORDS W ";

        private readonly string _getSysStatTotalDifferentWordsCountStatement = $"select count(1) {TotalWordsCount} " +
                                                                               $"from   WORDS W " +
                                                                               $"where  W.REPETITION > 0 ";
        
        private readonly string _sysStatGetLongestSongStatement = $"select S.TITLE " +
                                                                  $"     , count(1)   {WordCountText} " +
                                                                  $"from   SONGS      S " +
                                                                  $"     , SONG_WORDS SW " +
                                                                  $"where  S.ID = SW.SONG_ID " +
                                                                  $"group by S.ID, S.TITLE " +
                                                                  $"having count(1) = (select max(count(1)) " +
                                                                  $"                   from   SONG_WORDS SW " +
                                                                  $"                   group by SW.SONG_ID) ";

        private readonly string _sysStatGetShortestSongStatement = $"select S.TITLE " +
                                                                   $"     , count(1)   {WordCountText} " +
                                                                   $"from   SONGS      S " +
                                                                   $"     , SONG_WORDS SW " +
                                                                   $"where  S.ID = SW.SONG_ID " +
                                                                   $"group by S.ID, S.TITLE " +
                                                                   $"having count(1) = (select min(count(1)) " +
                                                                   $"                   from   SONG_WORDS SW " +
                                                                   $"                   group by SW.SONG_ID) ";

        private readonly string _sysStatGetMostRepeatedWordStatement = "select W.WORD " +
                                                                       "     , W.REPETITION " +
                                                                       "from   WORDS W " +
                                                                       "where  W.REPETITION = (select max(WI.REPETITION) " +
                                                                       "                       from   WORDS WI " +
                                                                       "                       where  WI.REPETITION > 0) " +
                                                                       "and    rownum = 1 ";

        private readonly string _sysStatGetLongestWordStatement = $"select W.WORD " +
                                                                  $"     , length(W.WORD)   {WordLengthText} " +
                                                                  $"from   WORDS W " +
                                                                  $"where  W.REPETITION > 0 " +
                                                                  $"and    length(W.WORD) = (select max(length(WI.WORD)) " +
                                                                  $"                         from   WORDS WI " +
                                                                  $"                         where  WI.REPETITION > 0) " +
                                                                  $"and    rownum = 1 ";

        private readonly string _sysStatGetShortestWordStatement = $"select W.WORD " +
                                                                   $"     , length(W.WORD)   {WordLengthText} " +
                                                                   $"from   WORDS W " +
                                                                   $"where  W.REPETITION > 0 " +
                                                                   $"and    length(W.WORD) = (select min(length(WI.WORD)) " +
                                                                   $"                         from   WORDS WI " +
                                                                   $"                         where  WI.REPETITION > 0) " +
                                                                   $"and    rownum = 1 ";
        
        #endregion

        #endregion

        #region Singleton

        private static StatisticsQuery _instance;
        public static StatisticsQuery Instance => _instance ?? (_instance = new StatisticsQuery());

        private StatisticsQuery() { }

        #endregion

        public SongStatistics GetSongStatistics(int songId)
        {
            var songCount = OracleDataLayer.Instance.Select(reader => ReadNumericField(reader, WordCountText),
                _getSongStatWordsCountStatement,
                new KeyValuePair<string, object>(SongIdText, songId));

            var longestWord = FetchTextAndNumber(_getSongStatMaxLengthWordStatement, WordText, WordLengthText, songId);
            var shortestWord = FetchTextAndNumber(_getSongStatMinLengthWordStatement, WordText, WordLengthText, songId);
            var mostRepeatedWord = FetchTextAndNumber(_getSongStatMostRepeatedWordStatement, WordText, WordCountText, songId);
            
            return new SongStatistics
                {
                    SongId = songId,
                    WordsAmount = songCount,
                    LongestWord = longestWord.TextField,
                    LongestWordLength = longestWord.NumericField,
                    ShortestWord = shortestWord.TextField,
                    ShortestWordLength = shortestWord.NumericField,
                    MostRepeatedWord = mostRepeatedWord.TextField,
                    MostRepeatedWordRepetitions = mostRepeatedWord.NumericField
                };
        }

        public SystemStatistics GetSystemStatistics()
        {
            var songsCount = OracleDataLayer.Instance.Select(reader => ReadNumericField(reader, SongsCountText), _getSysStatTotalSongsCountStatement);
            var totalSystemWords = OracleDataLayer.Instance.Select(reader => ReadNumericField(reader, TotalWordsCount), _getSysStatTotalSongsWordsCountStatement);
            var systemDifferentWords = OracleDataLayer.Instance.Select(reader => ReadNumericField(reader, TotalWordsCount), _getSysStatTotalDifferentWordsCountStatement);

            var longestSong = FetchTextAndNumber(_sysStatGetLongestSongStatement, SongTitleText, WordCountText);
            var shortestSong = FetchTextAndNumber(_sysStatGetShortestSongStatement, SongTitleText, WordCountText);
            var mostRepeatedWord = FetchTextAndNumber(_sysStatGetMostRepeatedWordStatement, WordText, RepetitionText);
            var longestWord = FetchTextAndNumber(_sysStatGetLongestWordStatement, WordText, WordLengthText);
            var shortestWord = FetchTextAndNumber(_sysStatGetShortestWordStatement, WordText, WordLengthText);

            return new SystemStatistics
                {
                    TotalSongsAmount = songsCount,
                    TotalSystemSongsWordsAmount = totalSystemWords,
                    SystemDifferentSongsWordsAmount = systemDifferentWords,
                    LongestSongName = longestSong.TextField,
                    LongestSongWordsAmount = longestSong.NumericField,
                    ShortestSongName = shortestSong.TextField,
                    ShortestSongWordsAmount = shortestSong.NumericField,
                    MostRepeatedWord = mostRepeatedWord.TextField,
                    MostRepeatedWordRepetition = mostRepeatedWord.NumericField,
                    LongestWord = longestWord.TextField,
                    LongestWordLength = longestWord.NumericField,
                    ShortestWord = shortestWord.TextField,
                    ShortestWordLength = shortestWord.NumericField
                };
        }

        private int ReadNumericField(OracleDataReader reader, string columnName)
        {
            if (!reader.Read())
                return 0;

            return (int) (decimal) reader[columnName];
        }

        private TextAndNumber FetchTextAndNumber(string statement, string textColumn, string numericColumn, int? songId = null)
        {
            var parameters = songId.HasValue
                ? new[] {new KeyValuePair<string, object>(SongIdText, songId.Value)}
                : null;

            return OracleDataLayer.Instance.Select(reader => ReadWordAndNumber(reader, textColumn, numericColumn), statement, parameters);
        }

        private TextAndNumber ReadWordAndNumber(OracleDataReader reader, string textColumn, string numericColumn)
        {
            if (!reader.Read())
                return new TextAndNumber {TextField = string.Empty, NumericField = 0};

            var numeric = reader[numericColumn];
            var number = numeric is decimal ? (int) (decimal) numeric : (int) numeric;

            return new TextAndNumber
                {
                    TextField = (string) reader[textColumn],
                    NumericField = number
                };
        }
    }
}