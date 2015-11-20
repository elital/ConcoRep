using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Concord.App.Models;
using Microsoft.Practices.Prism.Commands;

namespace Concord.App.ViewModels
{
    public class PhrasesViewModel
    {
        public PhraseModel NewPhrase { get; set; }
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
            NewPhrase = new PhraseModel();
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
            if (string.IsNullOrEmpty(NewPhrase.Text))
            {
                // TODO : set error
                return;
            }

            if (Phrases.SingleOrDefault(p => p.Text == NewPhrase.Text) != null)
            {
                // TODO : set error
                return;
            }

            // TODO : create real phrase

            //var phraseWords = NewPhrase.Text.Split(' ');
            //var seq = 1;
            //var newPhrase = new PhraseModel();

            // TODO : NEW DB SEQUENCE - phrase number

            // TODO : this is where i stopped

            //foreach (var phraseWord in phraseWords)
            //{
            //    // TODO : create new word
            //    // TODO : create new phrase word
            //    var word = new WordModel {Id = _wordIdTemp++, Word = phraseWord, Repetitions = 0};
            //    //var newPhraseWord = new PhraseWordModel {Id = _phId++, Word = word, WordSequence = seq++, PhraseNumber = _phraseNumberTemp};
            //    //newPhrase.Words.Add(newPhraseWord);
            //}

            var newPhrase = new PhraseModel {PhraseNumber = _phraseNumberTemp++, Text = NewPhrase.Text};
            Phrases.Add(newPhrase);
            NewPhrase.Text = string.Empty;
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

            if (string.IsNullOrEmpty(SelectedPhrase.Text))
            {
                // TODO : set error
                return;
            }

            // TODO : fetch group contexts

            ((MainWindow) Application.Current.MainWindow).HiddenTabFocusAllowed = true;
            ((MainWindow) Application.Current.MainWindow).MainTabControl.SelectedIndex = 5;
        }

        public void AppendWord(WordModel word)
        {
            NewPhrase.Text = string.IsNullOrEmpty(NewPhrase.Text)
                ? word.Word
                : $"{NewPhrase.Text} {word.Word}";
        }
    }
}