using Concord.App.Models;
using Concord.App.ViewModels;

namespace Concord.App.Controls.MainTabs
{
    /// <summary>
    /// Interaction logic for RelationsControl.xaml
    /// </summary>
    public partial class RelationsControl : BaseTabItem
    {
        public RelationsControl()
        {
            InitializeComponent();
        }

        public override void WordDoubleClicked(WordModel word)
        {
            (DataContext as RelationsViewModel).AppendWord(word);
        }
    }
}
