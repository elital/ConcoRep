﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
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

        private DelegateCommand createGroupCommand;
        public ICommand CreateGroupCommand
        {
            get
            {
                if (createGroupCommand == null)
                    createGroupCommand = new DelegateCommand(CreateGroupExecuted, CreateGroupCanExecute);

                return createGroupCommand;
            }
        }

        public bool CreateGroupCanExecute()
        {
            return true;
        }

        public void CreateGroupExecuted()
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

            // TODO : create real group

            var newGroup = new GroupModel {Name = NewData.GroupName, Id = _groupIdTemp++, Words = new ObservableCollection<WordModel>()};
            Groups.Add(newGroup);
            NewData.GroupName = string.Empty;
            SelectedGroup = newGroup;
        }

        private DelegateCommand addWordCommand;
        public ICommand AddWordCommand
        {
            get
            {
                if (addWordCommand == null)
                    addWordCommand = new DelegateCommand(AddWordExecuted, AddWordCanExecute);

                return addWordCommand;
            }
        }

        public bool AddWordCanExecute()
        {
            return true;
        }

        public void AddWordExecuted()
        {
            if (string.IsNullOrEmpty(SelectedGroup.Name) || string.IsNullOrEmpty(NewData.Word))
            {
                // TODO : set error
                return;
            }

            if (SelectedGroup.Words.SingleOrDefault(w => w.Word == NewData.Word) != null)
            {
                // TODO : set error
                return;
            }

            // TODO : create real word
            // TODO : connect new word to the group

            Groups.Single(g => g.Id == SelectedGroup.Id).Words.Add(new WordModel {Id = _wordIdTemp++, Word = NewData.Word, Repetition = 0});
            SelectedGroupExecuted();
            NewData.Word = string.Empty;
        }

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
    }
}