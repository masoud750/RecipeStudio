using RecipeStudioUI.Commands;
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
using System.Windows.Shapes;

namespace RecipeStudioUI.Views
{
    /// <summary>
    /// Interaction logic for EditDialog.xaml
    /// </summary>
    public partial class EditDialog : Window
    {
        public EditDialog()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
       
            if (e.NewValue is IValidatable validatable)
            {
                // Subscribe to event
                validatable.Done += (s, args) =>
                {
                    // react to validity changes
                    if (!validatable.IsValid)
                    {
                       Close();
                    }
                };
            }
        
        }

   

    

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is IValidatable v && v.IsValid)
            {
                this.DialogResult = true; // closes dialog and returns true
            }
            else
            {
                this.Close(); // just closes
            }
        }
    }
}
