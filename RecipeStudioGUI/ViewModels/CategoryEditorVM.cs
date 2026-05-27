using RecipesDataAccess.Models;
using RecipeStudioUI.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RecipeStudioUI.ViewModels
{
    internal class CategoryEditorVM : ModelBase
    {
        /// <summary>
        /// ViewModel wrapper for <see cref="CategoryDM"/> used in the Category Editor.
        /// </summary>
        public CategoryEditorVM(ObservableCollection<CategoryDM> availableCategories,
            ObservableCollection<CategoryDM> selectedCategories
           ) { 
            ArgumentNullException.ThrowIfNull(selectedCategories,
                nameof(selectedCategories));
            ArgumentNullException.ThrowIfNull(availableCategories, 
                nameof(availableCategories));

            SelectedCategories = new ObservableCollection<CategoryVM>(
        selectedCategories.Select(dm => new CategoryVM(dm)));

            AvailableCategories = new ObservableCollection<CategoryVM>(
                availableCategories.Select(dm => new CategoryVM(dm)));

            SynchronizeSelections();
        }

        public ObservableCollection<CategoryVM> SelectedCategories {  get; set; }
        public ObservableCollection<CategoryVM> AvailableCategories { get; set; }

        private void SynchronizeSelections()
        {

            foreach( var avilable in AvailableCategories)
            {
                avilable.PropertyChanged += HandlePropertyChnaged;

            }

            // Ensure all selected VMs are marked
            foreach (var selected in SelectedCategories)
            {
                selected.IsSelected = true;
            }

           

            foreach (var available in AvailableCategories)
            {
                if (SelectedCategories.Any(x => x.Name == available.Name))
                {
                    available.IsSelected = true;
                }
            }
        }

        private void HandlePropertyChnaged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is CategoryVM vm && e?.PropertyName == nameof(CategoryVM.IsSelected))
            {
                var existing = SelectedCategories.FirstOrDefault(x => x.Name == vm.Name);

                if (vm.IsSelected)
                {
                    // only add if not already present
                    if (existing == null)
                        SelectedCategories.Add(vm);
                }
                else
                {
                    // only remove if present
                    if (existing != null)
                        SelectedCategories.Remove(existing);
                }
            }
        }




        public ICommand ClearCmd
        {
            get
            {
                if (_clearCmd == null)
                {

                    _clearCmd = new RelayCommand(
                    execute: _ =>
                    {
                        ClearAllSelectedItemsInAvailableCategories();


                    }
                   ,
                    canExecute: _ => {
                        return IsLogInDone &&

                          AvailableCategories.Any(x => x.IsSelected);
                        
                        }
                   );
                }
                return _clearCmd;
            }

        }

        private ICommand _clearCmd;

        private void ClearAllSelectedItemsInAvailableCategories()
        {
            foreach (var available in AvailableCategories)
            {
                available.IsSelected = false;
            }

        }

        public ICommand SelectAllCmd
        {
            get
            {
                if (_selectAllCmd == null)
                {

                    _selectAllCmd = new RelayCommand(
                    execute: _ =>
                    {
                        SelectAllAvailableCategories();
                    }
                   ,
                    canExecute: _ => {
                        return IsLogInDone &&
                          !AvailableCategories.All(x => x.IsSelected);
                        }
                   );
                }
                return _selectAllCmd;
            }

        }
        private ICommand _selectAllCmd;

        private void SelectAllAvailableCategories()
        {
            foreach (var available in AvailableCategories)
            {
                available.IsSelected = true;
            }
        }


     
        
        public ICommand SaveCmd
        {
            get
            {
                if (_saveCmd == null)
                {

                    _saveCmd = new RelayCommand(
                    execute: _ =>
                    {
                        FinishedEditingCategories();
                    }
                   ,
                    canExecute: _ => {
                        return IsLogInDone &&
                          SelectedCategories.Count >= 1;
                    }
                   );
                }
                return _saveCmd;
            }

        }
        private ICommand _saveCmd;


        public void FinishedEditingCategories()
        {
            CategoryEditCompleted?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CategoryEditCompleted;


        public ICommand RemoveDeletedCategoriesFromRecipeCmd
        {
            get
            {
                if (_removeCmd == null)
                {

                    _removeCmd = new RelayCommand(
                    execute: _ =>
                    {
                        var expiredItems = 
                        SelectedCategories.Where(c => c.Model.IsExpired).ToList();

                        foreach (var item in expiredItems)
                        {
                            SelectedCategories.Remove(item);
                        }
                    }
                   ,
                    canExecute: _ => {
                        return IsLogInDone &&
                          SelectedCategories.Any(x => x.Model.IsExpired == true);
                    }
                   );
                }
                return _removeCmd;
            }

        }
        private ICommand _removeCmd;


        public ObservableCollection<CategoryDM> GetSelectedCategoryDMs()
        {
            return new ObservableCollection<CategoryDM>(
                SelectedCategories.Select(vm => vm.Category)
            );
        }
    }
}
