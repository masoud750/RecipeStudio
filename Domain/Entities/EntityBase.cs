using System.ComponentModel.DataAnnotations;


namespace RecipeStudio.Domain.Entities
{
    public abstract class EntityBase
    {
        public EntityBase() { }


        [Key]
        public int Id { get; set; }
    }
}
