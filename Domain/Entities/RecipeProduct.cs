namespace Domain.Entities
{
    public partial class RecipeProduct
    {
        public int RecipeProductId { get; set; }
        public int ProductId { get; set; }
        public int RecipeId { get; set; }
        public decimal Quantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
