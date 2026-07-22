using RecipeStudio.UI.ViewModels;
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
using System.Windows.Shapes;

namespace RecipeStudio.UI.Views
{
    /// <summary>
    /// Interaction logic for LogInDialog.xaml
    /// </summary>
    public partial class LogInDialog : Window
    {
        public LogInDialog()
        {
            InitializeComponent();
            DataContextChanged += (s, e) =>
            {
                if (e.NewValue is LogInVM vm)
                {
                   
                    vm.LoggedIn += (sender, args) => Close();
                }
            };

        }
    }
}
