using System.ComponentModel;
using System.Runtime.CompilerServices;
using Concord.App.Annotations;

namespace Concord.App.Models
{
    public class ContextModel : INotifyPropertyChanged
    {
        private string _songTitle;
        public string SongTitle
        {
            get { return _songTitle; }
            set
            {
                _songTitle = value;
                OnPropertyChanged("SongTitle");
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

        private int _contextLineNumber;
        public int ContextLineNumber
        {
            get { return _contextLineNumber; }
            set
            {
                _contextLineNumber = value;
                OnPropertyChanged("ContextLineNumber");
            }
        }

        private int _contextColumnNumber;
        public int ContextColumnNumber
        {
            get { return _contextColumnNumber; }
            set
            {
                _contextColumnNumber = value;
                OnPropertyChanged("ContextColumnNumber");
            }
        }

        private string _contextLine1;
        public string ContextLine1
        {
            get { return _contextLine1; }
            set
            {
                _contextLine1 = value;
                OnPropertyChanged("ContextLine1");
            }
        }

        private string _contextLine2;
        public string ContextLine2
        {
            get { return _contextLine2; }
            set
            {
                _contextLine2 = value;
                OnPropertyChanged("ContextLine2");
            }
        }

        private string _contextLine3;
        public string ContextLine3
        {
            get { return _contextLine3; }
            set
            {
                _contextLine3 = value;
                OnPropertyChanged("ContextLine3");
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