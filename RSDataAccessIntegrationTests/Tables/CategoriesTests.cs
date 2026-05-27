using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Domain.Entities;
using RecipesDataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesDataAccessIntegrationTests.Tables
{
    internal class CategoriesTests
    {

        // Category

        [Test]
        public async Task CategoryModel_CanInsertCategory_SavesSuccessfully()
        {
            using var context = TestHelper.GetDbContext();

            // Arrange

            var category = new CategoryDM { Name = "Dessert" };

            context.Categories.Add(category);
            await context.SaveChangesAsync();

            // Act
            var savedRecipe = await context.Categories.FirstOrDefaultAsync(r => r.Name == "Dessert");

            // Assert
            Assert.IsNotNull(savedRecipe);
            Assert.That(savedRecipe.ValidFrom, Is.Not.EqualTo(DateTime.MinValue)); // not default 0001-01-01
            Assert.That(savedRecipe.ValidFrom.Date, Is.EqualTo(DateTime.Today));   // matches today's date
            Assert.That(savedRecipe.ValidTo, Is.EqualTo(new DateTime(2090, 1, 1, 0, 0, 0)));

        }

        [Test]
        public async Task CategoryModel_CanInsertCategoryWithTheSameName_ThrowsException()
        {

            var options = new DbContextOptionsBuilder<RecipesDBContext>()
                .UseSqlite("Filename=:memory:")   // SQLite in-memory database
                    .Options;

            using var context = new RecipesDBContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            // Arrange

            bool isThrown = false;
            var category = new CategoryDM { Name = "Dessert" };

            context.Categories.Add(category);
            await context.SaveChangesAsync();

            // Act
            try
            {
                var sut = new CategoryDM { Name = "Dessert" };
                context.Categories.Add(sut);
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
        public async Task CategoryModel_AddingDuplicateCategoryNamebyFormat_ShouldThrowException()
        {
            var options = new DbContextOptionsBuilder<RecipesDBContext>()
                .UseSqlite("Filename=:memory:")   // SQLite in-memory database
                    .Options;

            using var context = new RecipesDBContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            // Arrange

            bool isThrown = false;
            var category = new CategoryDM { Name = "Dessert" };

            context.Categories.Add(category);
            await context.SaveChangesAsync();

            // Act
            try
            {
                var sut = new CategoryDM { Name = "DesSert" };
                context.Categories.Add(sut);
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
    }
}
