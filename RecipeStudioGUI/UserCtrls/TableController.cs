using Domain.Entities;
using RecipeStudioUI.Helpers;
using RecipeStudioUI.Repositories;
using RecipeStudioUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeStudioUI.UserCtrls
{
    public class TableController : ModelBase
    {
        private ModelBase? _currentVM;
        public ModelBase? CurrentVM
        {
            get => _currentVM;
            set { _currentVM = value; OnPropertyChanged(); }
        }
    }
}

