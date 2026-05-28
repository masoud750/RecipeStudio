using RecipeStudio.Domain.Entities;
using NUnit.Framework.Internal;
using System.Collections.ObjectModel;
using System.Windows.Input;
using RecipeStudio.UI.ViewModels;

namespace RecipesGUITests.ViewModels

{
    [TestFixture]
    internal class AddRecipesEditorVMTests
    {

        [SetUp]
        public void Setup()
        {
            _categoryColumn = new ColumnVM<CategoryVM>(
                new ObservableCollection<CategoryVM>(), "Category");
            _instructionColumn = new ColumnVM<StepVM>(
                new ObservableCollection<StepVM>(), "Instruction");
            _ingredientColumn = new ColumnVM<IngredientVM>(
                new ObservableCollection<IngredientVM>(), "Ingredient");
        }


        ColumnVM<CategoryVM> _categoryColumn;
        ColumnVM<StepVM> _instructionColumn;
        ColumnVM<IngredientVM> _ingredientColumn;

        [Test]
        public void AddRecipeCommand_CanExecute_InvalidInput_ReturnsFalse()
        {

            var editor = new AddRecipeEditorVM(
                new ItemChooserVM<CategoryVM>(_categoryColumn),
                new ItemChooserVM<StepVM>(_instructionColumn),
                new ItemChooserVM<IngredientVM>(_ingredientColumn));

            ICommand command = editor.AddRecipeCmd;
            Assert.IsFalse(condition: (command).CanExecute(editor));

        }

        [Test]
        public void AddRecipeCommand_CanExecute_ValidInput_ReturnTrue()
        {
            var category = new CategoryVM(new CategoryDM() { Name = "Breakfast" });
            _categoryColumn.Items.Add(category);
            var instruction = new StepVM(new StepDM { Name = "Boiling" });
            _instructionColumn.Items.Add(instruction);
            var ingredient = new IngredientVM(new IngredientDM()
            {
                Name = "Milk",
                Quantity = "200ml"
            });
            _ingredientColumn.Items.Add(ingredient);

            var editor = new AddRecipeEditorVM(
               new ItemChooserVM<CategoryVM>(_categoryColumn),
               new ItemChooserVM<StepVM>(_instructionColumn),
               new ItemChooserVM<IngredientVM>(_ingredientColumn));

            editor.IngredientChooser.SelectedItems.Add(ingredient);
            editor.CategoryChooser.SelectedItems.Add(category);
            editor.StepChooser.SelectedItems.Add(instruction);
            string name = TestContext.CurrentContext.Test.Name;
            editor.RecipeName = name;
            ICommand command = editor.AddRecipeCmd;
            Assert.IsTrue(condition: (command).CanExecute(editor));
        }

        [Test]
        public void AddRecipeCommand_Execute_RaiseEvent()
        {
            var category = new CategoryVM(new CategoryDM() { Name = "Breakfast" });
            _categoryColumn.Items.Add(category);
            var instruction = new StepVM(new StepDM { Name = "Boiling" });
            _instructionColumn.Items.Add(instruction);
            var ingredient = new IngredientVM(new IngredientDM()
            {
                Name = "Milk",
                Quantity = "200ml"
            });
            _ingredientColumn.Items.Add(ingredient);

            var editor = new RecipeStudio.UI.ViewModels.AddRecipeEditorVM(
               new ItemChooserVM<CategoryVM>(_categoryColumn),
               new ItemChooserVM<StepVM>(_instructionColumn),
               new ItemChooserVM<IngredientVM>(_ingredientColumn));

            editor.IngredientChooser.SelectedItems.Add(ingredient);
            editor.CategoryChooser.SelectedItems.Add(category);
            editor.StepChooser.SelectedItems.Add(instruction);
            string name = TestContext.CurrentContext.Test.Name;
            editor.RecipeName = name;
            ICommand command = editor.AddRecipeCmd;
            bool isTriggered = false;
            editor.AddRecipe += (s, e) =>
            {
                Assert.IsNotNull(e);
                isTriggered = true;
            };
            command.Execute(editor);
            Assert.IsTrue(isTriggered);
        }

        [Test]
        public void CreateInstance_InvalidCategoryChooser_ThrowsArgumentNullException()
        {

            var instruction = new StepVM(new StepDM { Name = "Boiling" });
            _instructionColumn.Items.Add(instruction);
            var ingredient = new IngredientVM(new IngredientDM()
            {
                Name = "Milk",
                Quantity = "200ml"
            });
            _ingredientColumn.Items.Add(ingredient);

            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                new RecipeStudio.UI.ViewModels.AddRecipeEditorVM(
               categoryChooser: null!,
               new ItemChooserVM<StepVM>(_instructionColumn),
               new ItemChooserVM<IngredientVM>(_ingredientColumn));
            });

            Assert.IsTrue(ex.Message.Contains("category"));
        }

        [Test]
        public void CreateInstance_InvalidInstructionChooser_ThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () =>
                {
                    var category = new CategoryVM(new CategoryDM() { Name = "Breakfast" });
                    _categoryColumn.Items.Add(category);

                    var ingredient = new IngredientVM(new IngredientDM()
                    {
                        Name = "Milk",
                        Quantity = "200ml"
                    });
                    _ingredientColumn.Items.Add(ingredient);

                    var editor = new RecipeStudio.UI.ViewModels.AddRecipeEditorVM(
                       new ItemChooserVM<CategoryVM>(_categoryColumn),
                       null!,
                       new ItemChooserVM<IngredientVM>(_ingredientColumn));

                });
            Assert.IsTrue(ex.Message.Contains("instruction"));
        }

        [Test]
        public void CreateInstance_InvalidIngredientChooser_ThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () =>
                {
                    var category = new CategoryVM(new CategoryDM() { Name = "Breakfast" });
                    _categoryColumn.Items.Add(category);
                    var instruction = new StepVM(new StepDM { Name = "Boiling" });
                    _instructionColumn.Items.Add(instruction);
                  

                    var editor = new AddRecipeEditorVM(
                       new ItemChooserVM<CategoryVM>(_categoryColumn),
                       new ItemChooserVM<StepVM>(_instructionColumn),
                       null!);
                });
            Assert.IsTrue(ex.Message.Contains("ingredient"));
        }

    }


}
