using RecipeStudio.Domain.Entities;
using RecipeStudio.UI.ViewModels;
using RecipeStudioUI.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeStudio.UI.Tests.ViewModels
{
    [TestFixture]
    internal class TableVMTests
    {
        [Test]
        public void Constructor_WhenRepositoryIsNull_ThrowsArgNullEx()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _= new TableVM<IngredientDM>(
                    null!,new MessageServiceMock());
            });
            Assert.That(ex.ParamName, Is.EqualTo("repository"));
        }

        [Test]
        public void Constructor_WhenMsgServiceIsNull_ThrowsArgNullEx()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new TableVM<UserDM>(
                    new UserRepositoryMock(),null!);
            });
            Assert.That(ex.ParamName, Is.EqualTo("msg"));
        }
    }
}
