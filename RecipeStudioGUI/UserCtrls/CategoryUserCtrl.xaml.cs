using Microsoft.EntityFrameworkCore;
using RecipeStudioUI.ViewModels;
using RecipeStudioUI.Views;
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
    /// Interaction logic for CategoriesUserCtrl.xaml
    /// </summary>
    public partial class CategoryUserCtrl : UserControl
    {
        public CategoryUserCtrl()
        {
            InitializeComponent();
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true; // prevent default behavior
            }
        }



        private   void  MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {

            var menuItem = sender as MenuItem;
            var category = menuItem?.DataContext as CategoryTableVM;




            if (category is CategoryTableVM vm && vm.CurrentSelectedItem != null)
            {
                var result = MessageBox.Show(Application.Current.MainWindow,
                    $"Are you sure you want to delete '{vm.CurrentSelectedItem.Name}'?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                if(result == MessageBoxResult.Yes)
                {
                     vm.FakeDelete();
                }
               
            }
            else
            {
                MessageBox.Show("No item selected to delete.");
            }

        }


        private void MenuItemEdit_Click(object sender, RoutedEventArgs e)
        {

            var menuItem = sender as MenuItem;
            var categoryTbl = menuItem?.DataContext as CategoryTableVM;
            if (categoryTbl != null)
            {



                var dialog = new EditDialog
                {
                    Owner = Application.Current.MainWindow,
                    DataContext = categoryTbl.CurrentSelectedItem,
                    Title = "Category Editor"
                };


                if (dialog.ShowDialog() == true)
                {


                    string oldName = categoryTbl.CurrentSelectedItem.Name;

                    string name = categoryTbl.CurrentSelectedItem.UpdateName;
                    if (DataContext is CategoryTableVM tableVM)
                    {

                        try
                        {
                            name = name.Replace(" ", "");
                            tableVM.CurrentSelectedItem.Name = name.ToLowerInvariant();
                            tableVM.Repository.SaveChanges();
                            tableVM.OnCategoryNameChanged(); 

                        }
                        catch (DbUpdateException)
                        {
                            // revert to old value
                            tableVM.CurrentSelectedItem.Name = oldName;
                            tableVM.CurrentSelectedItem.UpdateName = oldName;
                            // notify user
                            MessageBox.Show("Invalid category name. Reverted to previous value.");
                        }

                    }

                }



            }
        }

        private async void MenuItemAdd_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var categoryTbl = menuItem?.DataContext as CategoryTableVM;
            if (categoryTbl != null)
            {
                var newCategory = new CategoryVM(new RecipesDataAccess.Models.CategoryDM()
                {
                    Name = string.Empty
                });
                var dialog = new EditDialog
                {
                    Owner = Application.Current.MainWindow,
                    DataContext = newCategory,
                    Title = "Category Editor"
                };
              

                if (dialog.ShowDialog() == true)
                {

                    if (DataContext is CategoryTableVM tableVM)
                    {

                        try
                        {

                            newCategory.UpdateName.ToLowerInvariant();
                            newCategory.Name = newCategory.UpdateName;

                            var existingCategory = await tableVM.Repository.GetByNameAsync(newCategory.Name);

                            if (existingCategory != null && existingCategory.IsExpired)
                            {
                                existingCategory.ValidFrom = DateTime.Now;
                                existingCategory.ValidTo = tableVM.ResetValidToTimeStamp();
                                await tableVM.Repository.SaveChangesAsync();                                                               
                                MessageBox.Show("Category added successfully.");
                                tableVM.Categories.Add(new CategoryVM(existingCategory));
                            }                        
                            else 
                            {
                                await tableVM.AddAsync(category: newCategory.Model);
                                await tableVM.Repository.SaveChangesAsync();
                                var existing = tableVM.Categories.FirstOrDefault(newCategory);
                                if (existing != null)
                                {
                                    MessageBox.Show("Category add successfully.");
                                }

                            }

                        }
                        catch (DbUpdateException)
                        {                        
                          
                            // notify user
                            MessageBox.Show("Invalid category name.","Error");
                        }

                    }

                }
            }

        }
    }
}
