using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using RecipeStudio.Domain.Entities;
using RecipeStudio.Repository.Interfaces;
using RecipeStudio.UI.Helpers;
using RecipeStudio.UI.Views;
using System.Collections.ObjectModel;
using RecipeStudio.UI.Commands;


namespace RecipeStudio.UI.ViewModels
{
    // Design-time RecipeVM
    internal class DesignRecipeVM : RecipeTableVM
    {
        public DesignRecipeVM()
            : base(new FakeRecipeRepository(),
                  new DesignMessageServiceFake())
        {
            Recipes.Add(new RecipeDM { Id = 1, Name = "Chocolate Cake" });
            Recipes.Add(new RecipeDM { Id = 2, Name = "Spaghetti Bolognese" });
            Recipes.Add(new RecipeDM { Id = 3, Name = "Caesar Salad" });
        }
    }

    public class RecipeTableVM : ModelBase
    {
      

        public RecipeTableVM(
            IRepository<RecipeDM> repository,           
            IMessageService msgService)
        {
           
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));             
            _msgService = msgService ?? throw new ArgumentNullException(nameof(msgService));
          
        }

 
        private readonly IRepository<RecipeDM> _repository;
        private readonly IMessageService _msgService;
        public TableCollectionVM CollectionVM
        {
            get;
        }
        private readonly TableCollectionVM? _collectionVM;

      

        public ObservableCollection<RecipeDM> Recipes { get; set; }= new ();
        public ObservableCollection<IngredientDM> AllIngredients { get; set; } = new();
        public ObservableCollection<StepDM> AllSteps { get; set; } = new();

        public ObservableCollection<CategoryDM> AllCategories { get; set; } = new();

        public async Task InitializeCurrentUserRecipesAsync()
        {
            try
            {
                Recipes.Clear();
                var recipes = await _repository.GetAllAsync();
                foreach (var recipe in recipes)
                    if (!recipe.IsExpired && recipe.User.UserName.ToLowerInvariant() == 
                            CurrentUser?.UserName.ToLowerInvariant())
                    {
                        Recipes.Add(recipe);
                    }
                    

                //_msgService.ShowMessage("Recipes loaded successfully."
                //    , Helpers.MessageType.Info, "Info");
            }
            catch (Exception ex)
            {
                _msgService.ShowMessage(
                    $"Error while loading recipes: {ex.Message}",
                     Helpers.MessageType.Error, "Initialize Data");
            }
        }


        public async Task InitializeAsync()
        {
            try
            {
                Recipes.Clear();
                var recipes = await _repository.GetAllAsync();
                foreach (var recipe in recipes)
                    if (!recipe.IsExpired)
                    {
                        Recipes.Add(recipe);
                    }


                //_msgService.ShowMessage("Recipes loaded successfully."
                //    , Helpers.MessageType.Info, "Info");
            }
            catch (Exception ex)
            {
                _msgService.ShowMessage(
                    $"Error while loading recipes: {ex.Message}",
                     Helpers.MessageType.Error, "Initialize Data");
            }
        }


        public async Task AddAsync(RecipeDM recipe)
        {
            try
            {
                await _repository.AddAsync(recipe);
                Recipes.Add(recipe);
                _msgService.ShowMessage("Recipe added successfully.", 
                    Helpers.MessageType.Info, "Add Data");
            }
            catch (Exception ex)
            {
                _msgService.ShowMessage($"Error while adding recipe: {ex.Message}", 
                    Helpers.MessageType.Error, "Add Data");
            }
        }

        public async Task UpdateAsync(RecipeDM recipe)
        {
            try
            {
                await _repository.UpdateAsync(recipe);
                List < RecipeDM > recipes = (List<RecipeDM>)await _repository.GetAllAsync();
                var existing = Recipes.FirstOrDefault(r => r.Id == recipe.Id);
                if (existing != null)
                {
                    Recipes.Clear();
                    var subRecipes = recipes.Where(x => x.User.UserName == CurrentUser?.UserName
                       && x.IsExpired == false);
                       
                    foreach( var item in subRecipes)
                    {
                        Recipes.Add(item);
                    }


                    _msgService.ShowMessage("Recipe updated successfully.",
                     Helpers.MessageType.Info, "Info");

                }

                
            }
            catch (Exception ex)
            {
                _msgService.ShowMessage(
                    $"Error while updating recipe: {ex.Message}", 
                    Helpers.MessageType.Error, "Update Data");
            }
        }

        public async Task DeleteAsync(RecipeDM recipe)  
        {
            try
            {
                await _repository.DeleteAsync(recipe.Id);
                Recipes.Remove(recipe);
                _msgService.ShowMessage("Recipe deleted successfully.", 
                    Helpers.MessageType.Info, "Delete Data");
            }
            catch (Exception ex)
            {
                _msgService.ShowMessage(
                    $"Error while deleting recipe: {ex.Message}",
                      Helpers.MessageType.Error, "Delete Data");
            }
        }




  


        public void FakeDeleteAsync()
        {
            try
            {

                CurrentSelectedItem.ValidTo = DateTime.Now; // or set IsExpired flag
                _repository.SaveChanges();
                Recipes.Remove(CurrentSelectedItem);
                _msgService.ShowMessage("Recipe deleted successfully.",
                    Helpers.MessageType.Info, "Delete Data");
            }
            catch (Exception ex)
            {
                _msgService.ShowMessage(
                    $"Error while deleting recipe: {ex.Message}",
                      Helpers.MessageType.Error, "Delete Data");
            }
        }


        public RecipeDM CurrentSelectedItem
        {
            get { return _currentSelectedItem; }
            set {
                if (_currentSelectedItem != value)
                {
                    _currentSelectedItem = value;
                    OnPropertyChanged();
                }
            }
        }

       private RecipeDM _currentSelectedItem;

       public ICommand ShowAddRecipeDialogCmd
        {

            get
            {
                if (_showAddRecipeDialogCmd == null)
                {
                    
                    _showAddRecipeDialogCmd = new RelayCommand(
                    execute: _ =>
                    {

                        ObservableCollection<IngredientVM> ingredientVMs = new();
                        foreach (var dm in AllIngredients)
                        {
                            ingredientVMs.Add(new IngredientVM(dm)
                            {
                                Name = dm.Name
                            });
                        }
                        var ingredient = new ColumnVM<IngredientVM>(ingredientVMs, "Ingredient");
                  


                       ObservableCollection <CategoryVM> categoryVMs = new();
                        foreach (var dm in AllCategories)
                        {
                            categoryVMs.Add(new CategoryVM(dm)
                            {
                                Name = dm.Name
                            });
                        }


                        ObservableCollection<StepVM> stepVMs = new();
                        foreach (var dm in AllSteps)
                        {
                            stepVMs.Add(new StepVM(dm)
                            {
                                Name = dm.Name
                            });
                        }

                        var categoryColumn = new ColumnVM<CategoryVM>(categoryVMs, "Category");
                        var instructionColumn = new ColumnVM<StepVM>(stepVMs, "Instruction");
                        var ingredientColumn = new ColumnVM<IngredientVM>(ingredientVMs, "Ingredient");



                        var dialog = new AddRecipeDialog
                        {
                            DataContext = new AddRecipeEditorVM(
                                new ItemChooserVM<CategoryVM>(categoryColumn),
                                new ItemChooserVM<StepVM>(instructionColumn),
                                new ItemChooserVM<IngredientVM>(ingredientColumn)),
                            Title = "Add Recipe",
                            Owner = Application.Current.MainWindow,
                            Height = 700
                        };
                        ((AddRecipeEditorVM)dialog.DataContext).AddRecipe +=
                            async (s, e) => await AddNewRecipeAsync(s, e); 
                       bool? ret =  dialog.ShowDialog(); 
                        if(ret == true)
                        {
                            // ignore
                        }
                    }
                   ,
                    canExecute: _ => {
                        return IsLogInDone;
                    }
                   );
                }
                return _showAddRecipeDialogCmd;
            }
        }
        private ICommand? _showAddRecipeDialogCmd;
        
        public UserDM? CurrentUser { get; internal set; }

        private async Task AddNewRecipeAsync(object? sender, AddRecipeEventArgs e)
        {
            if(sender is AddRecipeEditorVM vm && e != null)
            {
                var recipeDM = new RecipeDM();
                foreach(CategoryVM categoryVM in e.SelectedCategories)
                {
                    recipeDM.Categories.Add(categoryVM.Category);
                }
                foreach (IngredientVM ingredientVM in e.SelectedIngredients)
                {
                    recipeDM.Ingredients.Add(ingredientVM.IngredientDM);
                }
                foreach (StepVM stepVM in e.SelectedInstructions)
                {
                    recipeDM.Steps.Add(stepVM.Instrunction);
                }
                recipeDM.User = CurrentUser;
                string name = e.Name.Replace(" ", ""); // remove spaces to prevent many duplicate names
                recipeDM.Name = name.ToLowerInvariant();
                if (!string.IsNullOrEmpty(e.Description))
                {
                    recipeDM.Description = e.Description;
                }
                try
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        throw new Exception("Name of recipe cannot be null or empty.");
                    }
                   var recipes = await _repository.GetAllAsync();
                    var validRecipes = recipes.Where(x => x.IsExpired == false);

                    if(validRecipes != null && 
                        validRecipes.Any(x => x.Name == recipeDM.Name))
                    {
                        _msgService.ShowMessage(
                            "A recipe with this name already exists. " +
                                 "Please choose a different name.",
                                        MessageType.Warning, "Add Recipe");
                        return;
                    }
                  
                        await _repository.AddAsync(recipeDM);
                        _repository.SaveChanges();
                        Recipes.Add(recipeDM);
              
                      
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Handle concurrency conflicts
                    Console.WriteLine("Concurrency issue: " + ex.Message);
                    _msgService.ShowMessage("Failed to add the recipe. " +
                        " Please check the details and try again.",
                        MessageType.Error,"Add Recipe");
                    return;
                }
                catch (DbUpdateException ex)
                {
                    // Handle general DB update errors
                    Console.WriteLine("Update failed: " + ex.Message);

                    // Inspect inner exception for provider-specific details
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("Inner: " + ex.InnerException.Message);
                    }

                    _msgService.ShowMessage("Failed to add the recipe. " +
                       " Please check the details and try again.",
                       MessageType.Error, "Add Recipe");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Error while adding new recipe: {ex.Message}");
                    _msgService.ShowMessage("Failed to add the recipe. " +
                        " Please check the details and try again.",
                        MessageType.Error, "Add Recipe");
                    return;
                }
                _msgService.ShowMessage(
                $"A new recipe was added Successfully.",
                 Helpers.MessageType.Info, "Add Recipe");
               
            }
        }

       
    }
}
