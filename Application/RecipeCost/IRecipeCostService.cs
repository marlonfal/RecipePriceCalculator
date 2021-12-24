using System.Collections.Generic;
using Application.RecipeCost.Dto;
using Domain.Entities;

namespace Application.RecipeCost
{
    public interface IRecipeCostService
    {
        List<RecipeCostDto> GetRecipesCost();
        List<Recipe> Get();
        Recipe Get(int id);
    }
}
