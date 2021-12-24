namespace Application.RecipeCost.Dto
{
    using Domain.Entities;

    public class RecipeCostDto
    {
        public Recipe Recipe { get; set; }
        public decimal SaleTax { get; set; }
        public decimal WellnessDiscount { get; set; }
        public decimal Total { get; set; }
        public string SaleTaxFormatted => $"{SaleTax:C}";
        public string WellnessDiscountFormatted => $"({WellnessDiscount:C})";
        public string TotalFormatted => $"{Total:C}";
    }
}
