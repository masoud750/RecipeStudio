
namespace RecipeStudio.Domain.Entities
{
    public class FavoriteDM
    {
        public int RecipeId { get; set; }
        public RecipeDM Recipe { get; set; } = null!;

        public int UserId { get; set; }
        public UserDM User { get; set; } = null!;
    }

}
