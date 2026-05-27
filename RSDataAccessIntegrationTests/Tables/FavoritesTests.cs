using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RecipesDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesDataAccessIntegrationTests.Tables
{
    internal class FavoritesTests
    {

        [Test]
        public async Task User_CanFavoriteRecipeOfAnotherUser_SavesSuccessfully()
        {
            using var context = TestHelper.GetDbContext();

            // Arrange: create two users
            var author = new UserDM { UserName = "chef_author", Email = "author@test.com", PasswordHash = "abc123" };
            var fan = new UserDM { UserName = "chef_fan", Email = "fan@test.com", PasswordHash = "xyz789" };

            context.Users.Add(author);
            context.Users.Add(fan);

            // Create recipe authored by 'author'
            var recipe = new RecipeDM { Name = "Chocolate Cake", User = author };
            context.Recipes.Add(recipe);

            await context.SaveChangesAsync();

            // Act: fan favorites author's recipe
            fan.FavoriteRecipes.Add(recipe);
            context.Users.Update(fan);
            await context.SaveChangesAsync();

            // Assert: reload fan with favorites
            var savedFan = await context.Users
                .Include(u => u.FavoriteRecipes)
                .FirstOrDefaultAsync(u => u.UserName == "chef_fan");

            Assert.IsNotNull(savedFan);
            Assert.That(savedFan.FavoriteRecipes.Count, Is.EqualTo(1));
            Assert.That(savedFan.FavoriteRecipes.Single().Name, Is.EqualTo("Chocolate Cake"));

            // Assert: recipe knows who favorited it
            var savedRecipe = await context.Recipes
                .Include(r => r.FavoritedBy)
                .FirstOrDefaultAsync(r => r.Name == "Chocolate Cake");

            Assert.IsNotNull(savedRecipe);
            Assert.That(savedRecipe.FavoritedBy.Single().UserName, Is.EqualTo("chef_fan"));
        }
    }
}
