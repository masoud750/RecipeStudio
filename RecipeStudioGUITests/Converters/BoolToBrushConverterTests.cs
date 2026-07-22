using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using RecipeStudio.DataAccess.Data;
using RecipeStudio.Domain.Entities;
using RecipeStudio.Repository.Presistence;
using RecipeStudio.UI.Converters;

using RecipeStudioUI.Tests.Helpers;


namespace RecipeStudio.UI.Tests.Converters
{
    internal class BoolToBrushConverterTests
    {

        [Test]
        public async Task  Convertor_GetRedBrush_Failed()
        {
            // Arrange
            using var dbContext = TestHelper.GetDbContext();
            var repo = new IngredientRepository(dbContext);
            var ingredients = await repo.GetAllAsync();
            Assert.IsNotNull(ingredients);
            Assert.That(ingredients.Count(), expression: Is.GreaterThan(0));
            var cInfo = new CultureInfo("en-US");
            IValueConverter con = new BoolToBrushConverter();
            IngredientDM? ingredient = ingredients.ToList()[0];
            var brush = con.Convert(!ingredient.IsExpired, 
                        ingredient.GetType(), null, cInfo);
            Assert.IsNotNull(brush);
            Assert.That(brush, Is.EqualTo(Brushes.Red));
        }

        [Test]
        public async Task Convert_GetBlackBrush_Success()
        {
            // Arrange
            using RecipesDBContext dbContext = TestHelper.GetDbContext();
            var repo = new IngredientRepository(dbContext);
            var ingredients = await repo.GetAllAsync();
            Assert.IsNotNull(ingredients);
            Assert.That(ingredients.Count(), expression: Is.GreaterThan(0));
            var cInfo = new CultureInfo("en-US");
            IValueConverter con = new BoolToBrushConverter();
            IngredientDM? ingredient = ingredients.ToList()[0];
            var brush = con.Convert(ingredient.IsExpired,
                        ingredient.GetType(), null, cInfo);
            Assert.IsNotNull(brush);
            Assert.That(brush, Is.EqualTo(Brushes.Black));
        }

    }
}
