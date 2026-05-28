using RecipeStudio.UI.ViewModels;


namespace RecipeStudio.UI.UserCtrls
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

