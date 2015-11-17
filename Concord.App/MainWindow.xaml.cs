using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Concord.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Tab names
        public string SongViewTabName => SongView.Name;
        public string ContextTabName => Context.Name;
        public string SongLoadTabName => SongLoad.Name;
        #endregion
        
        public MainWindow()
        {
            // TODO : Move to the app start
            SongMapping.MapSong();

            InitializeComponent();
            HiddenTabFocusAllowed = false;
        }

        public bool HiddenTabFocusAllowed { get; set; }

        private void HiddenTab_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!HiddenTabFocusAllowed)
                MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;
        }

        public bool GotToTab(string tabName)
        {
            var tabItem = MainTabControl.SelectedItem as TabItem;

            if (tabItem != null && tabItem.Name == tabName)
                return false;

            foreach (object item in MainTabControl.Items)
            {
                if (!(item is TabItem))
                    continue;

                var tab = (TabItem)item;

                if (tab.Name == tabName)
                {
                    MainTabControl.SelectedIndex = tab.TabIndex;
                    return true;
                }
            }

            return false;
        }

    }
}
