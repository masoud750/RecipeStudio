using CommunityToolkit.Mvvm.Input;
using RecipesDataAccess.Models;
using RecipeStudioUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RecipesUI.Tests.ViewModelsTests
{
    [TestFixture]
    internal class CategoryEditorVMTests
    {

        [Test]
        public void Constructor_InvalidCategory_ThrowsArgumentNullOrEmpty()
        {
            ObservableCollection<CategoryDM> selectedCategories = new();
            var ex = Assert.Throws<ArgumentNullException>(
                () =>
                {
                    _ = new CategoryEditorVM(null!,selectedCategories);
                });          
            
            Assert.That(ex.Message, Does.Contain("availableCategories"));
        }

        [Test]
        public void Constructor_InvalidSelectedCategory_ThrowsArgumentNullOrEmpty()
        {
            ObservableCollection<CategoryDM> availableCategories = new();
            var ex = Assert.Throws<ArgumentNullException>(
                () =>
                {
                    _ = new CategoryEditorVM(availableCategories,null!);
                });

            Assert.That(ex.Message, Does.Contain("selectedCategories"));
        }


        [Test]
        public void SelectAllCmd_SelectAllAvailableCategories_Success()
        {
            ObservableCollection<CategoryDM> selectedCategories = new();
            ObservableCollection<CategoryDM> availableCategories = new();

            var category = new CategoryDM()
            {
                Name = "One",

            };
            var categoryTwo = new CategoryDM()
            {
                Name = "Two",

            };

            var categoryThree = new CategoryDM()
            {
                Name = "Tree",
            };


            availableCategories.Add(category);
            availableCategories.Add(categoryTwo);
            availableCategories.Add(categoryThree);
            var categoryEditorVM = new CategoryEditorVM(availableCategories
                                                            ,selectedCategories);
            categoryEditorVM.IsLogInDone = true;
            System.Windows.Input.ICommand selectAllCmd = categoryEditorVM
                                                                 .SelectAllCmd;
            selectAllCmd.Execute(null);
            Assert.That(categoryEditorVM.AvailableCategories.All(x => x.IsSelected),
                                                                        Is.True);
            Assert.That(categoryEditorVM.SelectedCategories.Count, Is.EqualTo(3));
            ICommand clearAllCmd = categoryEditorVM.ClearCmd;

        }

            [Test]
        public void ClearCmd_ClearSelectedItem_Success()
        {
            ObservableCollection<CategoryDM> selectedCategories = new();
            ObservableCollection<CategoryDM> availableCategories = new();
            
            var category = new CategoryDM()
            {
                Name = "One",

            };
            var categoryTwo = new CategoryDM()
            {
                Name = "Two",

            };

            var categoryThree = new CategoryDM()
            {
                Name = "Tree",
            };


            availableCategories.Add(category);
            availableCategories.Add(categoryTwo);
            availableCategories.Add(categoryThree);
            var categoryEditorVM = new CategoryEditorVM(availableCategories
                                                            ,selectedCategories);

            categoryEditorVM.IsLogInDone = true;
            System.Windows.Input.ICommand selectAllCmd = categoryEditorVM
                                                                 .SelectAllCmd;
            selectAllCmd.Execute(null);
            Assert.That(categoryEditorVM.AvailableCategories.All(x=> x.IsSelected),
                                                                        Is.True);
            Assert.That(categoryEditorVM.SelectedCategories.Count, Is.EqualTo(3));
            ICommand clearAllCmd = categoryEditorVM.ClearCmd;
            clearAllCmd.Execute(null);
            Assert.That(categoryEditorVM.AvailableCategories.All(x => x.IsSelected), 
                                                                    Is.False);
            Assert.That(categoryEditorVM.SelectedCategories.Count, Is.EqualTo(0));

        }

        [Test]
        public void SaveCmd_RaisesCategoryEditCompleted_Success()
        {
            ObservableCollection<CategoryDM> selectedCategories = new();
            ObservableCollection<CategoryDM> availableCategories = new();

            var category = new CategoryDM()
            {
                Name = "One",

            };
           
            
            availableCategories.Add(category);
      
            var categoryEditorVM = new CategoryEditorVM(availableCategories
                                                           , selectedCategories);
            
            bool isTriggered = false;
            categoryEditorVM.CategoryEditCompleted += (s, e) =>
            {
                Assert.IsNotNull(s);
                Assert.That(s.GetType(), Is.EqualTo(categoryEditorVM.GetType()));
                isTriggered = true;

            };
            Assert.IsTrue(categoryEditorVM.SelectedCategories.Count == 0);
            categoryEditorVM.AvailableCategories[0].IsSelected = true;
            Assert.IsTrue(categoryEditorVM.SelectedCategories.Count == 1);
            ICommand save = categoryEditorVM.SaveCmd;        
            save.Execute(null);
            Assert.IsTrue(isTriggered);
            
        }

        [Test]
        public void SaveCmd_CanExecute_ReturnsFalse_WhenUserIsNotLoggedIn()
        {
            ObservableCollection<CategoryDM> selectedCategories = new();
            ObservableCollection<CategoryDM> availableCategories = new();

            var category = new CategoryDM()
            {
                Name = "One",

            };
            
            availableCategories.Add(category);       
            var categoryEditorVM = new CategoryEditorVM(availableCategories
                                                           , selectedCategories);
            bool isTriggered = false;
            categoryEditorVM.CategoryEditCompleted += (s, e) =>
            {
                Assert.IsNotNull(s);
                Assert.That(s.GetType(), Is.EqualTo(categoryEditorVM.GetType()));
                isTriggered = true;

            };
            Assert.IsTrue(categoryEditorVM.SelectedCategories.Count == 0);           
            ICommand save = categoryEditorVM.SaveCmd;
            categoryEditorVM.AvailableCategories[0].IsSelected = true;
            categoryEditorVM.IsLogInDone = false;
            Assert.IsFalse(save.CanExecute(null));
            Assert.IsFalse(categoryEditorVM.IsLogInDone);
        }

        [Test]
        public void SaveCmd_CanExecute_ReturnsFalse_WhenNoItemSelected()
        {
            ObservableCollection<CategoryDM> selectedCategories = new();
            ObservableCollection<CategoryDM> availableCategories = new();

            var category = new CategoryDM()
            {
                Name = "One",

            };

            availableCategories.Add(category);
            var categoryEditorVM = new CategoryEditorVM(availableCategories
                                                           , selectedCategories);
            bool isTriggered = false;
            categoryEditorVM.CategoryEditCompleted += (s, e) =>
            {
                Assert.IsNotNull(s);
                Assert.That(s.GetType(), Is.EqualTo(categoryEditorVM.GetType()));
                isTriggered = true;

            };
            Assert.IsTrue(categoryEditorVM.SelectedCategories.Count == 0);
            ICommand save = categoryEditorVM.SaveCmd;
            categoryEditorVM.IsLogInDone = true;
            Assert.IsFalse(save.CanExecute(null));
            
        }
    }
}
