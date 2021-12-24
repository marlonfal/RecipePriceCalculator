using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Identity;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private readonly IDomainEventService _domainEventService;

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            ICurrentUserService currentUserService,
            IDomainEventService domainEventService,
            IDateTime dateTime) : base(options, operationalStoreOptions)
        {
            _currentUserService = currentUserService;
            _domainEventService = domainEventService;
            _dateTime = dateTime;
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeProduct> RecipeProducts { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Parameter> Parameters { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            await DispatchEvents();

            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);

            builder.Entity<ProductType>().HasData(
                new ProductType()
                {
                    ProductTypeId = 1,
                    Name = "Produce"
                },
                new ProductType()
                {
                    ProductTypeId = 2,
                    Name = "Meat/Poultry"
                },
                new ProductType()
                {
                    ProductTypeId = 3,
                    Name = "Pantry"
                }
            );

            builder.Entity<Product>().HasData(
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
            );

            builder.Entity<Recipe>().HasData(
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
            );

            builder.Entity<RecipeProduct>().HasData(
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
                new RecipeProduct { RecipeProductId = 15, ProductId = 7, RecipeId = 3, Quantity = 0.33m },
                new RecipeProduct { RecipeProductId = 16, ProductId = 9, RecipeId = 3, Quantity = 1 },
                new RecipeProduct { RecipeProductId = 17, ProductId = 10, RecipeId = 3, Quantity = 0.75m }
            );

            builder.Entity<Parameter>().HasData(
                new Parameter {ParameterId = 1, Key = "SaleTax", Value = "8.6"},
                new Parameter {ParameterId = 2, Key = "WellnessDiscount", Value = "5"}
            );
        }

        private async Task DispatchEvents()
        {
            while (true)
            {
                var domainEventEntity = ChangeTracker
                    .Entries<IHasDomainEvent>()
                    .Select(x => x.Entity.DomainEvents)
                    .SelectMany(x => x)
                    .FirstOrDefault(domainEvent => !domainEvent.IsPublished);
                if (domainEventEntity == null) break;

                domainEventEntity.IsPublished = true;
                await _domainEventService.Publish(domainEventEntity);
            }
        }
    }
}
