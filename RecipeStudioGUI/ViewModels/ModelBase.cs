using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace RecipeStudioUI.ViewModels
{
    public class ModelBase : INotifyPropertyChanged
    {

        public ModelBase() {  }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       

        public bool IsViewNotSelected
        {
            get { return _isViewNotSelected; }
            set {
                if (_isViewNotSelected != value)
                {
                    _isViewNotSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isViewNotSelected = true;

      
        public string LogText => _log.ToString();

        private readonly StringBuilder _log = new();

        public void AddLogText(string txt)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                _log.AppendLine(txt);
                OnPropertyChanged(nameof(LogText));
            },DispatcherPriority.Normal);
           
               
            
        }

        public bool IsLogInDone
        {
            get { return _isLogInDone; }

            set
            {
                
                    _isLogInDone = value;
                    OnPropertyChanged(nameof(IsLogInDone));                  
                
            }
        }


        private static bool _isLogInDone;



        public string Session { get => _session; set => SetProperty(ref _session, value); }

        private string _session = "No Session";


        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }


        public DateTime ResetValidToTimeStamp()
        {
           return new DateTime(2090, 1, 1, 0, 0, 0);
        }


    }
}

namespace RecipeStudioUI.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object? parameter) => _execute(parameter);

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}


