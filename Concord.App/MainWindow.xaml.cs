using System;
using System.Collections.Generic;
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

namespace Concord.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            HiddenTabFocusAllowed = false;
        }

        public bool HiddenTabFocusAllowed { get; set; }

        private void HiddenTab_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!HiddenTabFocusAllowed)
                MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;
        }

    }
}
