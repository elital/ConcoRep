using System.ComponentModel;
using System.Runtime.CompilerServices;
using Concord.App.Annotations;

namespace Concord.App.Models
{
    public class WordModel : INotifyPropertyChanged
    {
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

        private string _word;
        public string Word
        {
            get { return _word; }
            set
            {
                _word = value;
                OnPropertyChanged("Word");
            }
        }

        private int _repetition;
        public int Repetition
        {
            get { return _repetition; }
            set
            {
                _repetition = value;
                OnPropertyChanged("Repetition");
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