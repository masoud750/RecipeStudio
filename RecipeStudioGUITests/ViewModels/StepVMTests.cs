using RecipeStudio.Domain.Entities;
using RecipeStudio.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeStudio.UI.Tests.ViewModels
{
    [TestFixture]
    internal class StepVMTests
    {
        [Test]
        public void Constructor_WhenDMIsNull_ThrowsArgNullEx()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new StepVM(null!);
            });

            Assert.That(ex.ParamName, Is.EqualTo("dm"));
        }
    }
}
