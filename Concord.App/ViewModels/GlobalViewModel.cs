using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Concord.App.Annotations;
using Concord.App.Models;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;

namespace Concord.App.ViewModels
{
    public class GlobalViewModel
    {
        public ObservableCollection<SongModel> Songs { get; set; }

        public GlobalViewModel()
        {
            Songs = new ObservableCollection<SongModel>();

            // TODO : REMOVE
            Songs.Add(new SongModel
                {
                    Id = 1,
                    Title = "asdasd",
                    Author = "jghgfg",
                    Album = "hey",
                    PublishDate = new DateTime(2015, 2, 26),
                    Lyrics = new ObservableCollection<LyricsModel>()
                        {
                            new LyricsModel() {Id = 1, Line = 1, Column = 1, Word = new WordModel() {Id = 1, Word = "hey", Repetition = 2}},
                            new LyricsModel() {Id = 2, Line = 1, Column = 2, Word = new WordModel() {Id = 2, Word = "you", Repetition = 7}},
                            new LyricsModel() {Id = 3, Line = 2, Column = 1, Word = new WordModel() {Id = 3, Word = "bye", Repetition = 1}}
                        }
                });
        }

        private DelegateCommand goCommand;
        public ICommand GoCommand
        {
            get
            {
                if (goCommand == null)
                    goCommand = new DelegateCommand(GoExecuted, GoCanExecute);

                return goCommand;
            }
        }

        public bool GoCanExecute()
        {
            return true;
        }

        public void GoExecuted()
        {
            // TODO : fetch results from db

            //Songs.Clear();
            //Songs.AddRange( from DB );

            Songs.Add(new SongModel
                {
                    Id = 2,
                    Title = "hello world",
                    Author = "who wrote that song",
                    Album = "hey",
                    PublishDate = new DateTime(2015, 2, 26),
                    Lyrics = new ObservableCollection<LyricsModel>()
                        {
                            new LyricsModel() {Id = 4, Line = 1, Column = 1, Word = new WordModel() {Id = 1, Word = "hey", Repetition = 2}},
                            new LyricsModel() {Id = 5, Line = 1, Column = 2, Word = new WordModel() {Id = 2, Word = "you", Repetition = 7}},
                            new LyricsModel() {Id = 6, Line = 2, Column = 1, Word = new WordModel() {Id = 4, Word = "boy", Repetition = 9}}
                        }
                });
        }
    }
}