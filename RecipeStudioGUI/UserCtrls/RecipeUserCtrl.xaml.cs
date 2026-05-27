using RecipesDataAccess.Models;
using RecipeStudioUI.Repositories;
using RecipeStudioUI.ViewModels;
using RecipeStudioUI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for RecipeUserCtrl.xaml
    /// </summary>
    public partial class RecipeUserCtrl : UserControl
    {
        public RecipeUserCtrl()
        {
            InitializeComponent();
        }

    

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {

            var menuItem = sender as MenuItem;      
            var recipe = menuItem?.DataContext as RecipeTableVM;
            if (recipe != null)
            {
                var result = MessageBox.Show(Application.Current.MainWindow,

              $"Are you sure you want to delete '{((RecipeTableVM)recipe)?.CurrentSelectedItem.Name}'?",
              "Confirm Delete",
              MessageBoxButton.YesNo,
              MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    recipe!.FakeDeleteAsync();
                }
            }
        }

        private void MenuItemUpdate_Click(object sender, RoutedEventArgs e)
        {

            var menuItem = sender as MenuItem;
            var recipe = menuItem?.DataContext as RecipeTableVM;
            if (recipe != null)
            {
                var result = MessageBox.Show(Application.Current.MainWindow,

              $"Are you sure you want to delete '{((RecipeTableVM)recipe)?.CurrentSelectedItem.Name}'?",
              "Confirm Delete",
              MessageBoxButton.YesNo,
              MessageBoxImage.Warning);

            }
        }

        private async void MenuItemCategoryEditor_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var recipeTbl = menuItem?.DataContext as RecipeTableVM;
            if (recipeTbl != null)
            {


                var window = new CategoryEditorView
                {
                    Owner = Application.Current.MainWindow
                };

                var editorVM = new CategoryEditorVM(
                    recipeTbl.AllCategories,
                    new ObservableCollection<CategoryDM>(recipeTbl.CurrentSelectedItem.Categories)
                );

                editorVM.CategoryEditCompleted += (s, e) =>
                {
                    window.DialogResult = true; // automatically closes the dialog
                };

                window.DataContext = editorVM;

                bool? result = window.ShowDialog();
                if (result == true)
                {
                    recipeTbl.CurrentSelectedItem.Categories.Clear();
                    foreach (var cat in editorVM.GetSelectedCategoryDMs())
                    {
                        recipeTbl.CurrentSelectedItem.Categories.Add(cat);
                    }

                    await recipeTbl.UpdateAsync(recipeTbl.CurrentSelectedItem);
                }



                //var window = new CategoryEditorView();
                //window.Owner = Application.Current.MainWindow;

                //CategoryEditorVM editorVM = new CategoryEditorVM(
                //    recipeTbl.AllCategories, new ObservableCollection<CategoryDM>(recipeTbl.CurrentSelectedItem.Categories));

                //editorVM.CategoryEditCompleted += (s, e) =>
                //{
                //    // window.Close();
                //    window.DialogResult = true;
                //};
                //window.DataContext = editorVM;


                //window.ShowDialog();
                //if (window.DialogResult == true)
                //{
                //   recipeTbl.CurrentSelectedItem.Categories.Clear();
                //    recipeTbl.CurrentSelectedItem.Categories = editorVM.GetSelectedCategoryDMs();

                //    await recipeTbl.UpdateAsync(recipeTbl.CurrentSelectedItem);

                //}
            }
               
        }
    }
}
