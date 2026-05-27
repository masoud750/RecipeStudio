using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesDataAccess.Models
{
    public class StepDM: EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public DateTime ValidFrom { get; set; } = DateTime.Now;
        public DateTime ValidTo { get; set; } = new DateTime(2090, 1, 1, 0, 0, 0);

        [NotMapped]
        public bool IsExpired => ValidTo < DateTime.Now;

        // Navigation
        public ICollection<RecipeDM> Recipes { get; set; } = new List<RecipeDM>();
    }
}
