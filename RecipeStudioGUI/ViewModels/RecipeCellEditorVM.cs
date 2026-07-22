using System.Collections.ObjectModel;


namespace RecipeStudio.UI.ViewModels
{
    internal class RecipeCellEditorVM<T, V>
    where T: CellItemVM
    where V : class
    {

        public RecipeCellEditorVM(string columnHeader, 
            ObservableCollection<T> availableItems,
         ObservableCollection<T> selectedItems
        )
        {
            if (string.IsNullOrEmpty(columnHeader)) 
            {
                columnHeader = "Item"; 
            }
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(availableItems));
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(selectedItems));
            ColumnHeader = columnHeader;
            SynchronizeSelections();
        }

        public ObservableCollection<T> SelectedCategories { get; set; }
        public ObservableCollection<T> AvailableCategories { get; set; }
        public string ColumnHeader { get; }

        private void SynchronizeSelections()
        {

            //foreach (var avilable in AvailableCategories)
            //{
            //    avilable.PropertyChanged += HandlePropertyChnaged;

            //}

            // Ensure all selected VMs are marked
            foreach (T selected in SelectedCategories)
            {
                selected.IsSelected = true;
            }



            foreach (T available in AvailableCategories)
            {
                if (SelectedCategories.Any(x => x.Name == available.Name))
                {
                    available.IsSelected = true;
                }
            }
        }


        

     

    }
}
