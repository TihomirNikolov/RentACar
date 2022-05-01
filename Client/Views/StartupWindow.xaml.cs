using Client.ViewModels;
using System.Windows;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            this.DataContext = new StartupViewModel();
            InitializeComponent();
        }
    }
}
