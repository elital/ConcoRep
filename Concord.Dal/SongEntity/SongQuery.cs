using System;
using System.Collections.Generic;
using System.Linq;
using Concord.Dal.SongWordEntity;
using Concord.Entities;
using Oracle.ManagedDataAccess.Client;

namespace Concord.Dal.SongEntity
{
    public class SongQuery : QueryBase
    {
        #region Properties

        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime? PublishDate { get; set; }
        public string AlbumName { get; set; }
        public string FreeText { get; set; }

        #endregion

        #region Fields names

        private const string IdText = "ID";
        private const string TitleText = "TITLE";
        private const string AuthorText = "AUTHOR";
        private const string PublishDateText = "PUBLISH_DATE";
        private const string AlbumNameText = "ALBUM_NAME";
        
        #endregion

        #region Statements

        private readonly string _getByIdStatement = $"select * " +
                                                    $"from   SONGS S " +
                                                    $"where  S.ID = :{IdText}";
        private readonly string _getStatement = $"select * " +
                                                $"from   SONGS ";
        
        #endregion

        public Song GetById(int id)
        {
            return OracleDataLayer.Instance.Select(ReadSong, _getByIdStatement, new KeyValuePair<string, object>(IdText, id));
        }

        public Song SingleOrDefault()
        {
            return Get().SingleOrDefault();
        }

        public IEnumerable<Song> Get()
        {
            var statement = _getStatement;
            var parameters = new List<KeyValuePair<string, object>>();

            AddAndLikeComparison(ref statement, TitleText, Title, parameters);
            AddAndLikeComparison(ref statement, AuthorText, Author, parameters);
            AddComparison(ref statement, PublishDateText, PublishDate, parameters);
            AddAndLikeComparison(ref statement, AlbumNameText, AlbumName, parameters);
            AddContainsLikePhraseComparison(ref statement, FreeText, parameters);
            
            return OracleDataLayer.Instance.Select(ReadSongs, statement, parameters.ToArray());
        }

        private IEnumerable<Song> ReadSongs(OracleDataReader reader)
        {
            var songs = new List<Song>();

            Song song;

            while ((song = ReadSong(reader)) != null)
                songs.Add(song);

            return songs;
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

            song.SongWords.AddRange(new SongWordQuery {SongId = song.Id}.Get());

            return song;
        }
    }
}