using Microsoft.EntityFrameworkCore.Query;
using RecipesDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeStudioUI.ViewModels
{
    public class ColumnVM<T> : ModelBase where T : CellItemVM
    {
        public ColumnVM(ObservableCollection<T> items, string name)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            ArgumentNullException.ThrowIfNull(name);
            if (name != string.Empty)
            {
                Name = name;
            }
        }

        public ObservableCollection<T> Items { get; private set; }
        public string Name { get { return _name; }

            set { 
                SetProperty(ref _name, value);

            }
        }



        private string _name;
    }

    internal class IgredientVM : CellItemVM
    {
        public IgredientVM(IngredientDM dm)
        {
            _dm = dm ?? throw new ArgumentNullException(nameof(dm));
        }

        private IngredientDM _dm;

        public IngredientDM DM { get { return _dm; } }
     
    }
}
