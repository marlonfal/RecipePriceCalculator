using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Recipe
    {
        public Recipe()
        {
            RecipeProducts = new HashSet<RecipeProduct>();
        }

        public int RecipeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<RecipeProduct> RecipeProducts { get; set; }
    }
}
