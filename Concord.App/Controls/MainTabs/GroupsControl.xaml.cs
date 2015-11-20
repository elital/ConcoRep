using System.Windows.Controls;
using Concord.App.Models;
using Concord.App.ViewModels;

namespace Concord.App.Controls.MainTabs
{
    /// <summary>
    /// Interaction logic for GroupsControl.xaml
    /// </summary>
    public partial class GroupsControl : BaseTabItem
    {
        public GroupsControl()
        {
            InitializeComponent();
        }

        public override void WordDoubleClicked(WordModel word)
        {
            (DataContext as GroupsViewModel).AppendWord(word);
        }
    }
}
