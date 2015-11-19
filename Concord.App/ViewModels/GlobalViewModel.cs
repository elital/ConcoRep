using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AutoMapper;
using Concord.App.HiddenTabsData;
using Concord.App.Models;
using Concord.Dal.SongEntity;
using Concord.Entities;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;

namespace Concord.App.ViewModels
{
    public class GlobalViewModel
    {
        public SongModel SongSearch { get; set; }
        
        public ObservableCollection<SongModel> Songs { get; set; }

        public SongModel SelectedSong { get; set; }

        public GlobalViewModel()
        {
            SongSearch = new SongModel();
            Songs = new ObservableCollection<SongModel>();
        }

        #region Go button
        
        private DelegateCommand _goCommand;
        public ICommand GoCommand => _goCommand ?? (_goCommand = new DelegateCommand(GoExecuted, GoCanExecute));

        private bool GoCanExecute()
        {
            return true;
        }

        private void GoExecuted()
        {
            var query = Mapper.Map<SongModel, SongQuery>(SongSearch);
            Songs.Clear();
            Songs.AddRange(query.Get().ToList().Select(Mapper.Map<Song, SongModel>));
        }

        #endregion

        #region Double click song

        private DelegateCommand _doubleClickSongCommand;

        public ICommand DoubleClickSongCommand => _doubleClickSongCommand ??
                                                  (_doubleClickSongCommand = new DelegateCommand(DoubleClickSongExecuted, DoubleClickSongCanExecute));

        private bool DoubleClickSongCanExecute()
        {
            return true;
        }

        private void DoubleClickSongExecuted()
        {
            if (!Songs.Any())
            {
                // TODO : set error
                return;
            }

            if (SelectedSong == null)
            {
                // TODO : set error
                return;
            }

            ResultData.Instance.Song.Copy(SelectedSong);
            var mainWindow = (MainWindow) Application.Current.MainWindow;
            mainWindow.HiddenTabFocusAllowed = true;
            mainWindow.GotToTab(mainWindow.SongViewTabName);
        }

        #endregion
    }
}