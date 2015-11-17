using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Concord.App.Annotations;

namespace Concord.App.Models
{
    public class SongModel : INotifyPropertyChanged
    {
        #region Properties

        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        private string _author;
        public string Author
        {
            get { return _author; }
            set
            {
                _author = value;
                OnPropertyChanged("Author");
            }
        }

        private DateTime _publishDate;
        public DateTime PublishDate
        {
            get { return _publishDate; }
            set
            {
                _publishDate = value;
                OnPropertyChanged("PublishDate");
            }
        }

        private string _album;
        public string Album
        {
            get { return _album; }
            set
            {
                _album = value;
                OnPropertyChanged("Album");
            }
        }

        //public ObservableCollection<LyricsModel> Lyrics { get; set; }

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        #endregion

        //public override string ToString()
        //{
        //    var text = string.Empty;
        //    var lastLineNumber = 0;

        //    foreach (var word in Lyrics.OrderBy(l=>l.Line).ThenBy(l=>l.Column).ToList())
        //    {
        //        if (lastLineNumber == 0)
        //        {
        //            lastLineNumber++;
        //            text = word.Word.Word;
        //        }
        //        else if (lastLineNumber == word.Line)
        //        {
        //            text = string.Format("{0} {1}", text, word.Word.Word);
        //        }
        //        else
        //        {
        //            text = string.Format("{0}{1}{2}", text, Environment.NewLine, word.Word.Word);
        //            lastLineNumber = word.Line;
        //        }
        //    }

        //    return text;
        //}

        public void Copy(SongModel source)
        {
            Id = source.Id;
            Title = source.Title;
            Author = source.Author;
            PublishDate = source.PublishDate;
            Album = source.Album;
            Text = source.Text;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}