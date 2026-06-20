using Microsoft.EntityFrameworkCore;
using RecipeStudio.DataAccess.Data;
using RecipeStudio.Domain.Entities;
using RecipeStudio.Repository.Presistence;
using RecipeStudio.UI.Tests.Mocks;
using RecipeStudio.UI.ViewModels;
using RecipeStudioUI.Tests.Helpers;
using RecipeStudioUI.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeStudio.UI.Tests.ViewModels
{
    [TestFixture]
    internal class TableCollectionVMTests
    {
        [Test]
        public void Constructor_WhenCategoryVMIsNull_ThrowsArgNullEx()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                      {
                          var recipeTableVm = new RecipeTableVM(
                                                    new FakeRecipeRepository(),
                                                    new MessageServiceMock());
                          var stepTableVM = new StepTableVM(
                                                    new FakeStepRepository(),
                                                    new MessageServiceMock());

                          var ingredientTableVM = new IngredientTableVM(
                                                    new FakeIngredientRepository(),
                                                    new MessageServiceMock());

                          var recipeQueryTable = new RecipeQueryTableVM(
                                                        new RecipeRepositoryMock(),
                                                        new MessageServiceMock());

                          _ = new TableCollectionVM(null!, recipeTableVm, stepTableVM,
                                    ingredientTableVM, recipeQueryTable,
                                    new MessageServiceMock());
                      });
            Assert.That(ex.ParamName, Is.EqualTo("categoryVM"));
        }

        [Test]
        public void Constructor_WhenRecipeTableVMIsNull_ThrowsArgNullEx()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var dbContext = TestHelper.GetDbContext();

                var categoryTableVM = new CategoryTableVM(
                    new CategoryRepository(dbContext),
                    new MessageServiceMock()
                    );
                var recipeTableVm = new RecipeTableVM(
                                                   new FakeRecipeRepository(),
                                                   new MessageServiceMock());
                var stepTableVM = new StepTableVM(
                                          new FakeStepRepository(),
                                          new MessageServiceMock());

                var ingredientTableVM = new IngredientTableVM(
                                          new FakeIngredientRepository(),
                                          new MessageServiceMock());

                var recipeQueryTable = new RecipeQueryTableVM(
                                              new RecipeRepositoryMock(),
                                              new MessageServiceMock());

                _ = new TableCollectionVM(categoryTableVM, null!, stepTableVM,
                          ingredientTableVM, recipeQueryTable,
                          new MessageServiceMock());
            });
            Assert.That(ex.ParamName, Is.EqualTo("recipeVM"));
        }

        [Test]
        public void Constructor_WhenStepTableVMIsNull_ThrowsArgNullEx()
        {

            var ex = Assert.Throws<ArgumentNullException>(() =>
                 {
                     var dbContext = TestHelper.GetDbContext();

                     var categoryTableVM = new CategoryTableVM(
                         new CategoryRepository(dbContext),
                         new MessageServiceMock()
                         );
                     var recipeTableVm = new RecipeTableVM(
                                                        new FakeRecipeRepository(),
                                                        new MessageServiceMock());

                     var ingredientTableVM = new IngredientTableVM(
                                               new FakeIngredientRepository(),
                                               new MessageServiceMock());

                     var recipeQueryTable = new RecipeQueryTableVM(
                                                   new RecipeRepositoryMock(),
                                                   new MessageServiceMock());


                     _ = new TableCollectionVM(categoryTableVM, recipeTableVm,
                                                 null!, ingredientTableVM,
                                                             recipeQueryTable,
                                                       new MessageServiceMock());
                 });

            Assert.That(ex.ParamName, Is.EqualTo("stepVM"));
        }

        [Test]
        public void Constructor_WhenIngredientTableVMIsNull_ThrowsArgNullEx()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var dbContext = TestHelper.GetDbContext();

                var categoryTableVM = new CategoryTableVM(
                    new CategoryRepository(dbContext),
                    new MessageServiceMock()
                    );
                var recipeTableVm = new RecipeTableVM(
                                                   new FakeRecipeRepository(),
                                                   new MessageServiceMock());
                var stepTableVM = new StepTableVM(
                                          new FakeStepRepository(),
                                          new MessageServiceMock());

                var recipeQueryTable = new RecipeQueryTableVM(
                                              new RecipeRepositoryMock(),
                                              new MessageServiceMock());

                _ = new TableCollectionVM(categoryTableVM, recipeTableVm,
                                            stepTableVM, null!,
                                                        recipeQueryTable,
                                                  new MessageServiceMock());
            });
            Assert.That(ex.ParamName, Is.EqualTo("ingredientVM"));
        }

        [Test]
        public void Constructor_WhenQueryTableVMIsNull_ThrowsArgNullEx()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var dbContext = TestHelper.GetDbContext();

                var categoryTableVM = new CategoryTableVM(
                    new CategoryRepository(dbContext),
                    new MessageServiceMock()
                    );
                var recipeTableVm = new RecipeTableVM(
                                                   new FakeRecipeRepository(),
                                                   new MessageServiceMock());
                var stepTableVM = new StepTableVM(
                                          new FakeStepRepository(),
                                          new MessageServiceMock());

                var ingredientTableVM = new IngredientTableVM(
                                          new FakeIngredientRepository(),
                                          new MessageServiceMock());
                _ = new TableCollectionVM(categoryTableVM, recipeTableVm, 
                                            stepTableVM, ingredientTableVM,
                                              null!,
                                                  new MessageServiceMock());
            });
            Assert.That(ex.ParamName, Is.EqualTo("queryVM"));
        }


        [Test]
        public void Constructor_WhenMsgServiceIsNull_ThrowsArgNullEx()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var dbContext = TestHelper.GetDbContext();

                var categoryTableVM = new CategoryTableVM(
                    new CategoryRepository(dbContext),
                    new MessageServiceMock()
                    );
                var recipeTableVm = new RecipeTableVM(
                                                   new FakeRecipeRepository(),
                                                   new MessageServiceMock());
                
                var stepTableVM = new StepTableVM(
                                          new FakeStepRepository(),
                                          new MessageServiceMock());
                                
                
                var ingredientTableVM = new IngredientTableVM(
                                          new FakeIngredientRepository(),
                                          new MessageServiceMock());

                var recipeQueryTable = new RecipeQueryTableVM(
                                             new RecipeRepositoryMock(),
                                             new MessageServiceMock());

                _ = new TableCollectionVM(categoryTableVM, recipeTableVm,
                                            stepTableVM, ingredientTableVM,
                                              recipeQueryTable,
                                                  null!);
            });
            Assert.That(ex.ParamName, Is.EqualTo("msgService"));
        }
    }
}
