using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AutoMapper;
using Concord.App.HiddenTabsData;
using Concord.App.Models;
using Concord.Dal.General;
using Concord.Dal.GroupEntity;
using Concord.Entities;
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

        
        public GroupsViewModel()
        {
            // TODO : fetch groups from db

            NewData = new NewGroupModel();
            Groups = new ObservableCollection<GroupModel>();
            Words = new ObservableCollection<WordModel>();
            SelectedGroup = Groups.FirstOrDefault() ?? new GroupModel();
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
            Groups.Clear();
            Groups.AddRange(Mapper.Map<List<GroupModel>>(new GroupQuery().Get()));
        }

        #endregion

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

            // TODO : Set message - the group will be created with it's first word only
        }

        #endregion

        #region Add word command

        private DelegateCommand _addWordCommand;

        public ICommand AddWordCommand => _addWordCommand ??
                                          (_addWordCommand = new DelegateCommand(AddWordExecuted, AddWordCanExecute));

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

            var word = Mapper.Map<Word, WordModel>(GroupCreator.Instance.CreateGroupWord(SelectedGroup.Name, NewData.Word));
            Groups.Single(g => g.Name == SelectedGroup.Name).Words.Add(word);
            Words.Add(word);
            NewData.Word = string.Empty;
        }

        #endregion

        #region Selected group command

        private DelegateCommand _selectedGroupCommand;

        public ICommand SelectedGroupCommand => _selectedGroupCommand ??
                                                (_selectedGroupCommand = new DelegateCommand(SelectedGroupExecuted, SelectedGroupCanExecute));

        private bool SelectedGroupCanExecute()
        {
            return true;
        }

        private void SelectedGroupExecuted()
        {
            Words.Clear();

            if (SelectedGroup != null && SelectedGroup.Words.Any())
                Words.AddRange(SelectedGroup.Words);
        }

        #endregion

        #region Double-click group command

        private DelegateCommand _doubleClickGroupCommand;

        public ICommand DoubleClickGroupCommand =>
            _doubleClickGroupCommand ??
            (_doubleClickGroupCommand = new DelegateCommand(DoubleClickGroupExecuted, DoubleClickGroupCanExecute));

        private bool DoubleClickGroupCanExecute()
        {
            return true;
        }

        private void DoubleClickGroupExecuted()
        {
            if (!Groups.Any())
                return;

            if (string.IsNullOrEmpty(SelectedGroup.Name))
            {
                // TODO : set error
                return;
            }
            
            var contexts = new ContextQuery {GroupName = SelectedGroup.Name}.Get();
            ResultData.Instance.Contexts = Mapper.Map<List<ContextModel>>(contexts);

            var main = (MainWindow) Application.Current.MainWindow;
            main.HiddenTabFocusAllowed = true;
            main.GotToTab(main.ContextTabName);
        }

        #endregion

        public void AppendWord(WordModel word)
        {
            NewData.Word = word.Word;
        }
    }
}