using System.Windows;
using System.Windows.Controls;
using RecipeStudio.UI.ViewModels;


namespace RecipeStudio.UI.UserCtrls
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
