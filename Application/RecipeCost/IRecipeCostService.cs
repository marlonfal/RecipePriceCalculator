using System.Collections.Generic;
using System.Threading.Tasks;
using Application.RecipeCost.Dto;
using Domain.Entities;

namespace Application.RecipeCost
{
    public interface IRecipeCostService
    {
        Task<List<RecipeCostDto>> Get();
        Task<RecipeCostDto> Get(int recipeId);
    }
}
