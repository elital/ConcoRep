using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Concord.App.HiddenTabsData;
using Concord.App.Models;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;

namespace Concord.App.ViewModels
{
    public class ContextViewModel
    {
        public ObservableCollection<ContextModel> Contexts { get; set; }

        public ContextViewModel()
        {
            Contexts = new ObservableCollection<ContextModel>();
        }

        private DelegateCommand mainDockLoadedCommand;
        public ICommand MainDockLoadedCommand
        {
            get
            {
                if (mainDockLoadedCommand == null)
                    mainDockLoadedCommand = new DelegateCommand(MainDockLoadedExecuted, MainDockLoadedCanExecute);

                return mainDockLoadedCommand;
            }
        }

        public bool MainDockLoadedCanExecute()
        {
            return true;
        }

        public void MainDockLoadedExecuted()
        {
            Contexts.Clear();

            if (ResultData.Instance.Contexts != null)
                Contexts.AddRange(ResultData.Instance.Contexts);
        }

        private DelegateCommand mainDockUnloadedCommand;

        public ICommand MainDockUnloadedCommand
        {
            get
            {
                if (mainDockUnloadedCommand == null)
                    mainDockUnloadedCommand = new DelegateCommand(MainDockUnloadedExecuted, MainDockUnloadedCanExecute);

                return mainDockUnloadedCommand;
            }
        }

        public bool MainDockUnloadedCanExecute()
        {
            return true;
        }

        public void MainDockUnloadedExecuted()
        {
            ResultData.Instance.Contexts.Clear();

            ((MainWindow) Application.Current.MainWindow).HiddenTabFocusAllowed = false;
        }
    }
}