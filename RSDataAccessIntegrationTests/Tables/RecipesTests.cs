using RecipeStudio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace RecipesDataAccessIntegrationTests.Tables
{
    [TestFixture]
    internal class RecipesTests
    {

        // Recipe

        [Test]
        public async Task RecipeModel_CanInsertRecipe_SavesSuccessfully()
        {
            // Arrange
            using var context = TestHelper.GetDbContext();

            // First create a user (since Recipe requires a UserId foreign key)
            var user = new UserDM { UserName = "chef1", Email = "chef1@test.com", PasswordHash = "7##65" };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var category = new CategoryDM { Name = "Dessert" };
            //context.Categories.Add(category);
            //await context.SaveChangesAsync();
            var ingredient = new IngredientDM { Name = "Egg" , Quantity = "2" };
            var step = new StepDM { Name = "Mix flour and sugar" };
            var recipe = new RecipeDM(

                      "Chocolate Cake",
                      user.Id,
                      new List<CategoryDM> { category },
                      new List<StepDM> { step },
                      new List<IngredientDM> { ingredient }
                      );



            // Act
            context.Recipes.Add(recipe);
            await context.SaveChangesAsync();

            // Assert
            var savedRecipe = await context.Recipes.FirstOrDefaultAsync(r => r.Name == "Chocolate Cake");
            Assert.IsNotNull(savedRecipe);
            Assert.That(savedRecipe.Name, Is.EqualTo(recipe.Name));
            Assert.That(savedRecipe.UserId, Is.EqualTo(user.Id));
            Assert.That(savedRecipe.ValidFrom, Is.Not.EqualTo(DateTime.MinValue)); // not default 0001-01-01
            Assert.That(savedRecipe.ValidFrom.Date, Is.EqualTo(DateTime.Today));   // matches today's date
            Assert.That(savedRecipe.ValidTo, Is.EqualTo(new DateTime(2090, 1, 1, 0, 0, 0)));
        }


        [Test]
        public async Task RecipesTable_HasCategoriesRelation_SavesSuccessfully()
        {
            using var context = TestHelper.GetDbContext();

            // Arrange: create a user
            var user = new UserDM { UserName = "chef_category", Email = "category@test.com", PasswordHash = "abc123" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Create categories
            var dessert = new CategoryDM { Name = "Dessert" };
            var breakfast = new CategoryDM { Name = "Breakfast" };

            // Create recipe with categories
            var recipe = new RecipeDM("Chocolate Pancakes", user.Id,
                new List<CategoryDM> { dessert, breakfast },
                new List<StepDM> { new StepDM { Name = "Mix batter" } },
                new List<IngredientDM> { new IngredientDM { Name = "Eggs", Quantity = "2" } });

            context.Recipes.Add(recipe);
            await context.SaveChangesAsync();

            // Act: reload recipe with categories
            var savedRecipe = await context.Recipes
                .Include(r => r.Categories)
                .FirstOrDefaultAsync(r => r.Name == "Chocolate Pancakes");

            // Assert: recipe has both categories
            Assert.IsNotNull(savedRecipe);
            Assert.That(savedRecipe.Categories.Count, Is.EqualTo(2));
            Assert.That(savedRecipe.Categories.Any(c => c.Name == "Dessert"), Is.True);
            Assert.That(savedRecipe.Categories.Any(c => c.Name == "Breakfast"), Is.True);

            // Assert: category knows its recipes
            var savedCategory = await context.Categories
                .Include(c => c.Recipes)
                .FirstOrDefaultAsync(c => c.Name == "Dessert");

            Assert.IsNotNull(savedCategory);
            Assert.That(savedCategory.Recipes.Any(r => r.Name == "Chocolate Pancakes"), Is.True);
        }

        [Test]
        public async Task RecipesTable_HasStepsRelation_SavesSuccessfully()
        {
            using var context = TestHelper.GetDbContext();

            // Arrange: create a user
            var user = new UserDM { UserName = "chef_steps", Email = "steps@test.com", PasswordHash = "abc123" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Create steps
            var step1 = new StepDM { Name = "Mix flour and sugar" };
            var step2 = new StepDM { Name = "Add eggs and whisk" };

            // Create recipe with steps
            var recipe = new RecipeDM("Pancakes", user.Id,
                new List<CategoryDM> { new CategoryDM { Name = "Breakfast" } },
                new List<StepDM> { step1, step2 },
                new List<IngredientDM> { new IngredientDM { Name = "Milk" , Quantity = "200 ml" } });

            context.Recipes.Add(recipe);
            await context.SaveChangesAsync();

            // Act: reload recipe with steps
            var savedRecipe = await context.Recipes
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.Name == "Pancakes");

            // Assert
            Assert.IsNotNull(savedRecipe);
            Assert.That(savedRecipe.Steps.Count, Is.EqualTo(2));
            Assert.That(savedRecipe.Steps.Any(s => s.Name == "Mix flour and sugar"), Is.True);
            Assert.That(savedRecipe.Steps.Any(s => s.Name == "Add eggs and whisk"), Is.True);
        }


        [Test]
        public void SeededUsers_ShouldExist()
        {
            using var context = TestHelper.GetDbContext();

            
            
                context.Database.EnsureCreated();

                var user = context.Users.FirstOrDefault(u => u.UserName == "ChefAnna");
                Assert.NotNull(user);
                Assert.AreEqual("anna@example.com", user.Email);
            
        }

        [Test]
        public void Recipe_ShouldHaveCategoryRelation()
        {

            using var context = TestHelper.GetDbContext();

           
            
                context.Database.EnsureCreated();

                var recipe = context.Recipes
                    .Include(r => r.Categories)
                    .FirstOrDefault(r => r.Name == "Spaghetti Carbonara");

                Assert.NotNull(recipe);
                Assert.IsTrue(recipe.Categories.Any(c => c.Name == "Italian"));
            
        }

        [Test]
        public void User_ShouldHaveFavoriteRecipe()
        {
            using var context = TestHelper.GetDbContext();

            var user = context.Users
                .Include(u => u.FavoriteRecipes)
                .FirstOrDefault(u => u.UserName == "ChefSam");
            Assert.NotNull(user);
            user = context.Users.Include(u => u.FavoriteRecipes)
                .FirstOrDefault(u => u.FavoriteRecipes.Any(
         r => r.Name == "Spaghetti Carbonara"));
            Assert.NotNull(user);
            Assert.That(user.UserName, Is.EqualTo("ChefSam"));
        }

        [Test]
        public void CheckMapIdToColumnName()
        {
            using var context = TestHelper.GetDbContext();

            var entityType = context.Model.FindEntityType(typeof(UserDM));
            Assert.NotNull(entityType);
            var idColumn = entityType.FindProperty("Id").GetColumnName();
            
            Assert.That(idColumn, Is.EqualTo("UserId"));


            entityType = context.Model.FindEntityType(typeof(RecipeDM));
            Assert.NotNull(entityType);
            idColumn = entityType.FindProperty("Id").GetColumnName();
            Assert.That(idColumn, Is.EqualTo("RecipeId"));


            entityType = context.Model.FindEntityType(typeof(CategoryDM));
            Assert.NotNull(entityType);
            idColumn = entityType.FindProperty("Id").GetColumnName();
            Assert.That(idColumn, Is.EqualTo("CategoryId"));


            entityType = context.Model.FindEntityType(typeof(IngredientDM));
            Assert.NotNull(entityType);
            idColumn = entityType.FindProperty("Id").GetColumnName();
            Assert.That(idColumn, Is.EqualTo("IngredientId"));


            entityType = context.Model.FindEntityType(typeof(StepDM));
            Assert.NotNull(entityType);
            idColumn = entityType.FindProperty("Id").GetColumnName();
            Assert.That(idColumn, Is.EqualTo("StepId"));
        }
    }
}
