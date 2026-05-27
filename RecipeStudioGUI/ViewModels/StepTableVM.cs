using RecipesDataAccess.Models;
using RecipeStudioUI.Helpers;
using RecipeStudioUI.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeStudioUI.ViewModels
{

    // Design-time StepVM
    internal class DesignStepVM : StepTableVM
    {
        public DesignStepVM()
            : base(new FakeStepRepository(), new DesignMessageServiceFake())
        {
            Steps.Add(new StepDM { Id = 1, Name = "Preheat oven to 180°C" });
            Steps.Add(new StepDM { Id = 2, Name = "Mix flour and sugar" });
            Steps.Add(new StepDM { Id = 3, Name = "Bake for 30 minutes" });
        }
    }
    public class StepTableVM : ModelBase
    {
      
     

        public StepTableVM(IRepository<StepDM> repository, IMessageService msgService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(Repositories));

            _msgService = msgService ?? throw new ArgumentNullException(nameof(msgService));
         
        }

        public IRepository<StepDM> Repository { get { return _repository; } }

        private readonly IRepository<StepDM> _repository;
        private readonly IMessageService _msgService;
        public ObservableCollection<StepDM> Steps { get; } = new();
      
        public async Task InitializeAsync()
        {
            try
            {
                Steps.Clear();
                var steps = await _repository.GetAllAsync();
                foreach (var step in steps)
                    Steps.Add(step);

                //_msgService.ShowMessage("Steps loaded successfully.",
                //    Helpers.MessageType.Info,"Load Step");
            }
            catch (Exception ex)
            {
                _msgService.ShowMessage($"Error while loading steps: {ex.Message}",
                    Helpers.MessageType.Error,"Load Step");
            }
        }

        public async Task AddAsync(StepDM step)
        {
            try
            {
                await _repository.AddAsync(step);
                Steps.Add(step);
                _msgService.ShowMessage("Step added successfully.",Helpers.MessageType.Info);
            }
            catch (Exception ex)
            {
                _msgService.ShowMessage($"Error while adding step: {ex.Message}",
                    Helpers.MessageType.Error,"Add Data");
            }
        }

        public async Task UpdateAsync(StepDM step)
        {
            try
            {
                await _repository.UpdateAsync(step);
                var existing = Steps.FirstOrDefault(s => s.Id == step.Id);
                if (existing != null)
                {
                    var index = Steps.IndexOf(existing);
                    Steps[index] = step;
                }
                _msgService.ShowMessage("Step updated successfully.",Helpers.MessageType.Info,"Update Data");
            }
            catch (Exception ex)
            {
                _msgService.ShowMessage($"Error while updating step: {ex.Message}", 
                    Helpers.MessageType.Info, "Update Data");
            }
        }

        public async Task DeleteAsync(StepDM step)
        {
            try
            {
                await _repository.DeleteAsync(step.Id);
                Steps.Remove(step);
                _msgService.ShowMessage("Step deleted successfully.", Helpers.MessageType.Info, "Update Data");
            }
            catch (Exception ex)
            {
                _msgService.ShowMessage($"Error while deleting step: {ex.Message}", 
                    Helpers.MessageType.Error, "Update Data");
            }
        }
    }
}
