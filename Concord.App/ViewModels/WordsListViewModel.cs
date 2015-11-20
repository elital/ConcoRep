using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AutoMapper;
using Concord.App.Models;
using Concord.Dal.WordEntity;
using Concord.Entities;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;

namespace Concord.App.ViewModels
{
    public class WordsListViewModel
    {
        public ObservableCollection<WordModel> Words { get; set; }

        public WordModel SelectedItem { get; set; }

        public WordsListViewModel()
        {
            Words = new ObservableCollection<WordModel>();
            SelectedItem = new WordModel();
        }

        private void RefreshWords()
        {
            Words.Clear();
            Words.AddRange(new WordQuery {OrderByField = WordQuery.OrderBy.Repetitions}.Get().ToList().Select(Mapper.Map<Word, WordModel>));
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
            ((MainWindow)Application.Current.MainWindow).RefreshWordAction = RefreshWords;
            RefreshWords();
        }

        #endregion

        #region Word double-click

        private DelegateCommand _wordDoubleClickCommand;

        public ICommand WordDoubleClickCommand => _wordDoubleClickCommand ??
                                                 (_wordDoubleClickCommand = new DelegateCommand(WordDoubleClickExecuted, WordDoubleClickCanExecute));

        private bool WordDoubleClickCanExecute()
        {
            return true;
        }

        private void WordDoubleClickExecuted()
        {
            ((MainWindow) Application.Current.MainWindow).SelectedTab?.WordDoubleClicked(SelectedItem);
        }

        #endregion
    }
}