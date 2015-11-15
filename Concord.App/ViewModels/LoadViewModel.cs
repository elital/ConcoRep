using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Concord.App.Models;
using Microsoft.Practices.Prism.Commands;

namespace Concord.App.ViewModels
{
    public class LoadViewModel
    {
        public SongModel Song { get; set; }

        private Visibility _buttonVisibility;
        public Visibility ButtonVisibility
        {
            get { return _buttonVisibility; }
            set { _buttonVisibility = value; }
        }

        public LoadViewModel()
        {
            Song = new SongModel { PublishDate = DateTime.Today };
        }

        #region Load new song

        private DelegateCommand loadNewSongCommand;
        public ICommand LoadNewSongCommand
        {
            get
            {
                if (loadNewSongCommand == null)
                    loadNewSongCommand = new DelegateCommand(LoadNewSongExecuted, LoadNewSongCanExecute);

                return loadNewSongCommand;
            }
        }

        public bool LoadNewSongCanExecute()
        {
            return true;
        }

        public void LoadNewSongExecuted()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "TEXT Files (*.txt)|*.txt|XML Files (*.xml)|*.xml"
            };

            var isValid = false;

            while (!isValid)
            {
                var dialogResult = openFileDialog.ShowDialog();

                if (!dialogResult.HasValue || !dialogResult.Value)
                    return;

                var filename = openFileDialog.FileName;
                string text = string.Empty;
                MessageBoxResult messageBoxResult = MessageBoxResult.OK;

                try
                {
                    text = File.ReadAllText(filename);
                    isValid = true;
                }
                catch (FileNotFoundException)
                {
                    messageBoxResult = MessageBox.Show("The chosen file is not found, please choose a different file.",
                        "File not found", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
                catch
                {
                    messageBoxResult = MessageBox.Show("An error occured while openning the file, please try again.",
                        "An error occured", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }

                if (messageBoxResult == MessageBoxResult.Cancel)
                    return;

                if (isValid)
                {
                    var data = text.Split(new[] { Environment.NewLine }, 2, StringSplitOptions.None);
                    var details = data[0].Split(';');

                    Song.Text = data[1];
                    Song.Title = details[0];
                    Song.Author = details[1];
                    Song.Album = details[2];
                    Song.PublishDate = DateTime.ParseExact(details[3], "dd/MM/yyyy", CultureInfo.CurrentCulture);
                }
            }
        }

        #endregion

        #region Save new song

        private DelegateCommand saveNewSongCommand;

        public ICommand SaveNewSongCommand
        {
            get
            {
                if (saveNewSongCommand == null)
                    saveNewSongCommand = new DelegateCommand(SaveNewSongExecuted, SaveNewSongCanExecute);

                return saveNewSongCommand;
            }
        }

        public bool SaveNewSongCanExecute()
        {
            return true;
        }

        public void SaveNewSongExecuted()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}