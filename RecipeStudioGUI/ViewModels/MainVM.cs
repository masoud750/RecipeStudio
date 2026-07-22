using Microsoft.Extensions.DependencyInjection;
using RecipeStudio.UI.Commands;
using RecipeStudio.UI.Helpers;
using RecipeStudio.Repository.Interfaces;

using RecipeStudio.UI.Services;
using RecipeStudio.UI.UserCtrls;
using RecipeStudio.UI.Views;
using System.Windows;
using System.Windows.Input;
using RecipeStudio.Domain.Entities;


namespace RecipeStudio.UI.ViewModels
{


    public interface IVMFactory
    {
        T Create<T>() where T : class;
    }

    public class VMFactory : IVMFactory
    {
     

        public VMFactory(IServiceProvider provider)
        {
            _provider = provider;
        }
        private readonly IServiceProvider _provider;

        public T Create<T>() where T : class
            => _provider.GetRequiredService<T>();
    }
    class UserRepositoryFake : IRepository<UserDM>
    {
        public void Add(UserDM entity)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(UserDM entity)
        {
            throw new NotImplementedException();
        }

        public Task AddFavoriteAsync(int userId, int recipeId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDM>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserDM?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public UserDM? GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<UserDM?> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UserDM entity)
        {
            throw new NotImplementedException();
        }
    }

    // Design-time MainVM
     public class DesignMainVM : MainVM
    {
        public DesignMainVM()
            : base(new RegisterVMDesigner(new UserRepositoryFake(), new
                  DesignMessageServiceFake()),                
                new LogInVM(),
                new TableCollectionVM(
                    new DesignCategoryVM(),
                    new DesignRecipeVM(),
                    new DesignStepVM(),
                    new DesignIngredientVM(),
                    new DesignerRecipeQueryTableVM(),
                    new DesignMessageServiceFake()
                   ),
                 new DesignMessageServiceFake()
            )
        {
            // Fill with sample data for design-time
            ColumnsVM.Category.Categories.Add(new CategoryVM(new CategoryDM { Name = "Breakfast" }));
            ColumnsVM.Category.Categories.Add(new CategoryVM(new CategoryDM { Name = "Lunch" }));

            ColumnsVM.Recipe.Recipes.Add(new RecipeDM { Name = "Pancakes" });
            ColumnsVM.Recipe.Recipes.Add(new RecipeDM { Name = "Grilled Cheese" });

            ColumnsVM.Step.Steps.Add(new StepDM { Name = "Mix ingredients" });
            ColumnsVM.Step.Steps.Add(new StepDM { Name = "Cook on medium heat" });
            Caption = "Designer";

        }
    }



    public class MainVM : ModelBase
    {
        [Obsolete("Design‑time constructor only", error: false)]
        internal MainVM(
                RegisterVM regVM,
                LogInVM logInVM,
                TableCollectionVM columnsVM,
                IMessageService msgService)
        {
            ColumnsVM = columnsVM ?? throw new ArgumentNullException(nameof(columnsVM));
            RegisterManager = regVM ?? throw new ArgumentNullException(nameof(regVM));
            LogInManager = logInVM ?? throw new ArgumentNullException(nameof(logInVM));
            LogInManager.LoggedIn += UpdateSessionState;
            _msgService = msgService ?? throw new ArgumentNullException(nameof(msgService));

            TableController = new TableController();
            ColumnsVM.Category.Categories.Add(
                new CategoryVM(new CategoryDM { Id = 1, Name = "TestCategory" }));
            ColumnsVM.Category.Categories.Add(
                new CategoryVM(new CategoryDM { Id = 2, Name = "AnotherCategory" }));

            TableController.CurrentVM = ColumnsVM.Ingredient; // Important!
            TableController.CurrentVM.IsViewNotSelected = false;
            Caption = "Ingredients";
            AddLogText("This feature is not available");
        }
        public MainVM(IDataService data,
            IMessageService msgService,IServiceProvider provider)
        {

            ColumnsVM = provider.GetRequiredService<TableCollectionVM>();
            LogInManager = provider.GetRequiredService<LogInVM>();
            LogInManager.LoggedIn += UpdateSessionState;
            _msgService = msgService ?? throw new ArgumentNullException(nameof(msgService));

            TableController = provider.GetRequiredService<TableController>();
            RegisterManager = provider.GetRequiredService<RegisterVM>();

            TableController.CurrentVM = ColumnsVM.Ingredient; // WICHTIG!
            TableController.CurrentVM.IsViewNotSelected = false;
            Caption = "Ingredients";
            AddLogText("This feature is not available");
            _data = data ?? throw new ArgumentNullException(nameof(data));
            
        }


        private readonly IDataService _data;
        public RegisterVM RegisterManager { get; set; }
        public LogInVM LogInManager { get; set; }
        public TableCollectionVM ColumnsVM { get; set; }
        public TableController TableController { get; }


        private readonly IMessageService _msgService;

        private void UpdateSessionState(object? sender, EventArgs e)
        {
            if (sender is LogInVM logInVM)
            {
                Session = "Session Active";
                ColumnsVM.Query.CurrentUser = LogInManager.CurrentUser;
            }
        }

        public ICommand RegisterDialogCmd
        {
            get
            {
                if (_registerDialogCmd == null)
                {

                    _registerDialogCmd = new RelayCommand(
                    execute: _ =>
                    {



                        var dialog = new RegisterDialog
                        {
                            Owner = Application.Current.MainWindow,
                            DataContext = RegisterManager
                        };

                        
                        if (dialog.DataContext is RegisterVM vm)
                        {
                            vm.RegPro += (s, e) =>
                            {
                                dialog.DialogResult = true;

                                dialog.Close();
                            };
                        }

                        bool? result = dialog.ShowDialog();


                    }
                   ,
                    canExecute: _ => { return !IsLogInDone; }
                   );
                }
                return _registerDialogCmd;
            }

        }

