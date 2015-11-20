using Concord.App.Models;
using Concord.App.ViewModels;

namespace Concord.App.Controls.MainTabs
{
    /// <summary>
    /// Interaction logic for GlobalControl.xaml
    /// </summary>
    public partial class GlobalControl : BaseTabItem
    {
        public GlobalControl()
        {
            InitializeComponent();
        }
        
        public override void WordDoubleClicked(WordModel word)
        {
            (DataContext as GlobalViewModel).AppendWord(word);
        }
    }
}
