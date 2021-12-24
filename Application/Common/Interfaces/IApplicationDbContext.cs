using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<Recipe> Recipes { get; set; }
        DbSet<RecipeProduct> RecipeProducts { get; set; }
        DbSet<ProductType> ProductTypes { get; set; }
        DbSet<Parameter> Parameters { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
