using CommunityToolkit.Mvvm.Input;
using RecipeStudio.Domain.Entities;
using RecipeStudio.Repository.Presistence;
using RecipeStudio.UI.ViewModels;
using RecipeStudioUI.Tests.Helpers;
using RecipeStudioUI.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace RecipeStudio.UI.Tests.ViewModels
{
    [TestFixture]
    internal class RecipeQueryTableVTests
    {
        [Test]
        public void Constructor_WhenRepositoryIsNull_ThrowsArgNullEx()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new RecipeQueryTableVM(null!, new MessageServiceMock());
            });

          Assert.That(ex.ParamName, Is.EqualTo("repository"));

        }

        [Test]
        public void Constructor_WhenMsgServiceIsNul_ThrowsArgNullEx()
        {
            var ex = Assert.Throws<ArgumentNullException>(static () =>
            {
                var contex = TestHelper.GetDbContext();
                _ = new RecipeQueryTableVM(new RecipeQueriesRepository(contex),
                    null!);
            });

            Assert.That(ex.ParamName, Is.EqualTo("msgService"));
        }


        [Test]
        public void UserQueryCmd_CanExecute_ReturnsTrue()
        {
            var dbContext = TestHelper.GetDbContext();
            RecipeQueryTableVM sut = new(new RecipeRepository(dbContext),
                 new MessageServiceMock());
            ICommand cmd = sut.UserQueryCmd;
            sut.CurrentSelectedUserIndex = 1;
            Assert.That(cmd, Is.Not.Null);
            bool canEx = cmd.CanExecute(null!);
            Assert.That(canEx,Is.EqualTo(true));

        }

        [Test]
        public async Task UserQueryCmd_Execute_ReturnsRecipeEntry()
        {
            var dbContext = TestHelper.GetDbContext();
            var repo = new RecipeRepository(dbContext);
            RecipeQueryTableVM sut = new(repo,
                 new MessageServiceMock());
            ICommand cmd = sut.UserQueryCmd;
            List<RecipeDM> list = (List<RecipeDM>)await repo.GetAllAsync();
            for(int i = 0; i < list.Count; i ++)
            {
                var user = list[i].User;
                if(sut.Users.Any(x => x == user.UserName) == false)
                {
                    sut.Users.Add(user.UserName);
                }
            }

            sut.CurrentSelectedUserIndex = 1;
            Assert.That(cmd, Is.Not.Null);
            Assert.That(sut.Recipes.Count, Is.EqualTo(0));
            cmd.Execute(null);
            Assert.That(sut.Recipes.Count, Is.EqualTo(1));

        }

        [Test]
        public void CategoryQueryCmd_CanExecute_ReturnsTrue()
        {
            var dbContext = TestHelper.GetDbContext();
            var repo = new RecipeRepository(dbContext);
            RecipeQueryTableVM sut = new(repo,
                 new MessageServiceMock());
            ICommand cmd = sut.CategoryQueryCmd;        
            sut.CurrentSelectedCategoryIndex = 1;
            Assert.That(cmd.CanExecute(null), Is.True);
        }

        [Test]
        public async Task CategoryQueryCmd_Execute_ReturnsRecipeEntry()
        {
            var dbContext = TestHelper.GetDbContext();
            var repo = new RecipeRepository(dbContext);
            RecipeQueryTableVM sut = new(repo,
                 new MessageServiceMock());
            ICommand cmd = sut.CategoryQueryCmd;
            List<RecipeDM> list = (List<RecipeDM>)await repo.GetAllAsync();
            Assert.That(sut.Recipes.Count,Is.EqualTo(0));
            Assert.That(sut.CurrentSelectedCategoryIndex, Is.EqualTo(0));
            for (int i = 0; i < list.Count; i++)
            {
                List<CategoryDM> categories = list[i].Categories.ToList();
                for (int j = 0; j < categories.Count; j++)
                {
                    if (sut.Categories.Any(x => x == categories[j].Name) == false)
                    {
                        sut.Categories.Add(categories[j].Name);
                    }
                }
            }
            Assert.That(sut.Categories.Count, Is.GreaterThan(0));
            sut.CurrentSelectedCategoryIndex = 1;
            cmd.Execute(null!);
            Assert.That(sut.Recipes.Count, Is.GreaterThan(0));
        }

        [Test]
        public void IngredientQueryCmd_CanExecute_ReturnsTrue()
        {
            var dbContext = TestHelper.GetDbContext();
            var repo = new RecipeRepository(dbContext);
            RecipeQueryTableVM sut = new(repo,
                 new MessageServiceMock());

            ICommand cmd = sut.IngredientQueryCmd;
            Assert.That(cmd.CanExecute(null), Is.False);
            sut.CurrentSelectedIngredientIndex = 1;
            Assert.That(cmd.CanExecute(null), Is.True);
        }

        [Test]
        public async Task IngredientQueryCmd_Execute_ReturnsRecipeEntry()
        {
            var dbContext = TestHelper.GetDbContext();
            var repo = new RecipeRepository(dbContext);
            RecipeQueryTableVM sut = new(repo,
                 new MessageServiceMock());
            ICommand cmd = sut.IngredientQueryCmd;
            List<RecipeDM> list = (List<RecipeDM>)await repo.GetAllAsync();
            Assert.That(sut.Recipes.Count, Is.EqualTo(0));
            Assert.That(sut.CurrentSelectedIngredientIndex, Is.EqualTo(0));
            for (int i = 0; i < list.Count; i++)
            {
                List<IngredientDM> ingredients = list[i].Ingredients.ToList();
                for (int j = 0; j < ingredients.Count; j++)
                {
                    if (sut.Ingredients.Any(x => x == ingredients[j].Name) == false)
                    {
                        sut.Ingredients.Add(ingredients[j].Name);
                    }
                }
            }
            Assert.That(sut.Ingredients.Count, Is.GreaterThan(0));
            sut.CurrentSelectedIngredientIndex = 1;
            cmd.Execute(null!);
            Assert.That(sut.Recipes.Count, Is.GreaterThan(0));
        }

        [Test]
        public async Task AddToFavoritCmd_CanExecute_ReturnsTrue()
        {
            var dbContext = TestHelper.GetDbContext();
            var repo = new RecipeRepository(dbContext);
            RecipeQueryTableVM sut = new(repo,
                 new MessageServiceMock());
            List<RecipeDM> recipes = (List<RecipeDM>)await repo.GetAllAsync();
            string name = TestContext.CurrentContext.Test.Name;
            sut.CurrentUser = new UserDM()
            {
                UserName = name
            };
            ICommand cmd = sut.IngredientQueryCmd;
           
            Assert.That(sut.CurrentSelectedIngredientIndex, Is.EqualTo(0));
            for (int i = 0; i < recipes.Count; i++)
            {
                List<IngredientDM> ingredients = recipes[i].Ingredients.ToList();
                for (int j = 0; j < ingredients.Count; j++)
                {
                    if (sut.Ingredients.Any(x => x == ingredients[j].Name) == false)
                    {
                        sut.Ingredients.Add(ingredients[j].Name);
                    }
                }
            }
            sut.CurrentSelectedIngredientIndex = 1;
            Assert.That(sut.Ingredients[1], Is.EqualTo("Chicken"));
            cmd.Execute(null);
           
            Assert.That(sut.Recipes.Count, Is.EqualTo(1));
            Assert.That(sut.CurrentSelectedItem, Is.Null);
            sut.CurrentSelectedItem = sut.Recipes[0];
            ICommand addToFavorite = sut.AddToFavoriteCmd;
            bool canExecute = addToFavorite.CanExecute(null);
            Assert.That(canExecute, Is.True);       
        }

        [Test]
        public async Task AddToFavoritCmd_CanExecute_WhenRecipeUserAndCurrentUserAreSame_ReturnsFalse()
        {
            var dbContext = TestHelper.GetDbContext();
            var repo = new RecipeRepository(dbContext);
            RecipeQueryTableVM sut = new(repo,
                 new MessageServiceMock());
            List<RecipeDM> recipes = (List<RecipeDM>)await repo.GetAllAsync();
            string name = TestContext.CurrentContext.Test.Name;
            /// <see cref="DataAccess.Data.RecipesDBContext.OnModelCreating"/>
            sut.CurrentUser = recipes[1].User;
            Assert.That(sut.CurrentUser.FavoriteRecipes.Count,Is.EqualTo(1));
            ICommand cmd = sut.IngredientQueryCmd;

            Assert.That(sut.CurrentSelectedIngredientIndex, Is.EqualTo(0));
            for (int i = 0; i < recipes.Count; i++)
            {
                List<IngredientDM> ingredients = recipes[i].Ingredients.ToList();
                for (int j = 0; j < ingredients.Count; j++)
                {
                    if (sut.Ingredients.Any(x => x == ingredients[j].Name) == false)
                    {
                        sut.Ingredients.Add(ingredients[j].Name);
                    }
                }
            }
            sut.CurrentSelectedIngredientIndex = 1;
            Assert.That(sut.Ingredients[1], Is.EqualTo("Chicken"));
            cmd.Execute(null);

            Assert.That(sut.Recipes.Count, Is.EqualTo(1));
            Assert.That(sut.CurrentSelectedItem, Is.Null);
            sut.CurrentSelectedItem = sut.Recipes[0];
            ICommand addToFavorite = sut.AddToFavoriteCmd;
            bool canExecute = addToFavorite.CanExecute(null);
            Assert.That(canExecute, Is.False);
           

        }

        [Test]
        public async Task AddToFavoritCmd_CanExecute_TryToAddDuplicate_ReturnsFalse()
        {
            var dbContext = TestHelper.GetDbContext();
            var repo = new RecipeRepository(dbContext);
            RecipeQueryTableVM sut = new(repo,
                 new MessageServiceMock());
            List<RecipeDM> recipes = (List<RecipeDM>)await repo.GetAllAsync();
            string name = TestContext.CurrentContext.Test.Name;
            /// <see cref="DataAccess.Data.RecipesDBContext.OnModelCreating"/>
            sut.CurrentUser = recipes[0].User;
            Assert.That(sut.CurrentUser.FavoriteRecipes.Count, Is.EqualTo(1));
            ICommand cmd = sut.IngredientQueryCmd;

            Assert.That(sut.CurrentSelectedIngredientIndex, Is.EqualTo(0));
            for (int i = 0; i < recipes.Count; i++)
            {
                List<IngredientDM> ingredients = recipes[i].Ingredients.ToList();
                for (int j = 0; j < ingredients.Count; j++)
                {
                    if (sut.Ingredients.Any(x => x == ingredients[j].Name) == false)
                    {
                        sut.Ingredients.Add(ingredients[j].Name);
                    }
                }
            }
            sut.CurrentSelectedIngredientIndex = 1;
            Assert.That(sut.Ingredients[1], Is.EqualTo("Chicken"));
            cmd.Execute(null);

            Assert.That(sut.Recipes.Count, Is.EqualTo(1));
            Assert.That(sut.CurrentSelectedItem, Is.Null);
            sut.CurrentSelectedItem = sut.Recipes[0];
            ICommand addToFavorite = sut.AddToFavoriteCmd;
            bool canExecute = addToFavorite.CanExecute(null);
            Assert.That(canExecute, Is.False);

        }

        [Test]
        public async Task AddToFavoritCmd_Execute()
        {
            var dbContext = TestHelper.GetDbContext();
            var repo = new RecipeRepository(dbContext);
            RecipeQueryTableVM sut = new(repo,
                 new MessageServiceMock());
            List<RecipeDM> recipes = (List<RecipeDM>)await repo.GetAllAsync();
            string name = TestContext.CurrentContext.Test.Name;
            /// <see cref="DataAccess.Data.RecipesDBContext.OnModelCreating"/>
            sut.CurrentUser = recipes[0].User;
            Assert.That(sut.CurrentUser.FavoriteRecipes.Count, Is.EqualTo(1));
            

            Assert.That(sut.CurrentSelectedIngredientIndex, Is.EqualTo(0));
            for (int i = 0; i < recipes.Count; i++)
            {
                List<IngredientDM> ingredients = recipes[i].Ingredients.ToList();
                for (int j = 0; j < ingredients.Count; j++)
                {
                    if (sut.Ingredients.Any(x => x == ingredients[j].Name) == false)
                    {
                        sut.Ingredients.Add(ingredients[j].Name);
                    }
                }
            }
            sut.CurrentSelectedIngredientIndex = 5;
            ICommand cmd = sut.IngredientQueryCmd;
            cmd.Execute(null);
            Assert.That(sut.Recipes.Count, Is.EqualTo(1));
            Assert.That(sut.CurrentSelectedItem, Is.Null);
            sut.CurrentSelectedItem = sut.Recipes[0];
            ICommand addToFavorite = sut.AddToFavoriteCmd;
            bool canExecute = addToFavorite.CanExecute(null);
            Assert.That(canExecute, Is.True);
            addToFavorite.Execute(null);
            Assert.That(sut.CurrentUser.FavoriteRecipes.Count, Is.EqualTo(2));
        }
    }
}
