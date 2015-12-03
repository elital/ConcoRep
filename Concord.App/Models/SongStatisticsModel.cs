using System.ComponentModel;
using System.Runtime.CompilerServices;
using Concord.App.Annotations;

namespace Concord.App.Models
{
    public class SongStatisticsModel : INotifyPropertyChanged
    {
        #region Properties

        private int _wordsAmount;
        public int WordsAmount
        {
            get { return _wordsAmount; }
            set
            {
                _wordsAmount = value;
                OnPropertyChanged(nameof(WordsAmount));
            }
        }

        private string _longestWord;
        public string LongestWord
        {
            get { return _longestWord; }
            set
            {
                _longestWord = value;
                OnPropertyChanged(nameof(LongestWord));
            }
        }

        private int _longestWordLength;
        public int LongestWordLength
        {
            get { return _longestWordLength; }
            set
            {
                _longestWordLength = value;
                OnPropertyChanged(nameof(LongestWordLength));
            }
        }

        private string _shortestWord;
        public string ShortestWord
        {
            get { return _shortestWord; }
            set
            {
                _shortestWord = value;
                OnPropertyChanged(nameof(ShortestWord));
            }
        }

        private int _shortestWordLength;
        public int ShortestWordLength
        {
            get { return _shortestWordLength; }
            set
            {
                _shortestWordLength = value;
                OnPropertyChanged(nameof(ShortestWordLength));
            }
        }

        private string _mostRepeatedWord;
        public string MostRepeatedWord
        {
            get { return _mostRepeatedWord; }
            set
            {
                _mostRepeatedWord = value;
                OnPropertyChanged(nameof(MostRepeatedWord));
            }
        }

        private int _mostRepeatedWordRepetitions;
        public int MostRepeatedWordRepetitions
        {
            get { return _mostRepeatedWordRepetitions; }
            set
            {
                _mostRepeatedWordRepetitions = value;
                OnPropertyChanged(nameof(MostRepeatedWordRepetitions));
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}