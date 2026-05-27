
using RecipesDataAccess.Models;
using RecipeStudioUI.Commands;
using RecipeStudioUI.Helpers;
using RecipeStudioUI.Repositories;
using RecipeStudioUI.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RecipeStudioUI.ViewModels
{


    public class DesignCategoryVM : CategoryTableVM
    {
        public DesignCategoryVM()
            : base(new FakeCategoryRepository(), new DesignMessageServiceFake())
        {
            Categories.Add(new CategoryVM(new CategoryDM { Id = 1, Name = "Design Cat 1" }));
            Categories.Add(new CategoryVM(new CategoryDM { Id = 2, Name = "Design Cat 2" }));
            Categories.Add(new CategoryVM(new CategoryDM { Id = 3, Name = "Design Cat 3" }));
        }
    }


    public class CategoryTableVM : ModelBase
    {
    

        public CategoryTableVM(IRepository<CategoryDM> repository, IMessageService msgService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _msgService = msgService ?? throw new ArgumentNullException(nameof(msgService));
        }

        public ObservableCollection<CategoryVM> Categories { get; }
            = new ObservableCollection<CategoryVM>();

        public IRepository<CategoryDM> Repository {  get {  return _repository; }  }

        private readonly IRepository<CategoryDM> _repository;
        private readonly IMessageService _msgService;

        public async Task InitializeAsync()
        {
            try
            {
                await LoadAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Initialization failed: {ex.Message}");
                _msgService.ShowMessage("Unable to load data. Please try again.",
                    Helpers.MessageType.Warning, "Warning");
            }
        }

        private async Task LoadAsync()
        {
            Categories.Clear();
            var cats = await _repository.GetAllAsync();
            foreach (var cat in cats)
            {
                if (!cat.IsExpired)
                    Categories.Add(new CategoryVM(cat));
            }
        }

        public async Task AddAsync(CategoryDM category)
        {
            try
            {
                await _repository.AddAsync(category);
                Categories.Add(new CategoryVM(category));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Add failed: {ex.Message}");
                _msgService.ShowMessage("Unable to Add data.",
                    Helpers.MessageType.Warning, "Warning");
            }
        }

        public  async Task UpdateAsync(CategoryVM categoryVM)
        {
            try
            {
                await _repository.UpdateAsync(categoryVM.Model);

                var existing = Categories.FirstOrDefault(c => c.CategoryId == categoryVM.CategoryId);
                if (existing != null)
                {
                    var index = Categories.IndexOf(existing);
                    Categories[index] = categoryVM;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update failed: {ex.Message}");
                _msgService.ShowMessage("Unable to update data.",
                    Helpers.MessageType.Warning, "Warning");
            }
        }

        public async Task DeleteAsync(CategoryVM categoryVM)
        {
            try
            {
                await _repository.DeleteAsync(categoryVM.Model.Id);
                Categories.Remove(categoryVM);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete failed: {ex.Message}");
                _msgService.ShowMessage("Unable to delete data.",
                    Helpers.MessageType.Warning, "Warning");
            }
        }

        public CategoryVM CurrentSelectedItem
        {
            get => _currentSelectedItem;
            set
            {
                if (_currentSelectedItem != value)
                {
                    _currentSelectedItem = value;
                    OnPropertyChanged();
                }
            }
        }
        private CategoryVM _currentSelectedItem;


        internal void FakeDelete()
        {
            try
            {
                // Work with the wrapped model
                CurrentSelectedItem.Model.ValidTo = DateTime.Now; // or set IsExpired flag
                _repository.SaveChanges();

                Categories.Remove(CurrentSelectedItem);

                _msgService.ShowMessage("Category deleted successfully.",
                    Helpers.MessageType.Info, "Delete Data");
            }
            catch (Exception ex)
            {
                _msgService.ShowMessage(
                    $"Error while deleting Category: {ex.Message}",
                    Helpers.MessageType.Error, "Delete Data");
            }
        }


        public ObservableCollection<CategoryDM> CategoryModels
        {
            get
            {
                return new ObservableCollection<CategoryDM>(
                    Categories.Select(vm => vm.Model)
                );
            }
        }


        public void OnCategoryNameChanged()
        {
            EditCategory?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler EditCategory;
    }
}
