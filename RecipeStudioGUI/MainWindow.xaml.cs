using RecipeStudioUI.Listeners;
using RecipeStudioUI.UserCtrls;
using RecipeStudioUI.ViewModels;
using System.Diagnostics;
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

namespace RecipesGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainVM vm)
        {
#if DEBUG                      
            BindingErrorTraceListener.SetTrace();
#endif
            InitializeComponent();
            DataContext = vm;

        }

        private async void  Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainVM mainVm)
            {
                await mainVm.ColumnsVM.InitializeAsync();
            }
        }
    }
}