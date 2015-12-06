using System.ComponentModel;
using System.Runtime.CompilerServices;
using Concord.App.Annotations;

namespace Concord.App.Models
{
    public class SystemStatisticsModel : INotifyPropertyChanged
    {
        #region Properties

        private int _totalSongsAmount;
        public int TotalSongsAmount
        {
            get { return _totalSongsAmount; }
            set
            {
                _totalSongsAmount = value;
                OnPropertyChanged(nameof(TotalSongsAmount));
            }
        }

        private int _totalSystemSongsWordsAmount;
        public int TotalSystemSongsWordsAmount
        {
            get { return _totalSystemSongsWordsAmount; }
            set
            {
                _totalSystemSongsWordsAmount = value;
                OnPropertyChanged(nameof(TotalSystemSongsWordsAmount));
            }
        }

        private int _systemDifferentSongsWordsAmount;
        public int SystemDifferentSongsWordsAmount
        {
            get { return _systemDifferentSongsWordsAmount; }
            set
            {
                _systemDifferentSongsWordsAmount = value;
                OnPropertyChanged(nameof(SystemDifferentSongsWordsAmount));
            }
        }

        private string _longestSongName;
        public string LongestSongName
        {
            get { return _longestSongName; }
            set
            {
                _longestSongName = value;
                OnPropertyChanged(nameof(LongestSongName));
            }
        }

        private int _longestSongWordsAmount;
        public int LongestSongWordsAmount
        {
            get { return _longestSongWordsAmount; }
            set
            {
                _longestSongWordsAmount = value;
                OnPropertyChanged(nameof(LongestSongWordsAmount));
            }
        }

        private string _shortestSongName;
        public string ShortestSongName
        {
            get { return _shortestSongName; }
            set
            {
                _shortestSongName = value;
                OnPropertyChanged(nameof(ShortestSongName));
            }
        }

        private int _shortestSongWordsAmount;
        public int ShortestSongWordsAmount
        {
            get { return _shortestSongWordsAmount; }
            set
            {
                _shortestSongWordsAmount = value;
                OnPropertyChanged(nameof(ShortestSongWordsAmount));
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

        private string _mostRepeatedWordRepetition;
        public string MostRepeatedWordRepetition
        {
            get { return _mostRepeatedWordRepetition; }
            set
            {
                _mostRepeatedWordRepetition = value;
                OnPropertyChanged(nameof(MostRepeatedWordRepetition));
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