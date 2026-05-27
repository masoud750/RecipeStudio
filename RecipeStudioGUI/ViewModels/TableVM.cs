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
    public class TableVM<T> : ModelBase where T : class
    {
        

        public TableVM(IRepository<T> repository, IMessageService msg)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _msg = msg ?? throw new ArgumentNullException(nameof(msg));
        }

        private readonly IRepository<T> _repository;
        private readonly IMessageService _msg;

        public ObservableCollection<T> Items { get; } = new();

        public async Task InitializeAsync()
        {
            Items.Clear();
            var all = await _repository.GetAllAsync();
            foreach (var item in all) Items.Add(item);
        }
    }
}
