using System;
using System.Windows;
using System.Windows.Threading;
using Concord.App.Mapping;
using Concord.Dal;

namespace Concord.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            SongMapping.MapSong();
            WordMapping.MapWord();
            GroupMapping.MapGroup();
            ContextMapping.MapContext();
            RelationMapping.MapRelation();

            OracleDataLayer.Instance.Connect();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            ClosingActions();
        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"An error occured.{Environment.NewLine}The app will close now.", "Oops...", MessageBoxButton.OK, MessageBoxImage.Error);
            ClosingActions();
            Current.MainWindow.Close();
        }

        private void ClosingActions()
        {
            OracleDataLayer.Instance.Disconnect();
        }
    }
}
