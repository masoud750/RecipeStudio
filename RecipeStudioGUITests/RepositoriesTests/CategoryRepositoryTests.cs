using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using RecipeStudioUI.Repositories;


namespace RecipeStudioUI.Tests.RepositoriesTests
{
    internal class CategoryRepositoryTests
    {

        [Test]
        public void Constructor_PassInvalidContext_AgrNullEx()
        {
            var ex = Assert.ThrowsAsync<ArgumentNullException>(static async () =>
            {
                _ = new CategoryRepository(null!);
            });

            Assert.That(ex.ParamName,Is.EqualTo("context"));
        }

        [Test]
        public async Task GetByNameAsync_ReturnsCategoryWhenExists_Success()
        {   // Arrange
            using var dbContext = TestHelper.GetDbContext();

            string name = "Dessert";
            dbContext.Categories.Add(new CategoryDM { Name = name });
            await dbContext.SaveChangesAsync();
            var repo = new CategoryRepository(dbContext);
            // Act
            var result = await repo.GetByNameAsync(name);
            // Assert
            Assert.NotNull(result);

            Assert.IsTrue(result.Name == name);
        }


        [Test]

        public async Task UpdateAsync_ChangeName_Success()
        {
            var dbContext = TestHelper.GetDbContext();
            var sut = new CategoryRepository(dbContext);
            CategoryDM? untracked = await dbContext.Categories
                                        .AsNoTracking()
                                         .FirstAsync(static x => x.Id == 4);
            
            Assert.Multiple(() =>
            {
                Assert.That(untracked, Is.Not.Null);
                Assert.That(untracked.Id, Is.EqualTo(4));
                Assert.That(untracked.IsExpired, Is.False);
            });


            untracked.Name = TestContext.CurrentContext.Test.Name;

            await sut.UpdateAsync(untracked);
            CategoryDM? tracked = await sut.GetByIdAsync(4);
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(tracked);
                Assert.That(tracked!.Id, Is.EqualTo(4));
                Assert.That(tracked.Name,
                    Is.EqualTo(TestContext.CurrentContext.Test.Name));

            });
        }

        [Test]
        public async Task AddAsync_PassEntity_Success()
        {
            string name = TestContext.CurrentContext.Test.Name;
            CategoryDM category = new CategoryDM()
            {
                Name = name
            };

            var context = TestHelper.GetDbContext();
            var sut = new CategoryRepository(context);
            IEnumerable<CategoryDM?> categories = await sut.GetAllAsync();
            int count = categories.Count();
            Assert.That(categories.Any( x=>x.Name == name), Is.False);
            await sut.AddAsync(category);
            categories = await sut.GetAllAsync();
            Assert.Multiple(() =>
            {
                Assert.That(categories.Count, Is.EqualTo(count + 1));
                Assert.That(categories.Any(x => x.Name == name), Is.True);
            });
        }
    }
}
