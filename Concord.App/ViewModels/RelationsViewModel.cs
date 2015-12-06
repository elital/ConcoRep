using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AutoMapper;
using Concord.App.HiddenTabsData;
using Concord.App.Models;
using Concord.Dal.General;
using Concord.Dal.RelationEntity;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;

namespace Concord.App.ViewModels
{
    public class RelationsViewModel
    {
        public ObservableCollection<RelationModel> Relations { get; set; }
        public ObservableCollection<PairModel> Pairs { get; set; }

        public NewRelationModel NewData { get; set; }
        public RelationModel SelectedRelation { get; set; }

        public RelationsViewModel()
        {
            Relations = new ObservableCollection<RelationModel>();
            Pairs = new ObservableCollection<PairModel>();
            NewData = new NewRelationModel();
            SelectedRelation = Relations.FirstOrDefault() ?? new RelationModel();
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
            Relations.Clear();
            Relations.AddRange(Mapper.Map<List<RelationModel>>(new RelationQuery().Get()));
        }

        #endregion

        #region CreateRelationCommand

        private DelegateCommand _createRelationCommand;

        public ICommand CreateRelationCommand => _createRelationCommand ??
                                                 (_createRelationCommand = new DelegateCommand(CreateRelationExecuted, CreateRelationCanExecute));

        private bool CreateRelationCanExecute()
        {
            return true;
        }

        private void CreateRelationExecuted()
        {
            if (string.IsNullOrEmpty(NewData.RelationName))
            {
                // TODO : set error
                return;
            }

            if (Relations.SingleOrDefault(g => g.Name == NewData.RelationName) != null)
            {
                // TODO : set error
                return;
            }
            
            var newRelation = new RelationModel {Name = NewData.RelationName, Pairs = new ObservableCollection<PairModel>()};
            Relations.Add(newRelation);
            NewData.RelationName = string.Empty;
            SelectedRelation = newRelation;
            SelectedRelationExecuted();

            // TODO : Add message - relation will be created with the first pair
        }

        #endregion

        #region AddPairCommand

        private DelegateCommand _addPairCommand;
        public ICommand AddPairCommand => _addPairCommand ?? (_addPairCommand = new DelegateCommand(AddPairExecuted, AddPairCanExecute));

        private bool AddPairCanExecute()
        {
            return true;
        }

        private void AddPairExecuted()
        {
            if (string.IsNullOrEmpty(SelectedRelation.Name))
            {
                // TODO : set error
                return;
            }

            if (string.IsNullOrEmpty(NewData.FirstWord) || string.IsNullOrEmpty(NewData.SecondWord))
            {
                // TODO : set error
                return;
            }

            if (SelectedRelation.Pairs.SingleOrDefault(p => p.FirstWord.Word == NewData.FirstWord && p.SecondWord.Word == NewData.SecondWord) != null)
            {
                // TODO : set error - pair already exist
                return;
            }
            
            var newPair = RelationCreator.Instance.CreateRelationPair(SelectedRelation.Name, NewData.FirstWord, NewData.SecondWord);
            var pair = Mapper.Map<PairModel>(newPair);
            Relations.Single(r => r.Name == SelectedRelation.Name).Pairs.Add(pair);
            Pairs.Add(pair);
            NewData.FirstWord = NewData.SecondWord = string.Empty;
            ((MainWindow) Application.Current.MainWindow).RefreshWordAction();
        }

        #endregion

        #region SelectedRelationCommand

        private DelegateCommand _selectedRelationCommand;

        public ICommand SelectedRelationCommand =>
            _selectedRelationCommand ??
            (_selectedRelationCommand = new DelegateCommand(SelectedRelationExecuted, SelectedRelationCanExecute));

        private bool SelectedRelationCanExecute()
        {
            return true;
        }

        private void SelectedRelationExecuted()
        {
            Pairs.Clear();

            if (SelectedRelation != null && SelectedRelation.Pairs.Any())
                Pairs.AddRange(SelectedRelation.Pairs);
        }

        #endregion

        #region DoubleClickRelationCommand

        private DelegateCommand _doubleClickRelationCommand;

        public ICommand DoubleClickRelationCommand =>
            _doubleClickRelationCommand ??
            (_doubleClickRelationCommand = new DelegateCommand(DoubleClickRelationExecuted, DoubleClickRelationCanExecute));

        private bool DoubleClickRelationCanExecute()
        {
            return true;
        }

        private void DoubleClickRelationExecuted()
        {
            if (!Relations.Any())
                return;

            if (string.IsNullOrEmpty(SelectedRelation.Name))
            {
                // TODO : set error
                return;
            }

            var contexts = new ContextQuery {RelationName = SelectedRelation.Name}.Get();
            ResultData.Instance.Contexts = Mapper.Map<List<ContextModel>>(contexts);

            ((MainWindow)Application.Current.MainWindow).HiddenTabFocusAllowed = true;
            ((MainWindow)Application.Current.MainWindow).MainTabControl.SelectedIndex = 5;
        }

        #endregion

        public void AppendWord(WordModel word)
        {
            if (string.IsNullOrEmpty(NewData.FirstWord))
                NewData.FirstWord = word.Word;
            else
                NewData.SecondWord = word.Word;
        }
    }
}