using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KonturTest.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new KonturTest.ViewModels.MainViewModel();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (DataContext is ViewModels.MainViewModel vm)
            {
                vm.SaveSettings();
            }
            base.OnClosing(e);
        }
    }
}