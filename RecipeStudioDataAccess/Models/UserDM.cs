using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesDataAccess.Models
{
    public class UserDM: EntityBase
    {
     
      
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        public DateTime ValidFrom { get; set; } = DateTime.Now;
        public DateTime ValidTo { get; set; }= new DateTime(2090, 1, 1, 0, 0, 0);

        [NotMapped]
        public bool IsExpired => ValidTo < DateTime.Now;

        // Navigation
        public ICollection<RecipeDM> Recipes { get; set; } = new List<RecipeDM>();
        //public ICollection<FavoriteDM> Favorites { get; set; } = new List<FavoriteDM>(); 

        public ICollection<RecipeDM> FavoriteRecipes { get; set; } = new List<RecipeDM>();

    }
   
}
