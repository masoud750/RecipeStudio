using RecipeStudio.Domain.Entities;

namespace RecipeStudio.UI.ViewModels
{
    public class StepVM : CellItemVM
    {
        public StepVM(StepDM dm) 
        { 
            _dm = dm ?? throw new ArgumentNullException(nameof(dm));
        }

        public StepDM Instrunction
        {
            get { return _dm; }
        }

        private StepDM _dm;
    }
}