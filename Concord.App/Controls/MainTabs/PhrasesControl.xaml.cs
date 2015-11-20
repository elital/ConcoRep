using System.Windows.Controls;
using Concord.App.Models;
using Concord.App.ViewModels;

namespace Concord.App.Controls.MainTabs
{
    /// <summary>
    /// Interaction logic for PhrasesControl.xaml
    /// </summary>
    public partial class PhrasesControl : BaseTabItem
    {
        public PhrasesControl()
        {
            InitializeComponent();
        }

        public override void WordDoubleClicked(WordModel word)
        {
            (DataContext as PhrasesViewModel).AppendWord(word);
        }
    }
}
