using System.Collections.Generic;
using Concord.Dal.WordEntity;

namespace Concord.Dal.SongWordEntity
{
    public class SongWordCreator
    {
        #region Fields names

        private const string IdText = "ID";
        private const string SongIdText = "SONG_ID";
        private const string WordLineText = "WORD_LINE";
        private const string WordColumnText = "WORD_COLUMN";
        private const string WordIdText = "WORD_ID";

        #endregion

        #region Statements

        private readonly string _createSongWordStatement = $"insert into SONG_WORDS(ID, SONG_ID, WORD_LINE, WORD_COLUMN, WORD_ID) " +
                                                           $"values(:{IdText}, :{SongIdText}, :{WordLineText}, :{WordColumnText}, :{WordIdText})";

        #endregion

        // TODO : Singleton or not?
        #region Singleton

        private static SongWordCreator _instance;
        public static SongWordCreator Instance
        {
            get { return _instance ?? (_instance = new SongWordCreator()); }
            set { _instance = value; }
        }

        private SongWordCreator() { }

        #endregion

        internal void Create(string wordText, int songId, int line, int column, bool commit)
        {
            if (string.IsNullOrEmpty(wordText))
                return;

            var word = new WordQuery().GetOrCreateWord(wordText, true, commit);
            var id = SequenceQuery.Instance.GetSongWordId();

            OracleDataLayer.Instance.DmlAction(_createSongWordStatement,
                new KeyValuePair<string, object>(IdText, id),
                new KeyValuePair<string, object>(SongIdText, songId),
                new KeyValuePair<string, object>(WordLineText, line),
                new KeyValuePair<string, object>(WordColumnText, column),
                new KeyValuePair<string, object>(WordIdText, word.Id));

            if (commit)
                OracleDataLayer.Instance.Commit();
        }
    }
}