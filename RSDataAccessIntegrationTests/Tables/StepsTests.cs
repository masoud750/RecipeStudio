using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RecipeStudio.Domain.Entities;

namespace RecipesDataAccessIntegrationTests.Tables
{
    [TestFixture]
    internal class StepsTests
    {
        [Test]
        public void StepsTable_AddStep_SavesSuccessfully()
        {
            using var context = TestHelper.GetDbContext();

            // Arrange: create a new step
            var step = new StepDM { Name = "Mix Flour" };

            // Act: add and save
            context.Steps.Add(step);
            context.SaveChanges();

            // Assert: step is persisted
            var savedStep = context.Steps.FirstOrDefault(s => s.Name == "Mix Flour");
            Assert.IsNotNull(savedStep);
            Assert.That(savedStep.Name, Is.EqualTo("Mix Flour"));
        }


        [Test]
        public void SetpAddingDuplicateStepName_ShouldThrowException()
        {
            using var context = TestHelper.GetDbContext();

            var step1 = new StepDM { Name = "Mix Flour" };
            context.Steps.Add(step1);
            context.SaveChanges();

            var step2 = new StepDM { Name = "Mix Flour" };
            bool isThrown = false;
         try
         {
               

                context.Steps.Add(step2);
                context.SaveChanges();
            }
             catch (DbUpdateException ex)
            {
                var sqliteEx = ex.InnerException as SqliteException;
                Assert.That(sqliteEx?.SqliteErrorCode, Is.EqualTo(19));
                isThrown = true;
            }
     
            int i = context.Steps.Count();
            // Assert
            Assert.That(isThrown, Is.True);

        }


        

        
    }
}
