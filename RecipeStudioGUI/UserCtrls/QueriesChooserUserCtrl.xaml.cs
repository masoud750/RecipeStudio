using RecipeStudioUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecipeStudioUI.UserCtrls
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
