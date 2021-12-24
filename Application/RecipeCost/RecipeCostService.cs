using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Logging;
using Application.Common.Utils;
using Application.Parameters;
using Application.RecipeCost.Dto;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductType = Domain.Enums.ProductType;

namespace Application.RecipeCost
{
    public class RecipeCostService : IRecipeCostService
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<RecipeCostService> _logger;

        private readonly decimal _saleTaxPercentage;
        private readonly decimal _wellnessDiscountPercentage;

        public RecipeCostService(IApplicationDbContext context,
            ILogger<RecipeCostService> logger,
            IParameterService parameterService)
        {
            _context = context;
            _logger = logger;

            _saleTaxPercentage = parameterService.Get_Decimal(Domain.Enums.Parameters.SaleTax) / 100;
            _wellnessDiscountPercentage = parameterService.Get_Decimal(Domain.Enums.Parameters.WellnessDiscount) / 100;
        }

        public async Task<List<RecipeCostDto>> Get()
        {
            try
            {
                var recipes = await _context
                    .Recipes
                    .Include(x => x.RecipeProducts).ThenInclude(x => x.Product)
                    .ToListAsync();

                return recipes.Select(GetCost).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ExceptionHelper.GetCurrentMethod());
            }

            return null;
        }

        public async Task<RecipeCostDto> Get(int recipeId)
        {
            try
            {
                var recipe = await _context
                    .Recipes
                    .Include(x => x.RecipeProducts).ThenInclude(y => y.Product)
                    .FirstOrDefaultAsync(x => x.RecipeId == recipeId);

                if (recipe != null)
                    return GetCost(recipe);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ExceptionHelper.GetCurrentMethod());
            }

            return null;
        }

        public RecipeCostDto GetCost(Recipe recipe)
        {
            decimal saleTax = 0;
            decimal wellnessDiscount = 0;
            decimal total = 0;

            foreach (var product in recipe.RecipeProducts)
            {
                var productPrice = GetProductPrice(product.Product.Price, product.Quantity);
                total += productPrice;

                switch ((ProductType)product.Product.ProductTypeId)
                {
                    case ProductType.Meat:
                    case ProductType.Pantry:

                        saleTax += GetProductTax(productPrice);
                        break;
                }

                if (product.Product.IsOrganic)
                    wellnessDiscount += GetProductWellnessDiscount(productPrice);
            }

            wellnessDiscount = Round.Amount(wellnessDiscount, 0.01M, Round.Type.Up);
            saleTax = Round.Amount(saleTax, 0.07M, Round.Type.Up);

            return new RecipeCostDto()
            {
                Recipe = recipe,
                SaleTax = saleTax,
                WellnessDiscount = wellnessDiscount,
                Total = Math.Round(total + saleTax - wellnessDiscount, 2, MidpointRounding.AwayFromZero)
            };
        }

        public decimal GetProductPrice(decimal price, decimal quantity)
        {
            return price * quantity;
        }

        public decimal GetProductTax(decimal price)
        {
            return price * _saleTaxPercentage;
        }

        public decimal GetProductWellnessDiscount(decimal price)
        {
            return price * _wellnessDiscountPercentage;
        }

        public decimal GetSaleTaxPercentage() => _saleTaxPercentage;
        public decimal GetWellnessDiscountPercentage() => _wellnessDiscountPercentage;
    }
}
