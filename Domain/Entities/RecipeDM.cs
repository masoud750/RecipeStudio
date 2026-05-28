using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RecipeStudio.Domain.Entities
{
    public class RecipeDM: EntityBase
    {
        // EF Core will use this
        public RecipeDM()
        {
            Categories = new List<CategoryDM>();
            Steps = new List<StepDM>();
            Ingredients = new List<IngredientDM>();
        }

        public RecipeDM(string name, int userId,
                   IEnumerable<CategoryDM> categories,
                   IEnumerable<StepDM> steps,
                   IEnumerable<IngredientDM> ingredients)
        {
         
            ArgumentNullException.ThrowIfNull(categories, nameof(categories));
            ArgumentNullException.ThrowIfNull(steps, nameof(steps));
            ArgumentNullException.ThrowIfNull(ingredients, nameof(ingredients));
            if (!categories.Any()) throw new ArgumentException("Recipe must have at least one category.");
            if (!steps.Any()) throw new ArgumentException("Recipe must have at least one step.");
            if (!ingredients.Any()) throw new ArgumentException("Recipe must have at least one ingredient.");

            Name = name;
            UserId = userId;
            Categories = categories.ToList();
            Steps = steps.ToList();
            Ingredients = ingredients.ToList();
        }

    
       
        public string Name { get; set; } = string.Empty;
        public string Description {  get; set; } = string.Empty;
        public DateTime ValidFrom { get; set; } = DateTime.Now;
        public DateTime ValidTo { get; set; } = new DateTime(2090, 1, 1, 0, 0, 0);

        [NotMapped]
        public bool IsExpired => ValidTo < DateTime.Now;

        // Foreign key
        public int UserId { get; set; }
        public UserDM User { get; set; } = null!;

        // Navigation
        public ICollection<CategoryDM> Categories { get; set; } = new List<CategoryDM>();
        public ICollection<StepDM> Steps { get; set; } = new List<StepDM>();
        public ICollection<IngredientDM> Ingredients { get; set; } = new List<IngredientDM>();

        public ICollection<UserDM> FavoritedBy { get; set; } = new List<UserDM>();
        // Navigation    

    }
}
