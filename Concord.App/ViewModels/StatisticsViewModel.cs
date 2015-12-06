using System.Windows.Input;
using AutoMapper;
using Concord.App.Models;
using Concord.Dal.General;
using Microsoft.Practices.Prism.Commands;

namespace Concord.App.ViewModels
{
    public class StatisticsViewModel
    {
        public SystemStatisticsModel SystemStatistics { get; set; }

        public StatisticsViewModel()
        {
            SystemStatistics = new SystemStatisticsModel();
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
            var systemStat = StatisticsQuery.Instance.GetSystemStatistics();
            Mapper.Map(systemStat, SystemStatistics);
        }

        #endregion
    }
}