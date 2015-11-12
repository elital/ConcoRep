using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
        public static readonly DependencyProperty IsReadonlyProperty = DependencyProperty.RegisterAttached("IsReadonly", typeof(Boolean), typeof(LoadControl),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

        public static void SetIsReadonly(UIElement element, bool value)
        {
            element.SetValue(IsReadonlyProperty, value);
        }

        public static bool GetIsReadonly(UIElement element)
        {
            return (bool)element.GetValue(IsReadonlyProperty);
        }

        public bool IsReadonly
        {
            get { return (bool)GetValue(IsReadonlyProperty); }
            set { SetValue(IsReadonlyProperty, value); }
        }

        public LoadControl()
        {
            InitializeComponent();
        }

        private void LoadControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (IsReadonly)
            {
                Template = new ControlTemplate();

                SetReadOnly();

                LoadNewSong.Visibility = Visibility.Collapsed;
                SaveNewSong.Visibility = Visibility.Collapsed;
            }
        }

        private void SetReadOnly()
        {
            SongTitle.IsReadOnly = true;
            AuthorName.IsReadOnly = true;
            AlbumName.IsReadOnly = true;
            PublishDate.IsEnabled = false;
            LyricsText.IsReadOnly = true;
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
                    var data = text.Split(new[] {Environment.NewLine}, 2, StringSplitOptions.None);
                    var details = data[0].Split(';');

                    LyricsText.Clear();
                    LyricsText.Text = data[1];
                    SongTitle.Text = details[0];
                    AuthorName.Text = details[1];
                    AlbumName.Text = details[2];
                    PublishDate.SelectedDate = DateTime.ParseExact(details[3], "dd/MM/yyyy", CultureInfo.CurrentCulture);
                }
            }
        }
    }
}
