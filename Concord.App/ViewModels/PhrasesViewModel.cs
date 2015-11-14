using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Concord.App.Models;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;

namespace Concord.App.ViewModels
{
    public class PhrasesViewModel
    {
        //public NewGroupModel NewData { get; set; }
        public string NewPhrase { get; set; }
        public ObservableCollection<PhraseModel> Phrases { get; set; }
        public PhraseModel SelectedPhrase { get; set; }

// TODO : MISSING : find contexts by word from group or by group (? or by few words from group ?)

        // TODO : REMOVE !!!
        private int _phId = 1;
        private int _phraseNumberTemp = 1;
        private int _wordIdTemp = 1;

        public PhrasesViewModel()
        {
            // TODO : fetch phrases from db

            Phrases = new ObservableCollection<PhraseModel>();
            SelectedPhrase = Phrases.FirstOrDefault() ?? new PhraseModel();
        }

        private DelegateCommand createPhraseCommand;
        public ICommand CreatePhraseCommand
        {
            get
            {
                if (createPhraseCommand == null)
                    createPhraseCommand = new DelegateCommand(CreatePhraseExecuted, CreatePhraseCanExecute);

                return createPhraseCommand;
            }
        }

        public bool CreatePhraseCanExecute()
        {
            return true;
        }

        public void CreatePhraseExecuted()
        {
            if (string.IsNullOrEmpty(NewPhrase))
            {
                // TODO : set error
                return;
            }

            if (Phrases.SingleOrDefault(g => g.ToString() == NewPhrase) != null)
            {
                // TODO : set error
                return;
            }

            // TODO : create real group

            var phraseWords = NewPhrase.Split(' ');
            var seq = 1;
            var newPhrase = new PhraseModel {Words = new ObservableCollection<PhraseWordModel>()};

            // TODO : NEW DB SEQUENCE - phrase number

            // TODO : this is where i stopped

            foreach (var phraseWord in phraseWords)
            {
                // TODO : create new word
                // TODO : create new phrase word
                var word = new WordModel {Id = _wordIdTemp++, Word = phraseWord, Repetition = 0};
                var newPhraseWord = new PhraseWordModel {Id = _phId++, Word = word, WordSequence = seq++, PhraseNumber = _phraseNumberTemp};
                newPhrase.Words.Add(newPhraseWord);
            }

            _phraseNumberTemp++;

            Phrases.Add(newPhrase);
            NewPhrase = string.Empty;
        }
        
        private DelegateCommand doubleClickPhraseCommand;
        public ICommand DoubleClickPhraseCommand
        {
            get
            {
                if (doubleClickPhraseCommand == null)
                    doubleClickPhraseCommand = new DelegateCommand(DoubleClickPhraseExecuted, DoubleClickPhraseCanExecute);

                return doubleClickPhraseCommand;
            }
        }

        public bool DoubleClickPhraseCanExecute()
        {
            return true;
        }

        public void DoubleClickPhraseExecuted()
        {
            if (!Phrases.Any())
                return;

            if (!SelectedPhrase.Words.Any())
            {
                // TODO : set error
                return;
            }

            // TODO : fetch group contexts

            ((MainWindow) Application.Current.MainWindow).HiddenTabFocusAllowed = true;
            ((MainWindow) Application.Current.MainWindow).MainTabControl.SelectedIndex = 5;
        } 
    }
}