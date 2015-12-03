using System.Collections.Generic;
using Concord.Dal.WordEntity;
using Concord.Entities;
using Oracle.ManagedDataAccess.Client;

namespace Concord.Dal.SongWordEntity
{
    public class SongWordQuery : QueryBase
    {
        #region Properties

        public int? SongId { get; set; }
        public int? Line { get; set; }
        public int? Column { get; set; }
        public int? WordId { get; set; }

        #endregion

        #region Fields names

        private const string IdText = "ID";
        private const string SongIdText = "SONG_ID";
        private const string WordLineText = "WORD_LINE";
        private const string WordColumnText = "WORD_COLUMN";
        private const string WordIdText = "WORD_ID";

        #endregion

        #region Statements

        private readonly string _getByIdStatement = $"select * " +
                                                    $"from   SONG_WORDS SW " +
                                                    $"where  SW.ID = :{IdText}";

        private readonly string _getStatement = $"select * " +
                                                $"from   SONG_WORDS ";

        #endregion

        public SongWord GetById(int id)
        {
            return OracleDataLayer.Instance.Select(ReadSongWord, _getByIdStatement, new KeyValuePair<string, object>(IdText, id));
        }

        public IEnumerable<SongWord> Get()
        {
            var statement = _getStatement;
            var parameters = new List<KeyValuePair<string, object>>();

            AddComparison(ref statement, SongIdText, SongId, parameters);
            AddComparison(ref statement, WordLineText, Line, parameters);
            AddComparison(ref statement, WordColumnText, Column, parameters);
            AddComparison(ref statement, WordIdText, WordId, parameters);

            return OracleDataLayer.Instance.Select(ReadSongWords, statement, parameters.ToArray());
        }


        private SongWord ReadSongWord(OracleDataReader reader)
        {
            if (!reader.Read())
                return null;

            return new SongWord
                {
                    Id = (int) reader[IdText],
                    Line = (short) reader[WordLineText],
                    Column = (short) reader[WordColumnText],
                    Word = new WordQuery().GetWordById((int) reader[WordIdText])
                };
        }

        private IEnumerable<SongWord> ReadSongWords(OracleDataReader reader)
        {
            var songWords = new List<SongWord>();

            SongWord songWord;

            while ((songWord = ReadSongWord(reader)) != null)
                songWords.Add(songWord);

            return songWords;
        }
    }
}