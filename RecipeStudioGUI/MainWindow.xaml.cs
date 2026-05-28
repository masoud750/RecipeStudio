using System.Windows;
using RecipeStudio.UI.Listeners;
using RecipeStudio.UI.ViewModels;



namespace RecipeStudio.UI
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