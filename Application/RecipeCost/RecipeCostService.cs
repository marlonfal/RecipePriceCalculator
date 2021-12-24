using System;
using System.Collections.Generic;
using System.Linq;
using Application.Common.Interfaces;
using Application.Common.Logging;
using Application.Common.Utils;
using Application.Parameters;
using Application.RecipeCost.Dto;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.CompilerServices;
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

            _saleTaxPercentage = parameterService.GetParameter_Decimal(Domain.Enums.Parameters.SaleTax) / 100;
            _wellnessDiscountPercentage = parameterService.GetParameter_Decimal(Domain.Enums.Parameters.WellnessDiscount) / 100;
        }

        public List<Recipe> Get()
        {
            try
            {
                return _context.Recipes.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ExceptionHelper.GetCurrentMethod());
            }

            return new List<Recipe>();
        }

        public Recipe Get(int id)
        {
            try
            {
                return _context.Recipes.FirstOrDefault(x => x.RecipeId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ExceptionHelper.GetCurrentMethod());
            }

            return null;
        }

        public List<RecipeCostDto> GetRecipesCost()
        {
            try
            {
                var recipes = _context
                    .Recipes
                    .Include(x => x.RecipeProducts).ThenInclude(x => x.Product)
                    .ToList();

                return recipes.Select(GetCost).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ExceptionHelper.GetCurrentMethod());
            }

            return new List<RecipeCostDto>();
        }

        public RecipeCostDto GetCost(Recipe recipe)
        {
            decimal saleTax = 0;
            decimal wellnessDiscount = 0;
            decimal total = 0;

            foreach (var product in recipe.RecipeProducts)
            {
                var productPrice = product.Product.Price * product.Quantity;
                total += productPrice;

                switch ((ProductType)product.Product.ProductTypeId)
                {
                    case ProductType.Meat:
                    case ProductType.Pantry:
                        
                        saleTax += productPrice * _saleTaxPercentage;
                        break;
                }

                if (product.Product.IsOrganic)
                    wellnessDiscount += productPrice * _wellnessDiscountPercentage;
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
    }
}
