using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using AutoMapper;
using Concord.App.Annotations;
using Concord.App.HiddenTabsData;
using Concord.App.Models;
using Concord.Dal.General;
using Concord.Dal.SongEntity;
using Concord.Entities;
using Microsoft.Practices.Prism.Commands;

namespace Concord.App.ViewModels
{
    public class GeneralProperties : INotifyPropertyChanged
    {
        private bool _readonly;

        public bool Readonly
        {
            get { return _readonly; }
            set
            {
                _readonly = value;
                OnPropertyChanged(nameof(Readonly));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class LoadViewModel
    {
        public SongModel Song { get; set; }

        public GeneralProperties Properties { get; set; }

        public SongStatisticsModel SongStatistics { get; set; }

        public LoadViewModel()
        {
            Song = new SongModel { PublishDate = DateTime.Today };
            Properties = new GeneralProperties();
            SongStatistics = new SongStatisticsModel();
        }
        
        #region Load new song

        private DelegateCommand _loadNewSongCommand;

        public ICommand LoadNewSongCommand => _loadNewSongCommand ??
                                              (_loadNewSongCommand = new DelegateCommand(LoadNewSongExecuted, LoadNewSongCanExecute));
        
        private bool LoadNewSongCanExecute()
        {
            return true;
        }

        private void LoadNewSongExecuted()
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

        private DelegateCommand _saveNewSongCommand;

        public ICommand SaveNewSongCommand => _saveNewSongCommand ??
                                              (_saveNewSongCommand = new DelegateCommand(SaveNewSongExecuted, SaveNewSongCanExecute));
        
        private bool SaveNewSongCanExecute()
        {
            return true;
        }

        private void SaveNewSongExecuted()
        {
            try
            {
                var songId = SongCreator.Instance.Create(Mapper.Map<Song>(Song));
                ResultData.Instance.SongId = songId;
                Song.Clear();

                var mainWindow = (MainWindow) Application.Current.MainWindow;

                mainWindow.RefreshWordAction();
                mainWindow.HiddenTabFocusAllowed = true;
                mainWindow.GotToTab(mainWindow.SongViewTabName);
            }
            catch (ArgumentNullException)
            {
                // TODO : Set error
            }
        }

        #endregion

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
            if (!Properties.Readonly)
                return;

            if (ResultData.Instance.SongId <= 0)
                return;

            var song = new SongQuery().GetById(ResultData.Instance.SongId);
            Mapper.Map(song, Song);

            var songStat = StatisticsQuery.Instance.GetSongStatistics(ResultData.Instance.SongId);
            Mapper.Map(songStat, SongStatistics);
        }

        #endregion

        #region MainDockUnloadedCommand

        private DelegateCommand _mainDockUnloadedCommand;

        public ICommand MainDockUnloadedCommand =>
            _mainDockUnloadedCommand ??
            (_mainDockUnloadedCommand = new DelegateCommand(MainDockUnloadedExecuted, MainDockUnloadedCanExecute));
        
        private bool MainDockUnloadedCanExecute()
        {
            return true;
        }

        private void MainDockUnloadedExecuted()
        {
            if (!Properties.Readonly)
                return;

            ResultData.Instance.SongId = 0;
            ((MainWindow)Application.Current.MainWindow).HiddenTabFocusAllowed = false;
        }

        #endregion
    }
}