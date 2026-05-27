using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesDataAccess.Models
{
    public abstract class EntityBase
    {
        public EntityBase() { }


        [Key]
        public int Id { get; set; }
    }
}
