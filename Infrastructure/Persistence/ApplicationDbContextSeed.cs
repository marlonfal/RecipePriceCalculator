using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var administratorRole = new IdentityRole("Administrator");

            if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                await roleManager.CreateAsync(administratorRole);
            }

            var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

            if (userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await userManager.CreateAsync(administrator, "Administrator1!");
                await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }

        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            // Seed, if necessary
            if (!context.TodoLists.Any())
            {
                context.TodoLists.Add(new TodoList
                {
                    Title = "Shopping",
                    Items =
                    {
                        new TodoItem { Title = "Apples", Done = true },
                        new TodoItem { Title = "Milk", Done = true },
                        new TodoItem { Title = "Bread", Done = true },
                        new TodoItem { Title = "Toilet paper" },
                        new TodoItem { Title = "Pasta" },
                        new TodoItem { Title = "Tissues" },
                        new TodoItem { Title = "Tuna" },
                        new TodoItem { Title = "Water" }
                    }
                });

                context.ProductTypes.AddRange(new List<ProductType>()
                {
                    new ProductType()
                    {
                        ProductTypeId = 1,
                        Name = "Produce"
                    },
                    new ProductType()
                    {
                        ProductTypeId = 2,
                        Name= "Meat/Poultry"
                    },
                    new ProductType()
                    {
                        ProductTypeId = 3,
                        Name= "Pantry"
                    }
                });

                context.Products.AddRange(new List<Product>()
                {
                    // Produce
                    new Product()
                    {
                        ProductId = 1,
                        ProductTypeId = 1,
                        IsOrganic = true,
                        Name = "Clove of organic garlic",
                        Price = 0.67m
                    },
                    new Product()
                    {
                        ProductId = 2,
                        ProductTypeId = 1,
                        IsOrganic = false,
                        Name = "Lemon",
                        Price = 2.03m
                    },
                    new Product()
                    {
                        ProductId = 3,
                        ProductTypeId = 1,
                        IsOrganic = false,
                        Name = "Cup of corn",
                        Price = 0.87m
                    },
                    // Meat/Poultry
                    new Product()
                    {
                        ProductId = 4,
                        ProductTypeId = 2,
                        IsOrganic = false,
                        Name = "Chicken breast",
                        Price = 2.19m
                    },
                    new Product()
                    {
                        ProductId = 5,
                        ProductTypeId = 2,
                        IsOrganic = false,
                        Name = "Slice of bacon",
                        Price = 0.24m
                    },
                    // Pantry
                    new Product()
                    {
                        ProductId = 6,
                        ProductTypeId = 3,
                        IsOrganic = false,
                        Name = "Ounce of pasta",
                        Price = 0.31m
                    },
                    new Product()
                    {
                        ProductId = 7,
                        ProductTypeId = 3,
                        IsOrganic = true,
                        Name = "Cup of organic olive oil",
                        Price = 1.92m
                    },
                    new Product()
                    {
                        ProductId = 8,
                        ProductTypeId = 3,
                        IsOrganic = false,
                        Name = "Cup of vinegar",
                        Price = 1.26m
                    },
                    new Product()
                    {
                        ProductId = 9,
                        ProductTypeId = 3,
                        IsOrganic = false,
                        Name = "Teaspoon of salt",
                        Price = 0.16m
                    },
                    new Product()
                    {
                        ProductId = 10,
                        ProductTypeId = 3,
                        IsOrganic = false,
                        Name = "Teaspoon of pepper",
                        Price = 0.17m
                    }
                });

                context.Recipes.AddRange(new List<Recipe>()
                {
                    new Recipe()
                    {
                        RecipeId = 1,
                        Name = "Recipe 1"
                    },
                    new Recipe()
                    {
                        RecipeId = 2,
                        Name = "Recipe 2"
                    },
                    new Recipe()
                    {
                        RecipeId = 3,
                        Name = "Recipe 3"
                    }
                });

                context.RecipeProducts.AddRange(new List<RecipeProduct>()
                {
                    // Recipe 1
                    new RecipeProduct { RecipeProductId = 1, ProductId = 1, RecipeId = 1, Quantity = 1 },
                    new RecipeProduct { RecipeProductId = 2, ProductId = 2, RecipeId = 1, Quantity = 1 },
                    new RecipeProduct { RecipeProductId = 3, ProductId = 7, RecipeId = 1, Quantity = 0.75m },
                    new RecipeProduct { RecipeProductId = 4, ProductId = 9, RecipeId = 1, Quantity = 0.75m },
                    new RecipeProduct { RecipeProductId = 5, ProductId = 10, RecipeId = 1, Quantity = 0.5m },
                    // Recipe 2       
                    new RecipeProduct { RecipeProductId = 6, ProductId = 1, RecipeId = 2, Quantity = 1 },
                    new RecipeProduct { RecipeProductId = 7, ProductId = 4, RecipeId = 2, Quantity = 4 },
                    new RecipeProduct { RecipeProductId = 8, ProductId = 7, RecipeId = 2, Quantity = 0.5m },
                    new RecipeProduct { RecipeProductId = 9, ProductId = 8, RecipeId = 2, Quantity = 0.5m },
                    // Recipe 3       
                    new RecipeProduct { RecipeProductId = 11, ProductId = 1, RecipeId = 3, Quantity = 1 },
                    new RecipeProduct { RecipeProductId = 12, ProductId = 3, RecipeId = 3, Quantity = 4 },
                    new RecipeProduct { RecipeProductId = 13, ProductId = 5, RecipeId = 3, Quantity = 4 },
                    new RecipeProduct { RecipeProductId = 14, ProductId = 6, RecipeId = 3, Quantity = 8 },
                    new RecipeProduct { RecipeProductId = 15, ProductId = 7, RecipeId = 3, Quantity = 0.33333333333m },
                    new RecipeProduct { RecipeProductId = 16, ProductId = 9, RecipeId = 3, Quantity = 1 },
                    new RecipeProduct { RecipeProductId = 17, ProductId = 10, RecipeId = 3, Quantity = 0.75m },
                });

                context.Parameters.AddRange(new List<Parameter>()
                {
                    new Parameter {ParameterId = 1, Key = "SaleTax", Value = "8.6"},
                    new Parameter {ParameterId = 2, Key = "WellnessDiscount", Value = "5"}
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
