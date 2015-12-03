using System;
using System.Collections.Generic;
using Concord.Dal.SongWordEntity;
using Concord.Entities;

namespace Concord.Dal.SongEntity
{
    public class SongCreator
    {
        #region Fields names

        private const string IdText = "ID";
        private const string TitleText = "TITLE";
        private const string AuthorText = "AUTHOR";
        private const string PublishDateText = "PUBLISH_DATE";
        private const string AlbumNameText = "ALBUM_NAME";

        #endregion

        #region Statements

        private readonly string _createSongStatement = $"insert into SONGS(ID, TITLE, AUTHOR, PUBLISH_DATE, ALBUM_NAME) " +
                                                       $"values(:{IdText}, :{TitleText}, :{AuthorText}, :{PublishDateText}, :{AlbumNameText})";

        #endregion

        // TODO : Singleton or not?
        #region Singleton

        private static SongCreator _instance;
        public static SongCreator Instance
        {
            get { return _instance ?? (_instance = new SongCreator()); }
            set { _instance = value; }
        }

        private SongCreator() {}

        #endregion
        
        public int Create(Song inputSong)
        {
            ValidateParameters(inputSong);

            var id = SequenceQuery.Instance.GetSongId();

            OracleDataLayer.Instance.DmlAction(_createSongStatement,
                new KeyValuePair<string, object>(IdText, id),
                new KeyValuePair<string, object>(TitleText, inputSong.Title),
                new KeyValuePair<string, object>(AuthorText, inputSong.Author),
                new KeyValuePair<string, object>(PublishDateText, inputSong.PublishDate),
                new KeyValuePair<string, object>(AlbumNameText, inputSong.AlbumName));

            BuildSongWords(id, inputSong.SongText);

            OracleDataLayer.Instance.Commit();

            return id;
        }

        private void ValidateParameters(Song inputSong)
        {
            if (string.IsNullOrEmpty(inputSong.Title))
                throw new ArgumentNullException(Utils.GetMemberName(() => inputSong.Title));

            if (string.IsNullOrEmpty(inputSong.Author))
                throw new ArgumentNullException(Utils.GetMemberName(() => inputSong.Author));

            if (string.IsNullOrEmpty(inputSong.AlbumName))
                throw new ArgumentNullException(Utils.GetMemberName(() => inputSong.AlbumName));

            if (!inputSong.PublishDate.HasValue)
                throw new ArgumentNullException(Utils.GetMemberName(() => inputSong.PublishDate));
        }

        private void BuildSongWords(int songId, string songText)
        {
            var lines = songText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var currentLine = 0;

            foreach (var line in lines)
            {
                currentLine++;
                // TODO : create array in some global location and refer to signs: '?', ':', '!', '.', ',', '(', ')', '-'
                var columns = line.Split(' ');
                var currentColumn = 0;

                foreach (var currentWord in columns)
                    SongWordCreator.Instance.Create(currentWord, songId, currentLine, ++currentColumn, false);
            }
        }
    }
}