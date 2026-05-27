using Microsoft.EntityFrameworkCore.Query.Internal;
using RecipesDataAccess.Models;
using RecipeStudioUI.Commands;
using RecipeStudioUI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

namespace RecipeStudioUI.ViewModels
{

    public interface IValidatable
    {
        bool IsValid { get; }
        event EventHandler? Done;
    }
    public class CategoryVM : CellItemVM, IValidatable
    {
       
        public CategoryVM(CategoryDM category) {
            _category = category ??
                 throw new ArgumentNullException(nameof(category));
            UpdateName = _category.Name;
        }



     

        public CategoryDM Model{

            get {  return _category; }
            
            set { _category = value; }
        }
       
        public override string Name
        {
            get { return _category.Name; }
            set
            {
                _category.Name = value;
                OnPropertyChanged();
            }
        }


        public int CategoryId => _category.Id;

       
        public string OldName { get; set; }

        public CategoryDM Category{
             get {  return _category; }
         
        } 
         
        private CategoryDM _category;
       
        public string UpdateName {
            get { return _updateName;  }

            set
            {
                _updateName = value ?? string.Empty;
                IsValid = !string.IsNullOrWhiteSpace(_updateName);
                OnPropertyChanged(nameof(UpdateName));
            }
        }

        string _updateName;
        // stores the name for TableVM update handling


      

        
       

        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                SetProperty(ref _isValid, value);
            }
        }

        private bool _isValid;
        public bool IsExpired
        {

            get { return _category.IsExpired; }
        }
       

        public void OnFinishedEditing()
        {
            Done?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler? Done;



        public ICommand SaveCmd
        {
            get
            {
                if (_commitCmd == null)
                {

                    _commitCmd = new RelayCommand(
                    execute: _ =>
                    {

                        OnFinishedEditing();
                    }
                   ,
                    canExecute: _ =>
                    {
                        return !string.IsNullOrEmpty(_updateName);
                    }
                   );
                }
                return _commitCmd;
            }

        }

        private ICommand? _commitCmd;

    }
}
