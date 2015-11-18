using System;
using System.Collections.Generic;
using System.Linq;

namespace Concord.Entities
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime? PublishDate { get; set; }
        public string AlbumName { get; set; }
        public List<SongWord> SongWords { get; set; }


        private string _songText;
        public string SongText
        {
            get
            {
                if (!string.IsNullOrEmpty(_songText))
                    return _songText;

                _songText = string.Empty;
                var lastLineNumber = 0;

                foreach (var word in SongWords.OrderBy(l => l.Line).ThenBy(l => l.Column).ToList())
                {
                    if (lastLineNumber == 0)
                    {
                        lastLineNumber++;
                        _songText = word.Word.Text;
                    }
                    else if (lastLineNumber == word.Line)
                    {
                        _songText = $"{_songText} {word.Word.Text}";
                    }
                    else
                    {
                        _songText = $"{_songText}{Environment.NewLine}{word.Word.Text}";
                        lastLineNumber = word.Line;
                    }
                }
                
                return _songText;
            }
            set { _songText = value; }
        }

        public Song()
        {
            SongWords = new List<SongWord>();
        }
    }
}