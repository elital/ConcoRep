using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Concord.App.Controls.MainTabs;

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
        public string StatisticsTabName => Statistics.Name;
        #endregion

        public BaseTabItem SelectedTab => MainTabControl.SelectedItem as BaseTabItem;

        public Action RefreshWordAction { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            HiddenTabFocusAllowed = false;
        }

        public bool HiddenTabFocusAllowed { get; set; }

        private void HiddenTab_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!HiddenTabFocusAllowed)
                GotToTab(StatisticsTabName);
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
