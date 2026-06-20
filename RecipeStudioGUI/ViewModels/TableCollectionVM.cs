using RecipeStudio.Domain.Entities;
using RecipeStudio.UI.ViewModels;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
namespace RecipeStudio.UI.ViewModels
{
    public class TableCollectionVM : ModelBase  
    {

        // Constructor injection
        public TableCollectionVM(CategoryTableVM categoryVM, 
                                    RecipeTableVM recipeVM, StepTableVM stepVM,
                                        IngredientTableVM ingredientVM,
                                    RecipeQueryTableVM queryVM,
                                        Helpers.IMessageService msgService)
        {
            ArgumentNullException.ThrowIfNull(categoryVM, nameof(categoryVM));
            ArgumentNullException.ThrowIfNull(recipeVM, nameof(recipeVM));
            ArgumentNullException.ThrowIfNull(stepVM, nameof(stepVM));
            ArgumentNullException.ThrowIfNull(ingredientVM, nameof(ingredientVM));
            ArgumentNullException.ThrowIfNull(queryVM, nameof(queryVM));
            ArgumentNullException.ThrowIfNull(msgService, nameof(msgService));
            Category = categoryVM;
            Recipe = recipeVM;
            Step = stepVM;
            _msgService = msgService;
            Ingredient = ingredientVM;
            Query = queryVM;
            Recipe.AllCategories = Category.CategoryModels;
            Recipe.AllIngredients = Ingredient.Ingredients;
            Recipe.AllSteps = Step.Steps;           
        }

       
        public CategoryTableVM Category { get; } 
        public RecipeTableVM Recipe { get; }
        public StepTableVM Step { get; }

        public RecipeQueryTableVM Query { get;}

        public IngredientTableVM Ingredient { get; }
        private Helpers.IMessageService _msgService;

        //public async Task AddCategoryAsync(CategoryDM category) => await CategoryVM.AddAsync(category);
        public async Task AddRecipeAsync(RecipeDM recipe) => 
            await Recipe.AddAsync(recipe);
        public async Task AddStepAsync(StepDM step) => 
            await Step.AddAsync(step);
        public async Task AddStepAsync(IngredientDM ingredient) => 
            await Ingredient.AddAsync(ingredient);

        public async Task InitializeAsync()
        {
            try
            {
                await Category.InitializeAsync();

                //await Recipe.InitializeAsync();
                Recipe.AllCategories = Category.CategoryModels;
                Category.Categories.CollectionChanged += UpdateCategoryFilter;
                Category.EditCategory += UpdateCategoryNamesInFilter;
                await Step.InitializeAsync();
                await Ingredient.InitializeAsync();            
                PopulateCategoryFilter();
                PopulateIngredientFilter();
                

            }
            catch (Exception ex)
            {
                _msgService.ShowMessage($"Initialization failed: {ex.Message}", 
                    Helpers.MessageType.Warning, "Warning");
            }
        }

        


        private void PopulateCategoryFilter()
        {
            Query.Categories.Clear();
            Query.Categories.Add("Select an item...");
            foreach (var item in Category.Categories)
            {
                Query.Categories.Add(item.Name);
            }
        }

        private void PopulateIngredientFilter()
        {
            Query.Ingredients.Clear();
            Query.Ingredients.Add("Select an item...");
            foreach (var item in Ingredient.Ingredients)
            {
                Query.Ingredients.Add(item.Name);
            }
        }

        public void PopulateUsers()
        {
            Query.Users.Clear();
            Query.Users.Add("Select an item...");
            foreach (var item in Recipe.Recipes)
            {

                if(!Query.Users.Any( x => x == item.User.UserName))
                    Query.Users.Add(item.User.UserName);
            }
        }

        private void UpdateCategoryNamesInFilter(object? sender, EventArgs e)
        {
           if(sender is CategoryTableVM vm)
            {
                PopulateCategoryFilter();

            }
        }

        private void UpdateCategoryFilter(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is ObservableCollection<CategoryVM>)
            {
                if( e.Action == NotifyCollectionChangedAction.Add)//CollectionChangeAction.Add)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        List<CategoryVM> items = e.NewItems.Cast<CategoryVM>().ToList();
                        if (items.Count > 0)
                        {
                            Query.Categories.Add(item: items[0].Name);
                        }

                    }));
                   
                }

                if (e.Action == NotifyCollectionChangedAction.Remove)//CollectionChangeAction.Add)
                {
                    PopulateCategoryFilter();

                }
            }
        }

        public string ColumnHeader { get; set; } = string.Empty;

 

        public string? ItemToAdd
        {
            get
            {
                return _itemToAdd;
            }
            set
            {
                if(!string.IsNullOrEmpty(value) && _itemToAdd != value)
                {
                    _itemToAdd = value;
                }
               
            }
        }

        public string? _itemToAdd;

        // Async Commands
        //public ICommand AddCommand => new RelayCommand(async _ =>
        //{
        //    try
        //    {


        //        if (string.IsNullOrWhiteSpace(_itemToAdd))
        //            return;

        //        // ✅ now valid

        //        await _repository!.AddAsync(newItem).ConfigureAwait(false);
        //        await _repository!.SaveChangesAsync().ConfigureAwait(false);
        //        Items.Add(newItem);
        //        _itemToAdd = string.Empty;
        //        OnPropertyChanged(nameof(ItemToAdd));



        //        // Nur wenn erfolgreich, UI aktualisieren
        //        Items.Add(newItem);
        //        _messageService?.ShowMessage("New entry created successfully.", Helpers.MessageType.Info);

        //    }
        //    catch (Exception ex)
        //    {
        //        _messageService?.ShowMessage($"$\"Error adding item: {ex.Message}", Helpers.MessageType.Error);

        //    }
        //});

        //public ICommand UpdateCommand => new RelayCommand(async _ =>
        //{
        //    if (SelectedItem != null)
        //    {
        //        await _repository!.UpdateAsync(SelectedItem);
        //        // UI updates automatically if SelectedItem properties implement INotifyPropertyChanged
        //    }
        //});

        //public ICommand DeleteCommand => new RelayCommand(async _ =>
        //{
        //    if (SelectedItem != null)
        //    {
        //        //await _repository!.DeleteAsync(SelectedItem);
        //        //Items.Remove(SelectedItem);
        //    }
        //});
    }


}
