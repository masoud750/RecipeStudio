using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using RecipeStudio.Domain.Entities;
using RecipeStudio.UI.Helpers;
using RecipeStudio.UI.Commands;
using RecipeStudio.Repository.Interfaces;


namespace RecipeStudio.UI.ViewModels
{


    internal class DesignerRecipeQueryTableVM : RecipeQueryTableVM
    {
        public DesignerRecipeQueryTableVM() :
            base(new FakeRecipeRepository(),
                  new DesignMessageServiceFake())

        {
            var recipe = new RecipeDM
            {
                Id = 1,
                Name = "Omlet",
                User = new UserDM
                {
                    Id = 1,
                    UserName = "John",
                    Email = "John@yahoo.com",
                    FavoriteRecipes = []
                },
            };
            Recipes.Add(recipe);


        }

    }



    public class RecipeQueryTableVM : ModelBase
    {
        public RecipeQueryTableVM(IRepository<RecipeDM> repository,
             IMessageService msgService)
        {

            _repository = repository??
                throw new ArgumentNullException(nameof(repository));
            _msgService = msgService ??
                                   throw new ArgumentNullException(nameof(msgService));
        }


        private IRepository<RecipeDM> _repository;
        public UserDM? CurrentUser
        {
            get; set;
        }
        public ObservableCollection<string> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value ??
                    throw new ArgumentNullException(nameof(_categories));
            }
        }
        private ObservableCollection<string> _categories = new();


        public ObservableCollection<string> Ingredients
        {
            get { return _ingredients; }
            set
            {
                _ingredients = value ??
                    throw new ArgumentNullException(nameof(_ingredients));
            }
        }
        private ObservableCollection<string> _ingredients = new();




        public ObservableCollection<string> Users
        {
            get { return _users; }
            set
            {
                _users = value ??
                    throw new ArgumentNullException(nameof(_users));
            }
        }
        private ObservableCollection<string> _users = new();


        public ObservableCollection<RecipeDM> Recipes
        {
            get { return _recipeDMs; }
            set { _recipeDMs = value; }
        }
        private ObservableCollection<RecipeDM> _recipeDMs = new();

        private IMessageService _msgService;


        public RecipeDM? CurrentSelectedItem
        {
            get => _currentSelectedItem;
            set
            {
                if (_currentSelectedItem != value)
                {
                    _currentSelectedItem = value;
                    OnPropertyChanged();
                }
            }
        }
        private RecipeDM? _currentSelectedItem;


        public int CurrentSelectedUserIndex
        {
            get; set;
        } = 0;


        public int CurrentSelectedCategoryIndex
        {
            get; set;
        } = 0;


        public int CurrentSelectedIngredientIndex
        {
            get;
            set;
        } = 0;


        public ICommand UserQueryCmd
        {
            get
            {
                if (_userQueryCmd == null)
                {

                    _userQueryCmd = new RelayCommand(
                    execute: async _ =>
                    {
                        try
                        {
                            string user = Users[CurrentSelectedUserIndex];
                            var recipes = await _repository.GetAllAsync();
                            var subRecipes = recipes.Where(x => x.User.UserName == user &&
                                       x.IsExpired == false);
                            Recipes.Clear();
                            foreach (var recipe in subRecipes)
                            {
                                Recipes.Add(recipe);
                            }
                        }
                        catch (DbUpdateException ex)
                        {
                            Console.WriteLine("Unexpected error: " + ex.Message);
                            // notify user
                            MessageBox.Show("The query could not be executed.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Unexpected error: " + ex.Message);
                            MessageBox.Show("The query could not be executed.");
                        }
                       
                    }
                   ,
                    canExecute: _ =>
                    {
                        return CurrentSelectedUserIndex > 0;
                    }
                   );
                }
                return _userQueryCmd;
            }

        }

        private ICommand? _userQueryCmd;



        public ICommand CategoryQueryCmd
        {
            get
            {
                if (_categoryQueryCmd == null)
                {

                    _categoryQueryCmd = new RelayCommand(
                    execute: async _ =>
                    {
                        string category = Categories[CurrentSelectedCategoryIndex];
                        try
                        {
                            var recipes = await _repository.GetAllAsync();
                            var subRecipes = recipes.Where(
                           x => x.Categories.Any(x => x.Name == category) &&
                                  x.IsExpired == false);
                            Recipes.Clear();
                            foreach (var recipe in subRecipes)
                            {
                                Recipes.Add(recipe);
                            }
                        }
                        catch (DbUpdateException ex)
                        {
                            Console.WriteLine("Unexpected error: " + ex.Message);
                            // notify user
                            MessageBox.Show("The query could not be executed.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Unexpected error: " + ex.Message);
                            MessageBox.Show("The query could not be executed.");
                        }


                    }
                   ,
                    canExecute: _ =>
                    {                       
                        return CurrentSelectedCategoryIndex > 0;
                    }
                   );
                }
                return _categoryQueryCmd;
            }

        }

        private ICommand? _categoryQueryCmd;


        public ICommand IngredientQueryCmd
        {
            get
            {
                if (_ingredientQueryCmd == null)
                {

                    _ingredientQueryCmd = new RelayCommand(
                    execute: async _ =>
                    {
                        string ingredient = Ingredients[CurrentSelectedIngredientIndex];
                        try
                        {
                            var recipes = await _repository.GetAllAsync();
                            var subRecipes = recipes.Where(
                           x => x.Ingredients.Any(x => x.Name == ingredient) &&
                                  x.IsExpired == false);
                            Recipes.Clear();
                            foreach (var recipe in subRecipes)
                            {
                                Recipes.Add(recipe);
                            }
                        }
                        catch (DbUpdateException ex)
                        {
                            Console.WriteLine("Unexpected error: " + ex.Message);
                            // notify user
                            MessageBox.Show("The query could not be executed.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Unexpected error: " + ex.Message);
                            MessageBox.Show("The query could not be executed.");
                        }


                    }
                   ,
                    canExecute: _ =>
                    {
                       return CurrentSelectedIngredientIndex > 0;
                    }
                   );
                }
                return _ingredientQueryCmd;
            }

        }

        private ICommand? _ingredientQueryCmd;



        public ICommand AddToFavoriteCmd
        {
            get
            {
                if (_addToFavoriteCmd == null)
                {

                    _addToFavoriteCmd = new RelayCommand(
                    execute: async _ =>
                    {
                        if (CurrentSelectedItem?.User.UserName != CurrentUser?.UserName)
                        {
                            if (CurrentSelectedItem is RecipeDM)
                            {
                                try
                                {
                                    await _repository.AddFavoriteAsync(CurrentUser!.Id,
                                        CurrentSelectedItem.Id);
                                }
                                catch (DbUpdateException ex)
                                {
                                    Console.WriteLine("Unexpected error: " + ex.Message);
                                    // notify user
                                    MessageBox.Show("Failed to add item to favorites.");
                                }
                                catch(Exception ex)
                                {
                                    Console.WriteLine("Unexpected error: " + ex.Message);
                                    MessageBox.Show("Failed to add item to favorites.");
                                }

                            }
                        }
                    }
                   ,
                    canExecute: _ =>
                    {
                        return EvaluateCanExecuteAddToFavorite();
                    }
                   );
                }
                return _addToFavoriteCmd;
            }

        }
        private ICommand? _addToFavoriteCmd;

        private bool EvaluateCanExecuteAddToFavorite()
        {
            return (CurrentUser != null) && !string.IsNullOrEmpty(CurrentUser.UserName) &&
                CurrentSelectedItem != null &&
                    CurrentSelectedItem.User.UserName != CurrentUser.UserName &&
                        !CurrentUser.FavoriteRecipes.Any(x => x.Id == CurrentSelectedItem.Id);
        }




        public ICommand ShowMyFavoriteCmd
        {
            get
            {
                if (_showMyFavoriteCmd == null)
                {

                    _showMyFavoriteCmd = new RelayCommand(
                    execute: async _ =>
                    {
                        Recipes.Clear();
                        foreach (var favorite in CurrentUser!.FavoriteRecipes)
                        {
                            Recipes.Add(favorite);
                        }
                    }
                   ,
                    canExecute: _ =>
                    {
                        return CurrentUser != null && CurrentUser!.FavoriteRecipes.Count > 0;
                    }
                   );
                }
                return _showMyFavoriteCmd;
            }

        }
        private ICommand? _showMyFavoriteCmd;

    }
}
