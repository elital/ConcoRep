using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Concord.App.Annotations;

namespace Concord.App.Models
{
    public class PhraseModel : INotifyPropertyChanged
    {
        public ObservableCollection<PhraseWordModel> Words { get; set; }

        public override string ToString()
        {
            var phrase = string.Empty;

            foreach (var word in Words.OrderBy(ph=>ph.WordSequence).ToList())
            {
                if (string.IsNullOrEmpty(phrase))
                    phrase = word.Word.Word;
                else
                    phrase = string.Format("{0} {1}", phrase, word.Word.Word);
            }

            return phrase;
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

    public class PhraseWordModel : INotifyPropertyChanged
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

        private int _wordSequence;
        public int WordSequence
        {
            get { return _wordSequence; }
            set
            {
                _wordSequence = value;
                OnPropertyChanged("WordSequence");
            }
        }

        private int _phraseNumber;
        public int PhraseNumber
        {
            get { return _phraseNumber; }
            set
            {
                _phraseNumber = value;
                OnPropertyChanged("PhraseNumber");
            }
        }

        private WordModel _word;
        public WordModel Word
        {
            get { return _word; }
            set
            {
                _word = value;
                OnPropertyChanged("Word");
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