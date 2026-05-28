using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RecipeStudio.DataAccess.Data;
using RecipeStudio.Domain.Entities;

namespace RecipesDataAccessIntegrationTests.Tables
{
    [TestFixture]
    internal class IngredientsTests
    {
        // Ingeredient

        [Test]
        public async Task CategoryModel_CanInsertIngredientWithTheSameName_ThrowException()
        {

            var options = new DbContextOptionsBuilder<RecipesDBContext>()
                 .UseSqlite("Filename=:memory:")   // SQLite in-memory database
                     .Options;

            using var context = new RecipesDBContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            // Arrange

            bool isThrown = false;
            var ingredient = new IngredientDM { Name = "Egg" , Quantity = "2" };

            context.Ingredients.Add(ingredient);
            await context.SaveChangesAsync();

            // Act
            try
            {
                var sut = new IngredientDM { Name = "Egg", Quantity ="2" };
                context.Ingredients.Add(sut);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var sqliteEx = ex.InnerException as SqliteException;
                Assert.That(sqliteEx?.SqliteErrorCode, Is.EqualTo(19));
                isThrown = true;
            }

            // Assert
            Assert.IsTrue(isThrown);

        }

        [Test]
        public async Task CategoryModel_AddingDuplicateNameWithDiffFormat_ThrowException()
        {

            var options = new DbContextOptionsBuilder<RecipesDBContext>()
                 .UseSqlite("Filename=:memory:")   // SQLite in-memory database
                     .Options;

            using var context = new RecipesDBContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            // Arrange

            bool isThrown = false;
            var ingredient = new IngredientDM { Name = "Egg", Quantity = "2" };

            context.Ingredients.Add(ingredient);
            await context.SaveChangesAsync();

            // Act
            try
            {
                var sut = new IngredientDM { Name = "EgG" , Quantity = "2"};
                context.Ingredients.Add(sut);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var sqliteEx = ex.InnerException as SqliteException;
                Assert.That(sqliteEx?.SqliteErrorCode, Is.EqualTo(19));
                isThrown = true;
            }

            // Assert
            Assert.IsTrue(isThrown);

        }

        [Test]
        public async Task RecipeModel_HasIngredientsRelation_SavesSuccessfully()
        {
            using var context = TestHelper.GetDbContext();

            // Arrange: create a user
            var user = new UserDM { UserName = "chef_ingredient", Email = "ingredient@test.com", PasswordHash = "abc123" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Create ingredients
            var flour = new IngredientDM { Name = "Flour" , Quantity = "2" };
            var sugar = new IngredientDM { Name = "Sugar" , Quantity = "2"   };
            var eggs = new IngredientDM { Name = "Eggs" , Quantity = "2" };

            // Create recipe with ingredients
            var recipe = new RecipeDM("Cake", user.Id,
                new List<CategoryDM> { new CategoryDM { Name = "Dessert" } },
                new List<StepDM> { new StepDM { Name = "Mix ingredients" } },
                new List<IngredientDM> { flour, sugar, eggs });

            context.Recipes.Add(recipe);
            await context.SaveChangesAsync();

            // Act: reload recipe with ingredients
            var savedRecipe = await context.Recipes
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Name == "Cake");

            // Assert: recipe has all ingredients
            Assert.IsNotNull(savedRecipe);
            Assert.That(savedRecipe.Ingredients.Count, Is.EqualTo(3));
            Assert.That(savedRecipe.Ingredients.Any(i => i.Name == "Flour"), Is.True);
            Assert.That(savedRecipe.Ingredients.Any(i => i.Name == "Sugar"), Is.True);
            Assert.That(savedRecipe.Ingredients.Any(i => i.Name == "Eggs"), Is.True);
        }

        
    }
}
