using Microsoft.EntityFrameworkCore.Query;
using RecipesDataAccess.Models;
using RecipeStudioUI.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace RecipeStudioUI.ViewModels
{
    internal class AddRecipeEditorVM: ModelBase
    {
        public AddRecipeEditorVM(
            ItemChooserVM<CategoryVM> categoryChooser,
            ItemChooserVM<StepVM> instructionChooser,
             ItemChooserVM<IngredientVM> ingredientChooser)
        {
            _categoryChooser = categoryChooser ??
                throw new ArgumentNullException(nameof(categoryChooser));
            _instructionChooser = instructionChooser ??
                throw new ArgumentNullException(nameof(instructionChooser));

            _ingredientChooser = ingredientChooser ?? throw new 
                ArgumentNullException(nameof(ingredientChooser));

        }

        public string RecipeName
        {
            get
            { return _recipeName; }
            set
            {
                SetProperty(ref _recipeName, value);
            }
        }

        private string _recipeName = string.Empty;


        public string Description
        {
            get
            { return _descr; }
            set
            {
                SetProperty(ref _descr, value);
            }
        }

        private string _descr;

        public ItemChooserVM<CategoryVM> CategoryChooser
        {
            get { return _categoryChooser; }
        }


        private ItemChooserVM<CategoryVM> _categoryChooser;



        public ItemChooserVM<StepVM> StepChooser
        {
            get { return _instructionChooser; }
        }

        private ItemChooserVM<StepVM> _instructionChooser;

        public ItemChooserVM<IngredientVM> IngredientChooser
        {
            get { return _ingredientChooser; }
        }


        private ItemChooserVM<IngredientVM> _ingredientChooser;


        public ICommand AddRecipeCmd
        {
            get
            {
                if (_addRecipeCmd == null)
                {
                    _addRecipeCmd = new RelayCommand(execute: _ => {

                        OnAddRecipe();
                    },
                    _ => { return IsValidRecipe(); });
                }

                return _addRecipeCmd;
            }

        } 


        private ICommand? _addRecipeCmd;

        private void OnAddRecipe()
        {
            AddRecipe?.Invoke(this, new AddRecipeEventArgs(
                _recipeName,
                _descr,
                CategoryChooser.SelectedItems,
                IngredientChooser.SelectedItems,
                StepChooser.SelectedItems));
        }

        public event EventHandler<AddRecipeEventArgs> AddRecipe;


        private bool IsValidRecipe() {

            return !String.IsNullOrEmpty(_recipeName) &&
                   CategoryChooser.SelectedItems.Count >= 1 &&
                   IngredientChooser.SelectedItems.Count >= 1 &&
                   StepChooser.SelectedItems.Count >= 1;
        
        }

        
    }

    public class AddRecipeEventArgs : EventArgs
    {

        public AddRecipeEventArgs(
            string recipeName,
            string descr,
            ICollection<CategoryVM> categories,
            ICollection<IngredientVM> ingredients,
                ICollection<StepVM>steps)
        {

            if (string.IsNullOrEmpty(nameof(recipeName))){
                throw new ArgumentNullException(nameof(recipeName));
            }
             Name = recipeName;
            Description = descr;
            SelectedCategories = categories ??
                throw new ArgumentNullException(nameof(categories));

            SelectedIngredients = ingredients ??
                throw new ArgumentNullException(nameof(ingredients));

            SelectedInstructions = steps ??
                throw new ArgumentNullException(nameof(steps));



        }

        public string Name { get; }
        public string? Description { get; }
        public ICollection<CategoryVM> SelectedCategories { get; }
        public ICollection<IngredientVM> SelectedIngredients { get; }
      
        public ICollection<StepVM> SelectedInstructions { get; }


    }
}
