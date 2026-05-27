using Domain.Entities;
using RecipeStudioUI.Helpers;
using RecipeStudioUI.Repositories;
using System.Collections.ObjectModel;


namespace RecipeStudioUI.ViewModels
{
    public class DesignIngredientVM : IngredientTableVM
    {
        public DesignIngredientVM()
            : base(new FakeIngredientRepository(), new DesignMessageServiceFake())
        {
            // Fill with sample data for design-time preview
            Ingredients.Add(new IngredientDM { Id = 1, Name = "Flour",Quantity = "500g" });
            Ingredients.Add(new IngredientDM { Id = 2, Name = "Milk", Quantity = "250ml" });
            Ingredients.Add(new IngredientDM { Id = 3, Name = "Eggs", Quantity = "3 pcs" });
            Ingredients.Add(new IngredientDM { Id = 4, Name = "Sugar",Quantity = "100g" });
        }
    }

    public class IngredientTableVM : ModelBase
    {
        public IngredientTableVM(IRepository<IngredientDM> repository, IMessageService msgService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _msgService = msgService ?? throw new ArgumentNullException(nameof(msgService));
        }
        // Alias-Property für Test mit CategoryUserCtrl
      
        public IRepository<IngredientDM> Repository { get { return _repository; } }


        private readonly IRepository<IngredientDM> _repository;
        private readonly IMessageService _msgService;
        public ObservableCollection<IngredientDM> Ingredients { get; } = new();

        public async Task InitializeAsync()
        {
            const string caption = "Load Ingredients";
            try
            {
                Ingredients.Clear();
                var items = await _repository.GetAllAsync();
                foreach (var i in items) Ingredients.Add(i);

                //_msgService.ShowMessage("Ingredients loaded successfully.", 
                //    MessageType.Info, caption);
            }
            catch (Exception ex)
            {
                _msgService.ShowMessage(
                    $"Failed to load ingredients.\nType: " +
                    $"{ex.GetType().Name}\nDetails: {ex.Message}",
                                  MessageType.Error,caption);
            }
        }

        public async Task AddAsync(IngredientDM ingredient)
        {
            const string caption = "Add Ingredient";
            try
            {
                await _repository.AddAsync(ingredient);
                await _repository.SaveChangesAsync();
                Ingredients.Add(ingredient);

                _msgService.ShowMessage("Ingredient added.",  MessageType.Info, caption);
            }
            catch (Exception ex)
            {
                _msgService.ShowMessage(
                    $"Failed to add ingredient.\nType: " +
                    $"{ex.GetType().Name}\nDetails: {ex.Message}",
                                   Helpers.MessageType.Info, "Add Data");
            }
        }

       

        public async Task UpdateAsync(IngredientDM ingredient)
        {
            const string caption = "Update Ingredient";
            try
            {
                await _repository.UpdateAsync(ingredient);
                await _repository.SaveChangesAsync();

                var existing = Ingredients.FirstOrDefault(
                    i => i.Id == ingredient.Id);
                if (existing != null)
                {
                    var idx = Ingredients.IndexOf(existing);
                    Ingredients[idx] = ingredient;
                }

                _msgService.ShowMessage("Ingredient updated.", MessageType.Info,caption);
            }
            catch (Exception ex)
            {
                _msgService.ShowMessage(
                    $"Failed to update ingredient.\nType: " +
                    $"{ex.GetType().Name}\nDetails: {ex.Message}",
                      Helpers.MessageType.Error, caption
                                 );
            }
        }

        public async Task DeleteAsync(IngredientDM ingredient)
        {
            const string caption = "Delete Ingredient";
            try
            {
                await _repository.DeleteAsync(ingredient.Id);
                await _repository.SaveChangesAsync();
                Ingredients.Remove(ingredient);

                _msgService.ShowMessage("Ingredient deleted.", MessageType.Info,
                     caption);
            }
            catch (Exception ex)
            {
                _msgService.ShowMessage($"Failed to delete ingredient.\nType: " +
                    $"{ex.GetType().Name}\nDetails: {ex.Message}",
                                 MessageType.Error, caption);
            }
        }
    }
}
