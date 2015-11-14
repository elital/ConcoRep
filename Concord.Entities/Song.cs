using System;
using System.Collections.Generic;

namespace Concord.Entities
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PublishDate { get; set; }
        public string AlbumName { get; set; }
        public List<SongWord> SongWords { get; set; }

        public Song()
        {
            SongWords = new List<SongWord>();
        }
    }
}