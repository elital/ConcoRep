using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Concord.App.Annotations;

namespace Concord.App.Models
{
    public class RelationModel : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public ObservableCollection<PairModel> Pairs { get; set; }

        public override string ToString()
        {
            return Name;
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

    public class PairModel : INotifyPropertyChanged
    {
        private int? _id;
        public int? Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private WordModel _firstWord;
        public WordModel FirstWord
        {
            get { return _firstWord; }
            set
            {
                _firstWord = value;
                OnPropertyChanged(nameof(FirstWord));
            }
        }

        private WordModel _secondWord;
        public WordModel SecondWord
        {
            get { return _secondWord; }
            set
            {
                _secondWord = value;
                OnPropertyChanged(nameof(SecondWord));
            }
        }

        public override string ToString()
        {
            return $"{FirstWord.Word} - {SecondWord.Word}";
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

    public class NewRelationModel : INotifyPropertyChanged
    {
        private string _relationName;
        public string RelationName
        {
            get { return _relationName; }
            set
            {
                _relationName = value;
                OnPropertyChanged(nameof(RelationName));
            }
        }

        private string _firstWord;
        public string FirstWord
        {
            get { return _firstWord; }
            set
            {
                _firstWord = value;
                OnPropertyChanged(nameof(FirstWord));
            }
        }

        private string _secondWord;
        public string SecondWord
        {
            get { return _secondWord; }
            set
            {
                _secondWord = value;
                OnPropertyChanged(nameof(SecondWord));
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