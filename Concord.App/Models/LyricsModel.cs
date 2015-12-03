using System.ComponentModel;
using System.Runtime.CompilerServices;
using Concord.App.Annotations;

namespace Concord.App.Models
{
    // TODO : REMOVE

    public class LyricsModel : INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private int _line;
        public int Line
        {
            get { return _line; }
            set
            {
                _line = value;
                OnPropertyChanged(nameof(Line));
            }
        }

        private int _column;
        public int Column
        {
            get { return _column; }
            set
            {
                _column = value;
                OnPropertyChanged(nameof(Column));
            }
        }

        private WordModel _word;
        public WordModel Word
        {
            get { return _word; }
            set
            {
                _word = value;
                OnPropertyChanged(nameof(Word));
            }
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