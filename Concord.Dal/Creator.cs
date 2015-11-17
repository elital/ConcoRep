using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Concord.Entities;
using Oracle.ManagedDataAccess.Client;

namespace Concord.Dal
{
    public class Creator
    {
        #region Statements

        private const string SongIdText = "SONG_ID";
        private const string TitleText = "TITLE";
        private const string AuthorText = "AUTHOR";
        private const string PublishDateText = "PUBLISH_DATE";
        private const string AlbumNameText = "ALBUM_NAME";
        private const string WordLineText = "WORD_LINE";
        private const string WordColumnText = "WORD_COLUMN";
        private const string WordIdText = "WORD_ID";
        
        private readonly string _createSongStatement =
            $"insert into SONGS(ID, TITLE, AUTHOR, PUBLISH_DATE, ALBUM_NAME) values(SONGS_S.NEXTVAL, :{TitleText}, :{AuthorText}, :{PublishDateText}, :{AlbumNameText})";

        private readonly string _createSongWordStatement =
            $"insert into SONG_WORDS(ID, SONG_ID, WORD_LINE, WORD_COLUMN, WORD_ID) values(SONG_WORDS_S.NEXTVAL," +
            $" :{SongIdText}, :{WordLineText}, :{WordColumnText}, :{WordIdText})";

        #endregion

        #region Singleton

        private static Creator _instance;
        public static Creator Instance
        {
            get { return _instance ?? (_instance = new Creator()); }
            set { _instance = value; }
        }

        private Creator() {}

        #endregion

        public Song CreateSong(Song inputSong)
        {
            var publishDate = $"to_date('{inputSong.PublishDate.ToShortDateString()}', 'dd/mm/yyyy')";

            var rowsinserted = OracleDataLayer.Instance.DmlAction(_createSongStatement,
                new KeyValuePair<string, object>(TitleText, inputSong.Title),
                new KeyValuePair<string, object>(AuthorText, inputSong.Author),
                new KeyValuePair<string, object>(PublishDateText, inputSong.PublishDate),
                new KeyValuePair<string, object>(AlbumNameText, inputSong.AlbumName));

            var song = Query.Instance.GetSong(inputSong.Title, inputSong.Author);
            song.SongText = inputSong.SongText;

            BuildSongWords(song);

            song = Query.Instance.GetSong(inputSong.Title, inputSong.Author);

            return song;
        }

        private void BuildSongWords(Song song)
        {
            if (string.IsNullOrEmpty(song.SongText) || song.SongWords.Any())
                return;

            var lines = song.SongText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var currentLine = 0;

            foreach (var line in lines)
            {
                currentLine++;
                var columns = line.Split(' ');
                var currentColumn = 0;

                foreach (var currentWord in columns)
                {
                    currentColumn++;
                    var word = Query.Instance.GetOrCreateWord(currentWord, true);

                    OracleDataLayer.Instance.DmlAction(_createSongWordStatement,
                        new KeyValuePair<string, object>(SongIdText, song.Id.ToString()),
                        new KeyValuePair<string, object>(WordLineText, currentLine),
                        new KeyValuePair<string, object>(WordColumnText, currentColumn),
                        new KeyValuePair<string, object>(WordIdText, word.Id));

                    var songWord = Query.Instance.GetUniqueSongWord(song.Id, currentLine, currentColumn);
                    song.SongWords.Add(songWord);
                }
            }
        }
    }
}