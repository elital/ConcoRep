using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AutoMapper;
using Concord.App.HiddenTabsData;
using Concord.App.Models;
using Concord.Dal.General;
using Concord.Dal.PhraseEntity;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;

namespace Concord.App.ViewModels
{
    public class PhrasesViewModel
    {
        public PhraseModel NewPhrase { get; set; }
        public ObservableCollection<PhraseModel> Phrases { get; set; }
        public PhraseModel SelectedPhrase { get; set; }

        public PhrasesViewModel()
        {
            // TODO : fetch phrases from db
            NewPhrase = new PhraseModel();
            Phrases = new ObservableCollection<PhraseModel>();
            SelectedPhrase = Phrases.FirstOrDefault() ?? new PhraseModel();
        }

        #region MainDockLoadedCommand

        private DelegateCommand _mainDockLoadedCommand;

        public ICommand MainDockLoadedCommand => _mainDockLoadedCommand ??
                                                 (_mainDockLoadedCommand = new DelegateCommand(MainDockLoadedExecuted, MainDockLoadedCanExecute));

        private bool MainDockLoadedCanExecute()
        {
            return true;
        }

        private void MainDockLoadedExecuted()
        {
            Phrases.Clear();
            Phrases.AddRange(Mapper.Map<List<PhraseModel>>(new PhraseQuery().Get()));
        }

        #endregion

        #region Create phrase command

        private DelegateCommand _createPhraseCommand;

        public ICommand CreatePhraseCommand => _createPhraseCommand ??
                                               (_createPhraseCommand = new DelegateCommand(CreatePhraseExecuted, CreatePhraseCanExecute));
        
        private bool CreatePhraseCanExecute()
        {
            return true;
        }

        private void CreatePhraseExecuted()
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

            var newPhrase = Mapper.Map<PhraseModel>(PhraseCreator.Instance.Create(NewPhrase.Text));
            Phrases.Add(newPhrase);
            NewPhrase.Text = string.Empty;
            ((MainWindow) Application.Current.MainWindow).RefreshWordAction();
        }

        #endregion

        #region DoubleClickPhraseCommand

        private DelegateCommand _doubleClickPhraseCommand;

        public ICommand DoubleClickPhraseCommand =>
            _doubleClickPhraseCommand ??
            (_doubleClickPhraseCommand = new DelegateCommand(DoubleClickPhraseExecuted, DoubleClickPhraseCanExecute));
        
        private bool DoubleClickPhraseCanExecute()
        {
            return true;
        }

        private void DoubleClickPhraseExecuted()
        {
            if (!Phrases.Any())
                return;

            if (string.IsNullOrEmpty(SelectedPhrase.Text))
            {
                // TODO : set error
                return;
            }

            // TODO : fetch group contexts

            var contexts = new ContextQuery {PhraseNumber = SelectedPhrase.PhraseNumber}.Get();
            ResultData.Instance.Contexts = Mapper.Map<List<ContextModel>>(contexts);

            ((MainWindow) Application.Current.MainWindow).HiddenTabFocusAllowed = true;
            ((MainWindow) Application.Current.MainWindow).MainTabControl.SelectedIndex = 5;
        }

        #endregion

        public void AppendWord(WordModel word)
        {
            NewPhrase.Text = string.IsNullOrEmpty(NewPhrase.Text)
                ? word.Word
                : $"{NewPhrase.Text} {word.Word}";
        }
    }
}