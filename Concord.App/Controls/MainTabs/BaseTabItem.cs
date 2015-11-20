using System;
using System.Windows.Controls;
using Concord.App.Models;

namespace Concord.App.Controls.MainTabs
{
    public abstract class BaseTabItem : TabItem
    {
        public abstract void WordDoubleClicked(WordModel word);
    }
}