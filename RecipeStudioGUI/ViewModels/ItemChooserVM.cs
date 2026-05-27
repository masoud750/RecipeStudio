using Domain.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RecipeStudioUI.ViewModels
{
     public class DesignerItemChooser : ItemChooserVM<IngredientVM> 
    {
        public DesignerItemChooser(): base(
            new ColumnVM<IngredientVM>(
                new ObservableCollection<IngredientVM>(),"Ingredient"))
        {
            
            AvailableItems.Add(new IngredientVM(new IngredientDM
            {
                Id = 1,
                Name = "Egg",
                Quantity = "2",

            })
            );
            AvailableItems.Add(new IngredientVM(new IngredientDM
            {
                Id = 2,
                Name = "Milk" ,
                Quantity = "2",

            })
            );

            AvailableItems.Add(new IngredientVM(new IngredientDM
            {
                Id = 3,
                Name = "Sugare",
                Quantity = "2",

            })
            );
        }
    }

    public class ItemChooserVM<T> : ModelBase where T : CellItemVM
    {
        public ItemChooserVM(ColumnVM<T> column)
        {
            ArgumentNullException.ThrowIfNull(column);
            AvailableItems =  column.Items;

            foreach (T avilable in AvailableItems)
            {
                avilable.PropertyChanged += HandlePropertyChnaged;

            }

            _column = column;
           
        }

        private void HandlePropertyChnaged(object? sender, PropertyChangedEventArgs e)
        {
             if(sender is CellItemVM vm)
            {
                if (vm.IsSelected)
                {
                    if(SelectedItems.Any( x => x.Name == vm.Name) == false)
                    {
                        SelectedItems.Add((T)vm);
                    }
                }
                else
                {
                    if (SelectedItems.Any(x => x.Name == vm.Name) == true)
                    {
                        SelectedItems.Remove((T)vm);
                    }
                }
            }
        }

        public ObservableCollection<T> AvailableItems {
            get;set;
        } = new();


        public ColumnVM<T> Column
        {
            get { return _column; }
        }

        ColumnVM<T> _column;
      

        public string ColumnHeader
        {
            get { return _column.Name; }
          
        
        }


        public ObservableCollection<T> SelectedItems { get; set; } = new();

    }


    public class IngredientVM : CellItemVM
    {
        public IngredientVM(IngredientDM dm)
        {
            ArgumentNullException.ThrowIfNull(dm);  
            IngredientDM = dm;
            Name = dm.Name;
     
        }

        public IngredientDM IngredientDM { get;  }

      

    

    }
}
