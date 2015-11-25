using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Concord.App.Models;
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

        // TODO : MISSING : find contexts by pair from relation or by relation (? or by few pairs from relation ?)

        // TODO : REMOVE !!!
        private int _relationIdTemp = 1;
        private int _wordIdTemp = 1;

        public RelationsViewModel()
        {
            Relations = new ObservableCollection<RelationModel>();
            Pairs = new ObservableCollection<PairModel>();
            NewData = new NewRelationModel();
            SelectedRelation = Relations.FirstOrDefault() ?? new RelationModel();
        }

        private DelegateCommand createRelationCommand;
        public ICommand CreateRelationCommand
        {
            get
            {
                if (createRelationCommand == null)
                    createRelationCommand = new DelegateCommand(CreateRelationExecuted, CreateRelationCanExecute);

                return createRelationCommand;
            }
        }

        public bool CreateRelationCanExecute()
        {
            return true;
        }

        public void CreateRelationExecuted()
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

            // TODO : create real relation

            var newRelation = new RelationModel {Name = NewData.RelationName, Pairs = new ObservableCollection<PairModel>()};
            Relations.Add(newRelation);
            NewData.RelationName = string.Empty;
            SelectedRelation = newRelation;
            SelectedRelationExecuted();
        }

        private DelegateCommand addPairCommand;
        public ICommand AddPairCommand
        {
            get
            {
                if (addPairCommand == null)
                    addPairCommand = new DelegateCommand(AddPairExecuted, AddPairCanExecute);

                return addPairCommand;
            }
        }

        public bool AddPairCanExecute()
        {
            return true;
        }

        public void AddPairExecuted()
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

            // TODO : create real words
            // TODO : connect new words to the relation

            var newPair = new PairModel
                {
                    FirstWord = new WordModel {Id = _wordIdTemp++, Word = NewData.FirstWord, Repetitions = 0},
                    SecondWord = new WordModel {Id = _wordIdTemp++, Word = NewData.SecondWord, Repetitions = 0}
                };
            Relations.Single(r => r.Name == SelectedRelation.Name).Pairs.Add(newPair);
            Pairs.Add(newPair);
            NewData.FirstWord = NewData.SecondWord = string.Empty;
        }

        private DelegateCommand selectedRelationCommand;
        public ICommand SelectedRelationCommand
        {
            get
            {
                if (selectedRelationCommand == null)
                    selectedRelationCommand = new DelegateCommand(SelectedRelationExecuted, SelectedRelationCanExecute);

                return selectedRelationCommand;
            }
        }

        public bool SelectedRelationCanExecute()
        {
            return true;
        }

        public void SelectedRelationExecuted()
        {
            Pairs.Clear();
            Pairs.AddRange(SelectedRelation.Pairs);
        }

        private DelegateCommand doubleClickRelationCommand;
        public ICommand DoubleClickRelationCommand
        {
            get
            {
                if (doubleClickRelationCommand == null)
                    doubleClickRelationCommand = new DelegateCommand(DoubleClickRelationExecuted, DoubleClickRelationCanExecute);

                return doubleClickRelationCommand;
            }
        }

        public bool DoubleClickRelationCanExecute()
        {
            return true;
        }

        public void DoubleClickRelationExecuted()
        {
            if (!Relations.Any())
                return;

            if (string.IsNullOrEmpty(SelectedRelation.Name))
            {
                // TODO : set error
                return;
            }

            // TODO : fetch relation contexts

            ((MainWindow)Application.Current.MainWindow).HiddenTabFocusAllowed = true;
            ((MainWindow)Application.Current.MainWindow).MainTabControl.SelectedIndex = 5;
        }

        public void AppendWord(WordModel word)
        {
            if (string.IsNullOrEmpty(NewData.FirstWord))
                NewData.FirstWord = word.Word;
            else
                NewData.SecondWord = word.Word;
        }
    }
}