using System;
using System.Windows;
using System.Windows.Controls;
using Concord.App.ViewModels;

namespace Concord.App.Controls.MainTabs
{
    /// <summary>
    /// Interaction logic for LoadControl.xaml
    /// </summary>
    public partial class LoadControl : TabItem
    {
        #region IsReadonly

        public static readonly DependencyProperty IsReadonlyProperty =
            DependencyProperty.RegisterAttached("IsReadonly", typeof (Boolean), typeof (LoadControl),
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

        #endregion

        public LoadControl()
        {
            InitializeComponent();
        }

        private void LoadControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (IsReadonly)
                SetReadOnly();

            ((LoadViewModel) DataContext).Properties.Readonly = IsReadonly;
        }

        private void SetReadOnly()
        {
            Template = new ControlTemplate();
        }
    }
}
