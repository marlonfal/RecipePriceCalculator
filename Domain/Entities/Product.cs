using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entities
{
    public partial class Product
    {
        public Product()
        {
            RecipeProducts = new HashSet<RecipeProduct>();
        }

        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsOrganic { get; set; }

        public virtual ProductType ProductType { get; set; }
        [JsonIgnore]
        public virtual ICollection<RecipeProduct> RecipeProducts { get; set; }
    }
}