        private ICommand? _registerDialogCmd;



        public ICommand LogInCmd
        {
            get
            {
                if (_logInCmd == null)
                {

                    _logInCmd = new RelayCommand(
                    execute: _ =>
                    {

                        var dialog = new LogInDialog();
                        dialog.Owner = Application.Current.MainWindow;
                        dialog.DataContext = LogInManager;
                        bool? result = dialog.ShowDialog();
                        if (result == true)
                        {
                            // login succeeded
                            //Ignore
                        }
                    }
                   ,
                    canExecute: _ => { return !IsLogInDone; }
                   );
                }
                return _logInCmd;
            }

        }

        private ICommand? _logInCmd;

        public string CurrentUser
        {
            get; set;
        } = string.Empty;


        public ICommand LogOutCmd
        {
            get
            {
                if (_logOutCmd == null)
                {

                    _logOutCmd = new RelayCommand(
                    execute: _ =>
                    {

                        IsLogInDone = false;
                        Session = "No Session *";
                        ColumnsVM.Recipe.Recipes.Clear();

                    }
                   ,
                    canExecute: _ => { return IsLogInDone; }
                   );
                }
                return _logOutCmd;
            }

        }

        private ICommand? _logOutCmd;

        //Navigation

        public ICommand GoToCategoriesCmd
        {
            get
            {
                if (_goToCategoriesCmd == null)
                {

                    _goToCategoriesCmd = new RelayCommand(
                    execute: _ =>
                    {

                        UpdateSelectionStateToNotSelected();
                        TableController.CurrentVM = ColumnsVM.Category;
                        TableController.CurrentVM.IsViewNotSelected = false;
                        Caption = "Categories";
                    },
                    _ =>
                    {
                        return true;
                        //if (TableController.CurrentVM != ColumnsVM.Category) 
                        //    { return true; } 
                        //   return false;
                    }
                   );
                }
                return _goToCategoriesCmd;
            }

        }

        private ICommand? _goToCategoriesCmd;

        public void UpdateSelectionStateToNotSelected()
        {
            if (TableController.CurrentVM != null)
            {
                TableController.CurrentVM.IsViewNotSelected = true;

            }
        }


        public ICommand GoToIngredientsCmd
        {
            get
            {
                if (_goToIngredientsCmd == null)
                {

                    _goToIngredientsCmd = new RelayCommand(
                    execute: _ =>
                    {
                        UpdateSelectionStateToNotSelected();
                        TableController.CurrentVM = ColumnsVM.Ingredient;
                        TableController.CurrentVM.IsViewNotSelected = false;
                        Caption = "Ingredients";
                    },
                    _ =>
                    {
                        if (TableController.CurrentVM != ColumnsVM.Ingredient)
                        {
                            return true;
                        }
                        return false;
                    }
                   );
                }
                return _goToIngredientsCmd;
            }

        }

        private ICommand? _goToIngredientsCmd;

        public ICommand GoToStepsCmd
        {
            get
            {
                _goToSetpsCmd ??= new RelayCommand(
                    execute: _ =>
                    {
                        UpdateSelectionStateToNotSelected();
                        TableController.CurrentVM = ColumnsVM.Step;
                        TableController.CurrentVM.IsViewNotSelected = false;
                        Caption = "Instructions";

                    },
                    _ =>
                    {
                        if (TableController.CurrentVM != ColumnsVM.Step)
                        {
                            return true;
                        }
                        return false;
                    }
                   );
                return _goToSetpsCmd;
            }
        }

        private ICommand? _goToSetpsCmd;





        public ICommand GoToRecipesCmd
        {
            get
            {
                _goToRecipesCmd ??= new RelayCommand(
                    execute: async _ =>
                    {
                        UpdateSelectionStateToNotSelected();
                        ColumnsVM.Recipe.CurrentUser = LogInManager.CurrentUser;
                        await ColumnsVM.Recipe.InitializeCurrentUserRecipesAsync();
                        TableController.CurrentVM = ColumnsVM.Recipe;
                        TableController.CurrentVM.IsViewNotSelected = false;
                        Caption = "Recipes";
                    },
                    _ =>
                    {
                        if (TableController.CurrentVM != ColumnsVM.Recipe && IsLogInDone)
                        {
                            return true;
                        }
                        return false;

                    }
                   );
                return _goToRecipesCmd;
            }
        }

        private ICommand? _goToRecipesCmd;


        public string? Caption
        {
            get => _caption;
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }

        private string? _caption;


        public ICommand GoToQueriesCmd
        {
            get
            {
                _goToQueriesCmd ??= new RelayCommand(
                    execute: async _ =>
                    {
                        UpdateSelectionStateToNotSelected();
                        ColumnsVM.Recipe.CurrentUser = LogInManager.CurrentUser;
                        ColumnsVM.Query.CurrentUser = LogInManager.CurrentUser;
                        await ColumnsVM.Recipe.InitializeAsync();

                        ColumnsVM.PopulateUsers();
                        TableController.CurrentVM = ColumnsVM.Query;
                        TableController.CurrentVM.IsViewNotSelected = false;
                        Caption = "Queries";
                    },
                    _ =>
                    {
                        if (TableController.CurrentVM != ColumnsVM.Query)
                        {
                            return true;
                        }

                        return false;

                    }
                   );
                return _goToQueriesCmd;
            }
        }

        private ICommand? _goToQueriesCmd;

    }
}




