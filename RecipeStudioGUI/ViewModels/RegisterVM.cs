using Microsoft.EntityFrameworkCore;
using RecipeStudio.UI.Commands;
using RecipeStudio.UI.Helpers;
using System.Net.Mail;
using System.Windows.Input;
using RecipeStudio.Domain.Entities;
using RecipeStudio.Repository.Interfaces;
using RecipeStudio.Repository.Presistence;

namespace RecipeStudio.UI.ViewModels
{
    
    public class RegisterVMDesigner : RegisterVM
    {

        public RegisterVMDesigner(IRepository<UserDM> repository, IMessageService msgServices):
            base(repository,msgServices)
        {
            // Sample design-time values
            Username = "DesignUser";
            Email = "design@example.com";
            Password = "••••••";
            LogInfo = "Only registered users are permitted to modify the table.";  
            
        }

    }
    public class RegisterVM : ModelBase
    {
        public RegisterVM(IRepository<UserDM> repository, IMessageService msgService)
        {
            ArgumentNullException.ThrowIfNull(
                repository,nameof(repository));
            ArgumentNullException.ThrowIfNull(msgService,
                nameof(msgService));
            _repository = repository;
            _messageService = msgService;
        }
        private readonly IMessageService? _messageService;
        protected IRepository<UserDM>? Repository { 
            get { return _repository; } }

        private IRepository<UserDM>? _repository;

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

        public string Password
        {
            get => _password;
            set
            {
                if (_password == value)
                    return;

                _password = value;
                OnPropertyChanged();
            }
        }

        private string _password = string.Empty;

        public string? Email
        {
            get => _email;
            set
            {
                var lower = value?.ToLowerInvariant();
                _email = lower; // always assign
                OnPropertyChanged();
            }
        }

        private string? _email;


        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                return new MailAddress(email).Address == email;
            }
            catch
            {
                return false;
            }
        }


        public string LogInfo
        {
            get => _logInfo;
            set
            {
                if (_logInfo != value)
                {
                    _logInfo = value;
                    OnPropertyChanged();
                }
            }
        }

        string _logInfo = "Only registered users are permitted to modify the table.";


        public ICommand RegisterCmd
        {
            get
            {
                if (_registerCmd == null)
                {

                    _registerCmd = new RelayCommand(
                    execute: async param =>
                    {
                        
                        if ( Password != string.Empty)
                        {
                            await DoRegister();   // <-- call with parameter
                        }
                    },
                    canExecuteRegister
                   );
                }
                return _registerCmd;
            }

        }

        private ICommand? _registerCmd;

        private bool canExecuteRegister(object? arg)
        {
           
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Email)||
                 string.IsNullOrEmpty(Password))
            {
                return false;

            }
            return true;
        }

        private async Task DoRegister()
        {
            bool isFailed = false;
            if (_repository != null)
            {

                try
                {
                    if (IsValidEmail(Email!))
                    {
                        await ((IUserRepository)_repository).AddAsync(Username!, Email!, password: Password);
                    }
                    else
                    {
                        _messageService?.ShowMessage("Email is not valid.", Helpers.MessageType.Warning, "Warning");

                        return;
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
                    isFailed = true;
                    // This catches any other unexpected errors
                    Console.WriteLine("Unexpected error: " + ex.Message);

                }

                if (isFailed)
                {

                    _messageService?.ShowMessage("Failed to register. Ensure all required fields are filled correctly.",
                        Helpers.MessageType.Warning, "Warning");

                    return;

                }
                {
                    _messageService?.ShowMessage("Successfully registered. You can now log in.",
                        Helpers.MessageType.Info, "Info");
                    Email = string.Empty;
                    Username = string.Empty;
                    Password = string.Empty;
                    FinishedRegister();

                }

            }

        }

        public void FinishedRegister()
        {
            RegPro?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler RegPro;
    }
}
