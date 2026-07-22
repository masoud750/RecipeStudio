using RecipeStudio.Domain.Entities;
using RecipeStudio.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeStudio.UI.Tests.ViewModels
{
    [TestFixture]
    internal class ItemChooserTests
    {
        

        [Test]
        public void Constructor_WhenColumnIsNull_ThrowsArgNullEx()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new ItemChooserVM<CellItemVM>(null!);
            });
            Assert.That(ex.ParamName,Is.EqualTo("column"));
        }

        [Test]
        public void SelectedItems_WhenCellItemPropertyChanged()
        {
            var col = new ColumnVM<IngredientVM>(
                                        new ObservableCollection<IngredientVM>(),
                                                                  "Ingredient");
            col.Items.Add(new IngredientVM(new IngredientDM
            {
                Id = 1,
                Name = "Egg",
                Quantity = "2",

            }));
            var itemChooser =
                new ItemChooserVM<IngredientVM>(col);
                                   
            itemChooser.SelectedItems.Clear();
                        
            Assert.That(itemChooser.AvailableItems.Count, Is.EqualTo(1));
            Assert.That(itemChooser.AvailableItems[0].IsSelected, Is.False);
            Assert.That(itemChooser.SelectedItems.Count, Is.EqualTo(0));
            itemChooser.AvailableItems[0].IsSelected = true;
            Assert.That(itemChooser.SelectedItems.Count, Is.EqualTo(1));
            itemChooser.AvailableItems[0].IsSelected = false;
            Assert.That(itemChooser.SelectedItems.Count, Is.EqualTo(0));
        }
    }
}
