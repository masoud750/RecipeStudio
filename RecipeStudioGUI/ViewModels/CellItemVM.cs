using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeStudioUI.ViewModels
{
    public class CellItemVM: ModelBase
    {
        public CellItemVM() { }

        public bool IsSelected { get { 
                return _isSelected; }

                set{
                    SetProperty(ref _isSelected, value);
                }
            }


        private bool _isSelected;

       

      


        public virtual string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private  string _name;

      

    }
}
