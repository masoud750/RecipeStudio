using Microsoft.EntityFrameworkCore;
using RecipesDataAccess.Data;
using RecipeStudioUI.Repositories;
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
    /// Interaction logic for RegisterUserCtrl.xaml
    /// </summary>
    public partial class RegisterUserCtrl : UserControl
    {
        public RegisterUserCtrl()
        {
            InitializeComponent();
            
        }

        private void DoSth(object sender, RoutedEventArgs e)
        {
            if(sender != null)
            {
                var dataContex = this.DataContext;
                if(dataContex is RegisterVM vm)
                {
                    string txtEmail = vm.Email;
                }
            }
        }

        private void pwdBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if( sender is PasswordBox pwdBox)
            {
               string password = pwdBox.Password;
                if (this.DataContext is RegisterVM vm)
                {
                    vm.Password = password;
                }
            }

        }
    }
}
