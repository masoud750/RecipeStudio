using RecipeStudio.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;


namespace RecipeStudio.UI.UserCtrls
{
    /// <summary>
    /// Interaction logic for QueriesChooser.xaml
    /// </summary>
    public partial class QueriesChooserUserCtrl : UserControl
    {
        public QueriesChooserUserCtrl()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null && 
                this.DataContext is RecipeQueryTableVM vm)
            {
              
            }
          
        }
    }
}
