using System;
using System.Collections.Generic;
using Concord.Entities;
using Oracle.ManagedDataAccess.Client;

namespace Concord.Dal
{
    public class Query
    {
        private const string IdText = "ID";
        private const string WordText = "WORD";
        private const string RepetitionText = "REPETITION";
        private const string SongIdText = "SONG_ID";
        private const string TitleText = "TITLE";
        private const string AuthorText = "AUTHOR";
        private const string PublishDateText = "PUBLISH_DATE";
        private const string AlbumNameText = "ALBUM_NAME";
        private const string WordLineText = "WORD_LINE";
        private const string WordColumnText = "WORD_COLUMN";
        private const string WordIdText = "WORD_ID";

        //private const string SequenceNameText = "SequenceName";
        //private readonly string _getSequenceNextVal = $"select :{SequenceNameText}.nextval VAL from dual";
        private readonly string _getWordByWordTextStatement = $"select * from WORDS W where W.WORD = :{WordText}";
        private readonly string _getWordByIdStatement = $"select * from WORDS W where W.ID = :{IdText}";
        private readonly string _selectSongByTitleAndAuthor =
            $"select * from SONGS S where S.TITLE = :{TitleText} and S.AUTHOR = :{AuthorText}";
        private readonly string _selectSongWords = $"select * from SONG_WORDS SW where SW.SONG_ID = :{SongIdText}";

        private readonly string _getUniqueSongWord =
            $"select * from SONG_WORDS SW where SW.SONG_ID = :{SongIdText} and SW.WORD_LINE = :{WordLineText} and SW.WORD_COLUMN = :{WordColumnText}";
        private readonly string _insertIntoWordsStatement =
            $"insert into WORDS(ID, WORD, REPETITION) values(WORDS_S.NEXTVAL, :{WordText}, :{RepetitionText})";
        private readonly string _increaseWordRepetitionStatement =
            $"update WORDS W set W.REPETITION = W.REPETITION + 1 where W.ID = :{IdText}";

        #region Singleton

        private static Query _instance;
        public static Query Instance
        {
            get { return _instance ?? (_instance = new Query()); }
            set { _instance = value; }
        }

        private Query() {}

        #endregion

        #region WORDS

        public Word GetWordById(int id)
        {
            return OracleDataLayer.Instance.Select(ReadWord, _getWordByIdStatement,
                new KeyValuePair<string, object>(IdText, id));
        }

        public Word GetOrCreateWord(string text, bool increaseRepetition)
        {
            var word = OracleDataLayer.Instance.Select(ReadWord, _getWordByWordTextStatement,
                new KeyValuePair<string, object>(WordText, text));

            if (word == null)
            {
                var repetition = increaseRepetition ? 1 : 0;

                OracleDataLayer.Instance.DmlAction(_insertIntoWordsStatement,
                    new KeyValuePair<string, object>(WordText, text),
                    new KeyValuePair<string, object>(RepetitionText, repetition));

                word = OracleDataLayer.Instance.Select(ReadWord, _getWordByWordTextStatement,
                    new KeyValuePair<string, object>(WordText, text));
            }
            else if (increaseRepetition)
            {
                // TODO : increase repetition (update)
                OracleDataLayer.Instance.DmlAction(_increaseWordRepetitionStatement,
                    new KeyValuePair<string, object>(IdText, word.Id));
                word.Repetitions++;
            }

            return word;
        }

        private Word ReadWord(OracleDataReader reader)
        {
            if (!reader.Read())
                return null;

            return new Word
                {
                    Id = (int) reader[IdText],
                    Text = (string) reader[WordText],
                    Repetitions = (int) reader[RepetitionText]
                };
        }

        #endregion

        #region SONGS

        public Song GetSongById(int id)
        {
            return OracleDataLayer.Instance.Select<Song>(ReadSong, _selectSongByTitleAndAuthor,
                new KeyValuePair<string, object>(IdText, id));
        }

        public Song GetSong(string title, string author)
        {
            return OracleDataLayer.Instance.Select<Song>(ReadSong, _selectSongByTitleAndAuthor,
                new KeyValuePair<string, object>(TitleText, title),
                new KeyValuePair<string, object>(AuthorText, author));
        }

        private Song ReadSong(OracleDataReader reader)
        {
            if (!reader.Read())
                return null;

            var song = new Song
                {
                    Id = (int) reader[IdText],
                    Title = (string) reader[TitleText],
                    Author = (string) reader[AuthorText],
                    PublishDate = (DateTime) reader[PublishDateText],
                    AlbumName = (string) reader[AlbumNameText],
                    SongWords = new List<SongWord>()
                };

            OracleDataLayer.Instance.Select<List<SongWord>>((ireader) => ReadSongWords(ireader, song),
                _selectSongWords,
                new KeyValuePair<string, object>(SongIdText, song.Id));

            return song;
        }

        private List<SongWord> ReadSongWords(OracleDataReader reader, Song song)
        {
            while (reader.Read())
            {
                var songWord = new SongWord
                    {
                        Id = (int) reader[IdText],
                        Line = (short) reader[WordLineText],
                        Column = (short) reader[WordColumnText],
                        Word = Query.Instance.GetWordById((int) reader[WordIdText])
                    };

                song.SongWords.Add(songWord);
            }

            return song.SongWords;
        }

        #endregion

        #region SONG_WORDS

        public SongWord GetUniqueSongWord(int songId, int line, int column)
        {
            return OracleDataLayer.Instance.Select<SongWord>(ReadSongWord, _getUniqueSongWord,
                new KeyValuePair<string, object>(SongIdText, songId),
                new KeyValuePair<string, object>(WordLineText, line),
                new KeyValuePair<string, object>(WordColumnText, column));
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
                    Word = GetWordById((int) reader[WordIdText])
                };
        }

        #endregion
    }
}