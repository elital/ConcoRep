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
                SetReadOnly();
            }
        }

        private void SetReadOnly()
        {
            Template = new ControlTemplate();

            LoadNewSong.Visibility = Visibility.Collapsed;
            SaveNewSong.Visibility = Visibility.Collapsed;

            SongTitle.IsReadOnly = true;
            AuthorName.IsReadOnly = true;
            AlbumName.IsReadOnly = true;
            PublishDate.IsEnabled = false;
            LyricsText.IsReadOnly = true;
        }
    }
}
