using NUnit.Framework;
using RecipesDataAccess.Models;

namespace RecipesDataAccessIntegrationTests.Models
{
    [TestFixture]
    internal class ReceipeDMTests
    {

        [Test]
        public void RecipeDM_TryCrecateInstanceWithoutStep_ThrowsException()
        {
            bool isThrown = false;
            var user = new UserDM { UserName = "chef1", Email = "chef1@test.com", PasswordHash= "7##65" };
            var category = new CategoryDM { Name = "Dessert" };
            var ingredient = new IngredientDM { Name = "Egg", Quantity="2" };
            try
            {
                _ = new RecipeDM(

                      "Chocolate Cake",
                      user.Id,
                      new List<CategoryDM> { category },
                      null!,
                      new List<IngredientDM> { ingredient }
                      );
            }
            catch (ArgumentNullException ex) {

                string msg = ex.Message;
                var result = msg.Contains("step");
                Assert.IsTrue(result);
                isThrown = true;

            };
            
            Assert.IsTrue(isThrown);
                          
        }

        [Test]
        public void RecipeDM_TryCrecateInstanceWithoutIngredient_ThrowsException()
        {
            bool isThrown = false;
            var user = new UserDM { UserName = "chef1", Email = "chef1@test.com", PasswordHash= "7##65" };
            var category = new CategoryDM { Name = "Dessert" };
        
            var step = new StepDM
            {
          
                Name = "Mix flour and sugar"
            };
            try
            {
                _ = new RecipeDM(

                      "Chocolate Cake",
                      user.Id,
                      new List<CategoryDM> { category },
                      new List<StepDM> { step },
                      null!
                      
                      );
            }
            catch (ArgumentNullException ex)
            {

                string msg = ex.Message;
                var result = msg.Contains("ingredient");
                Assert.IsTrue(result);
                isThrown = true;

            }            

            Assert.IsTrue(isThrown);

        }

        [Test]
        public void RecipeDM_TryCrecateInstanceWithoutCategory_ThrowsException()
        {
            bool isThrown = false;
            var user = new UserDM { UserName = "chef1", Email = "chef1@test.com", PasswordHash= "7##65" };
      
            var ingredient = new IngredientDM { Name = "Egg", Quantity= "2" };
            var step = new StepDM
            {

                Name = "Mix flour and sugar"
            };
            try
            {
                _ = new RecipeDM(

                      "Chocolate Cake",
                      user.Id,
                      null!,
                      new List<StepDM> { step },
                      new List<IngredientDM> { ingredient}

                      );
            }
            catch (ArgumentNullException ex)
            {

                string msg = ex.Message;
                var result = msg.Contains("categories");
                Assert.IsTrue(result);
                isThrown = true;

            }

            Assert.IsTrue(isThrown);

        }

        [Test]
        public void RecipeDM_TryCrecateInstanceWithEmptySteps_ThrowsException()
        {
            bool isThrown = false;
            var user = new UserDM { UserName = "chef1", Email = "chef1@test.com", PasswordHash= "7##65" };
            var category = new CategoryDM { Name = "Dessert" };
            var ingredient = new IngredientDM { Name = "Egg", Quantity = "2" };
            try
            {
                _ = new RecipeDM(

                      "Chocolate Cake",
                      user.Id,
                      new List<CategoryDM> { category },
                      new List<StepDM>(),
                      new List<IngredientDM> { ingredient }
                      );
            }
            catch (ArgumentException ex)
            {

                string msg = ex.Message;
                var result = msg.Contains("one step");
                Assert.IsTrue(result);
                isThrown = true;

            }
            

            Assert.IsTrue(isThrown);

        }

        [Test]
        public void RecipeDM_TryCrecateInstanceWithEmptyIngredient_ThrowsException()
        {
            bool isThrown = false;
            var user = new UserDM { UserName = "chef1", Email = "chef1@test.com", PasswordHash= "7##65" };
            var category = new CategoryDM { Name = "Dessert" };

            var step = new StepDM
            {

                Name = "Mix flour and sugar"
            };
            try
            {
                _ = new RecipeDM(

                      "Chocolate Cake",
                      user.Id,
                      new List<CategoryDM> { category },
                      new List<StepDM> { step },
                      new List<IngredientDM>()

                      );
            }
            catch (ArgumentException ex)
            {

                string msg = ex.Message;
                var result = msg.Contains("one ingredient");
                Assert.IsTrue(result);
                isThrown = true;

            }

            Assert.IsTrue(isThrown);

        }

        [Test]
        public void RecipeDM_TryCrecateInstanceWithEmptyCategory_ThrowsException()
        {
            bool isThrown = false;
            var user = new UserDM { UserName = "chef1", Email = "chef1@test.com", PasswordHash= "7##65" };

            var ingredient = new IngredientDM { Name = "Egg" , Quantity = "2" };
            var step = new StepDM{ Name = "Mix flour and sugar"};
            try
            {
                _ = new RecipeDM(

                      "Chocolate Cake",
                      user.Id,
                      new List<CategoryDM>(),
                      new List<StepDM> { step },
                      new List<IngredientDM> { ingredient }

                      );
            }
            catch (ArgumentException ex)
            {

                string msg = ex.Message;
                var result = msg.Contains("one category");
                Assert.IsTrue(result);
                isThrown = true;

            }

            Assert.IsTrue(isThrown);

        }
    }
}
