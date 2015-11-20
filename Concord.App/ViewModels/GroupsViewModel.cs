using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Concord.App.Models;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;

namespace Concord.App.ViewModels
{
    public class GroupsViewModel
    {
        public NewGroupModel NewData { get; set; }
        public ObservableCollection<GroupModel> Groups { get; set; }
        public ObservableCollection<WordModel> Words { get; set; }
        public GroupModel SelectedGroup { get; set; }

        // TODO : MISSING : find contexts by word from group or by group (? or by few words from group ?)

        // TODO : REMOVE !!!
        private int _groupIdTemp = 1;
        private int _wordIdTemp = 1;

        public GroupsViewModel()
        {
            // TODO : fetch groups from db

            NewData = new NewGroupModel();
            Groups = new ObservableCollection<GroupModel>();
            Words = new ObservableCollection<WordModel>();
            SelectedGroup = Groups.FirstOrDefault() ?? new GroupModel();
        }

        #region Create group command

        private DelegateCommand _createGroupCommand;

        public ICommand CreateGroupCommand => _createGroupCommand ??
                                              (_createGroupCommand = new DelegateCommand(CreateGroupExecuted, CreateGroupCanExecute));

        private bool CreateGroupCanExecute()
        {
            return true;
        }

        private void CreateGroupExecuted()
        {
            if (string.IsNullOrEmpty(NewData.GroupName))
            {
                // TODO : set error
                return;
            }

            if (Groups.SingleOrDefault(g => g.Name == NewData.GroupName) != null)
            {
                // TODO : set error
                return;
            }

            // TODO : create real group - not necessary
            
            var newGroup = new GroupModel {Name = NewData.GroupName, Words = new ObservableCollection<WordModel>()};
            Groups.Add(newGroup);
            NewData.GroupName = string.Empty;
            SelectedGroup = newGroup;
            SelectedGroupExecuted();
        }

        #endregion

        #region Add word command

        private DelegateCommand _addWordCommand;
        public ICommand AddWordCommand => _addWordCommand ?? (_addWordCommand = new DelegateCommand(AddWordExecuted, AddWordCanExecute));

        private bool AddWordCanExecute()
        {
            return true;
        }

        private void AddWordExecuted()
        {
            if (string.IsNullOrEmpty(SelectedGroup.Name) || string.IsNullOrEmpty(NewData.Word))
            {
                // TODO : set error
                return;
            }

            if (SelectedGroup.Words.SingleOrDefault(w => w.Word == NewData.Word) != null)
            {
                // TODO : set error - word already exist
                return;
            }

            // TODO : get or create word - inside new group word
            // TODO : create real group word

            var newGroupWord = new WordModel {Id = _wordIdTemp++, Word = NewData.Word, Repetitions = 0};
            Groups.Single(g => g.Id == SelectedGroup.Id).Words.Add(newGroupWord);
            Words.Add(newGroupWord);
            NewData.Word = string.Empty;
        }

        #endregion

        private DelegateCommand selectedGroupCommand;
        public ICommand SelectedGroupCommand
        {
            get
            {
                if (selectedGroupCommand == null)
                    selectedGroupCommand = new DelegateCommand(SelectedGroupExecuted, SelectedGroupCanExecute);

                return selectedGroupCommand;
            }
        }

        public bool SelectedGroupCanExecute()
        {
            return true;
        }

        public void SelectedGroupExecuted()
        {
            Words.Clear();
            Words.AddRange(SelectedGroup.Words);
        }

        private DelegateCommand doubleClickGroupCommand;
        public ICommand DoubleClickGroupCommand
        {
            get
            {
                if (doubleClickGroupCommand == null)
                    doubleClickGroupCommand = new DelegateCommand(DoubleClickGroupExecuted, DoubleClickGroupCanExecute);

                return doubleClickGroupCommand;
            }
        }

        public bool DoubleClickGroupCanExecute()
        {
            return true;
        }

        public void DoubleClickGroupExecuted()
        {
            if (!Groups.Any())
                return;

            if (string.IsNullOrEmpty(SelectedGroup.Name))
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
            NewData.Word = word.Word;
        }
    }
}