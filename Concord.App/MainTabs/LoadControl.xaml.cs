using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Concord.App.MainTabs
{
    /// <summary>
    /// Interaction logic for LoadControl.xaml
    /// </summary>
    public partial class LoadControl : TabItem
    {
        public LoadControl()
        {
            InitializeComponent();
        }

        private void SaveNewSong_OnClick(object sender, RoutedEventArgs e)
        {
            var lines = LyricsText.Text.Split(new[] {Environment.NewLine}, StringSplitOptions.None);

            throw new NotImplementedException();
        }

        private void LoadNewSong_OnClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
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
                    messageBoxResult = MessageBox.Show("The chosen file is not found, please choose a different file.", "File not found", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
                catch
                {
                    messageBoxResult = MessageBox.Show("An error occured while openning the file, please try again.", "An error occured", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }

                if (messageBoxResult == MessageBoxResult.Cancel)
                    return;

                if (isValid)
                {
                    LyricsText.Clear();
                    LyricsText.Text = text;
                }
            }
        }
    }
}
