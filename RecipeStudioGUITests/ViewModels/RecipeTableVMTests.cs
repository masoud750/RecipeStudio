using RecipeStudio.Repository.Presistence;
using RecipeStudio.UI.Helpers;
using RecipeStudio.UI.ViewModels;
using RecipeStudioUI.Tests.Helpers;


namespace RecipesUI.Tests.ViewModelsTests
{
    [TestFixture]
    internal class RecipeTableVMTests
    {

        [Test]
        public void CreateInstance_WithInvalidRepository_ThrowArgumentNullEx()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new RecipeTableVM(repository: null!, new MessageBoxService());
            });
            string msg = ex.Message;
            msg = msg.ToLowerInvariant();
            Assert.That(msg.Contains("repository"));

        }
        [Test]
        public void CreateInstance_WithInvalidMsgService()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                using var context = TestHelper.GetDbContext();
                _ = new RecipeTableVM(new RecipeRepository(context), null!);
            });
            var msg = ex.Message;
            msg = msg.ToLowerInvariant();
            Assert.That(msg.Contains("service"));
        }
    }
}
