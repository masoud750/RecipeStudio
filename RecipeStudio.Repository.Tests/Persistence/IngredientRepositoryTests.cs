using Microsoft.EntityFrameworkCore;
using RecipeStudio.DataAccess.Data;
using RecipeStudio.Domain.Entities;
using RecipeStudio.Repository.Presistence;
using RecipeStudio.Repository.Tests.Helpers;

namespace RecipeStudio.UI.Tests.RepositoriesTests
{
    internal class IngredientRepositoryTests
    {

        [SetUp]
        public void SetUp()
        {
            _dbContext = TestHelper.GetDbContext();
        }

        RecipesDBContext _dbContext;

        [TearDown]
        public void Teardown()
        {
            _dbContext.Dispose();
        }


        [Test]
        public void Constructor_PassInvalidContext_ArgNullEx()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new IngredientRepository(null!);
            });
            Assert.That(ex.ParamName, Is.EqualTo("context"));
        }


        [Test]
        public async Task AddAsync_passEntity_Success()
        {
            var sut = new IngredientRepository(_dbContext);
            string testName = TestContext.CurrentContext.Test.Name;
            IngredientDM entity = new IngredientDM()
            {
                Name = testName,
                Quantity= "2 liter"
            };
            List<IngredientDM?> results = 
                (List<IngredientDM?>)await sut.GetAllAsync();
            int count = results.Count;
           await sut.AddAsync(entity);

            results = (List<IngredientDM?>)await sut.GetAllAsync();
            int nCount = results.Count;
            Assert.That(nCount - 1, Is.EqualTo(count));
            IngredientDM? dm = await sut.GetByIdAsync(nCount);
            Assert.Multiple(() =>
            {
                Assert.That(dm, Is.Not.Null);
                Assert.That(dm!.Name, Is.EqualTo(testName));
            });
            
           
        }


        [Test]
        public async Task AddAsync_PassInvalidEntity_ArgNullEx()
        {
            var ex = Assert.ThrowsAsync<ArgumentNullException>(
                static async () =>
            {
                RecipesDBContext dbContext = TestHelper.GetDbContext();
                var sut = new IngredientRepository(dbContext);
                await sut.AddAsync(null!);
            });

            Assert.That(ex.ParamName, Is.EqualTo("entity"));
        }

        [Test]

        public async Task UpdateAsync_ChangeName_Success()
        {

            IngredientRepository sut = new IngredientRepository(_dbContext);
            IngredientDM? untracked = await _dbContext.Ingredients
                                        .AsNoTracking()
                                         .FirstAsync(static x => x.Id == 4);
            ;
            Assert.Multiple(() =>
            {
                Assert.That(untracked, Is.Not.Null);
                Assert.That(untracked.Id, Is.EqualTo(4));
                Assert.That(untracked.IsExpired, Is.False);
            });


            untracked.Name = TestContext.CurrentContext.Test.Name;

            await sut.UpdateAsync(untracked);
            IngredientDM? tracked = await sut.GetByIdAsync(4);
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(tracked);
                Assert.That(tracked!.Id, Is.EqualTo(4));
                Assert.That(tracked.Name,
                    Is.EqualTo(TestContext.CurrentContext.Test.Name));

            });
        }

        [Test]
        public async Task UpdateAsync_PassInvalidEntity_ArgNullEx()
        {
            ArgumentNullException? ex = 
                Assert.ThrowsAsync<ArgumentNullException>(static async () =>
            {
                RecipesDBContext dbContext = TestHelper.GetDbContext();
                var sut = new IngredientRepository(dbContext);
                await sut.UpdateAsync(null!);
            });

            Assert.That(ex.ParamName, Is.EqualTo("entity"));
        }

        [Test]
        public async Task DeleteAsync_DeleteValidEntry_Success()
        {
            var sut = new IngredientRepository(_dbContext);
            await sut.DeleteAsync(4);
            IEnumerable<IngredientDM> ingredients = await sut.GetAllAsync();

            Assert.That(ingredients.Any(x => x.Id == 4), Is.False);
        }

        [Test]
        public async Task DeleteAsync_DeleteInvalidId_OutAufRang()
        {
            var ex = Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                static async () =>
            {
                var context = TestHelper.GetDbContext();
                var sut = new IngredientRepository(context);
                await sut.DeleteAsync(0);
            });

            Assert.That(ex.ParamName, Is.EqualTo("id"));
        }
    }
}
