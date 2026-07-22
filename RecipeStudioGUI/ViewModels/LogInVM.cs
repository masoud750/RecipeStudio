using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using RecipeStudio.Domain.Entities;
using RecipeStudio.UI.Commands;
using RecipeStudio.UI.Helpers;
using System.Windows.Controls;
using RecipeStudio.Repository.Presistence;
using RecipeStudio.Repository.Interfaces;

namespace RecipeStudio.UI.ViewModels
{
    public class LogInVM : ModelBase
    {

        //    public LogInVM()
        //    {
        //        // Sample design-time values
        //        Username = "DesignUser";
        //        Email = "design@example.com";
        //        Password = "••••••";

        //    }
        //    public LogInVM(IRepository<UserDM> repository, IMessageService msgService) : base(repository, msgService)
        //    {

        //        _repository = repository;

        //        _msgService = msgService;

        //    }

        //    private IRepository<UserDM>? _repository;
        //    private IMessageService? _msgService;


        //    private string? _email;

        //    public ICommand? LogInCmd
        //    {
        //        get
        //        {
        //            if (_logInCmd == null)
        //            {

        //                _logInCmd = new RelayCommand(
        //                execute: async param =>
        //                {
        //                    var pwdBox = param as PasswordBox;
        //                    if (pwdBox != null)
        //                    {
        //                        await DoLogIn(pwdBox);   // <-- call with parameter
        //                    }
        //                },
        //                  canExecuteLogIn
        //                 );
        //            }

        //            return _logInCmd;
        //        }


        //    }

        //    ICommand? _logInCmd;

        //    private bool canExecuteLogIn(object? arg)
        //    {
        //        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
        //        {
        //            return false;

        //        }
        //        return true;
        //    }





        //    private async Task DoLogIn(PasswordBox pwdBox)
        //    {
        //        bool isFailed = false;
        //        if (_repository != null)
        //        {
        //            try
        //            {
        //                await ((IUserRepository)_repository).LogInUserAsync(username: Username!, pwdBox.Password);
        //            }
        //            catch (DbUpdateException ex)
        //            {
        //                // This catches database update errors, like unique constraint or foreign key violations
        //                Console.WriteLine("Error while saving: " + ex.InnerException?.Message);

        //                isFailed = true;
        //            }
        //            catch (Exception ex)
        //            {
        //                // This catches any other unexpected errors
        //                Console.WriteLine("Unexpected error: " + ex.Message);

        //                isFailed = true;
        //            }
        //            if (isFailed)
        //            {
        //                _msgService?.ShowMessage("Failed to log in. Ensure all required fields are filled correctly.",
        //                   Helpers.MessageType.Warning, "Warning");
        //            }
        //            else
        //            {

        //                _msgService?.ShowMessage("Successfully logged in!",
        //                Helpers.MessageType.Error, "Info");
        //            }
        //        }
        //    }

        //    public bool IsLogged
        //    {
        //        get { return _isLogged; }

        //        set
        //        {
        //            if (_isLogged != value)
        //            {
        //                _isLogged = value;
        //                OnPropertyChanged(nameof(IsLogged));
        //            }
        //        }
        //    }


        //    private bool _isLogged = false;




        public LogInVM()
        {
            // Sample design-time values
            Username = "DesignUser";
         
            Password = "••••••";
          
        }
        public LogInVM(IRepository<UserDM> repository, IMessageService msgService) 
            {

                _repository = repository;
                
                _msgService = msgService;

            }

            private IRepository<UserDM>? _repository;
            private IMessageService? _msgService;

        public string? Username
        {
            get
            {
                return _username;
            }
            set
            {
                // Convert to lowercase before assignment
                var lower = value?.ToLowerInvariant();
                if (_username != lower)
                {
                    _username = lower;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested(); // forces CanExecute re-check
                }
            }
        }

        private string? _username;

        public string? Password
        {
            get => _password;
            set
            {
                // Convert to lowercase before assignment
                var lower = value?.ToLowerInvariant();
                if (_password != lower)
                {
                    _password = lower;
                    OnPropertyChanged();
                }
            }
        }

        private string? _password;



            public ICommand? LogInCmd
            {
                get
                {
                    if (_logInCmd == null)
                    {

                        _logInCmd = new RelayCommand(
                        execute: async param =>
                        {
                            var pwdBox = param as PasswordBox;
                            if (pwdBox != null)
                            {
                                await DoLogIn(pwdBox);   // <-- call with parameter
                                if (IsLogInDone)
                                {                                   
                                    pwdBox.Clear();
                                    Username = string.Empty;
                                    
                                }
                            }
                        },
                          canExecuteLogIn
                         );
                    }

                    return _logInCmd;
                }
            }

            ICommand? _logInCmd;
        private string currentUserName;

        public UserDM? CurrentUser
        {
            get => _currentUser; set => SetProperty(ref _currentUser, value);
        }

        private UserDM? _currentUser;
        
        private bool canExecuteLogIn(object? arg)
            {

            return !string.IsNullOrEmpty(Username); 
   
            }

            private async Task DoLogIn(PasswordBox pwdBox)
            {
                bool isFailed = false;
                if (_repository != null)
                {
                    try
                    {
                        CurrentUser = await ((IUserRepository)_repository).LogInUserAsync(username: Username!, pwdBox.Password);
                    if (CurrentUser == null)
                    {
                        isFailed = true;
                        
                    }
                    }
                    catch (DbUpdateException ex)
                    {
                        // This catches database update errors, like unique constraint or foreign key violations
                        Console.WriteLine("Error while saving: " + ex.InnerException?.Message);

                        isFailed = true;
                    }
                    catch (Exception ex)
                    {
                        // This catches any other unexpected errors
                        Console.WriteLine("Unexpected error: " + ex.Message);
                       
                        isFailed = true;
                    }
                    if (isFailed)
                    {
                        _msgService?.ShowMessage("Failed to log in. Ensure all required fields are filled correctly.",
                           Helpers.MessageType.Warning, "Warning");
                         AddLogText("Failed to log in.Ensure all required fields are filled correctly.");
                        return;
                    }
                    else
                    {

                        _msgService?.ShowMessage("Successfully logged in!",
                        Helpers.MessageType.Info, "Info");
                           IsLogInDone = !IsLogInDone;
                        OnFinishedLogIn();
                        
                    }
                }
            }


           public void OnFinishedLogIn()
           {
                LoggedIn?.Invoke(this, EventArgs.Empty);
           }

           public event EventHandler LoggedIn;
        }
    }
