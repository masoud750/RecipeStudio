
using NUnit.Framework.Internal;
using RecipeStudio.Repository.Presistence;
using RecipeStudio.UI.Helpers;
using RecipeStudio.UI.ViewModels;
using RecipeStudioUI.Tests.Helpers;
using RecipeStudioUI.Tests.Mocks;

namespace RecipesUI.Tests.ViewModelsTests
{
    [TestFixture]
    internal class RegisterVMTests
    {


        [Test]
        public void Constructor_PassingInvalidRepository_ArgNullEx()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () =>
                {
                    _ = new RegisterVM(null!, new MessageBoxService());
                });
       
            Assert.That(ex.ParamName, Is.EqualTo("repository"));

        }

        [Test]
        public void Constructor_PassingInvalidMsgService_ArgNullEx()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () =>
                {
                    _ = new RegisterVM(new UserRepositoryMock(), null!);
                });

            Assert.That(ex.ParamName, Is.EqualTo("msgService"));

        }

        [Test]
        public void RegisterCmd_CanExecute_EmptyEmail_Fails()
        {
            var vm = new RegisterVM(new
                UserRepository(TestHelper.GetDbContext()),
                new MessageBoxService());
            vm.Username = "test";
            vm.Email = string.Empty;
            var cmd = vm.RegisterCmd;
            Assert.IsFalse(cmd.CanExecute(vm));

        }


        [Test]
        public void RegisterCmd_CanExecute_EmptyUser_Fails()
        {
            var vm = new RegisterVM(new
                UserRepository(TestHelper.GetDbContext()),
                new MessageBoxService());
            vm.Username = string.Empty;
            vm.Email = "test@yahoo.com";
            var sut = vm.RegisterCmd;
            Assert.IsFalse(sut.CanExecute(vm));

        }
        
        [Test]
       
        public async Task RegisterCmd_Execute_Success()
        {
            var context = TestHelper.GetDbContext();
        
            var usrRepMock = new UserRepositoryMock();
            var name = TestContext.CurrentContext.Test.Name;
            var vm = new RegisterVM(usrRepMock,     
            new MessageServiceMock())
            {
                Username = "Ali",
                Email = name +"@yahoo.com",
                Password = "123",
            };
            var cmd = vm.RegisterCmd;
            
            Assert.IsTrue(cmd.CanExecute(vm));                     
            cmd.Execute(vm);
            await Task.Delay(200);
            Assert.IsTrue(usrRepMock.RegisteredUsers.Count == 1);
            var user = usrRepMock.RegisteredUsers[0];
            Assert.IsNotNull(user);
            Assert.IsTrue(user.Email == name.ToLowerInvariant()+ "@yahoo.com");
            Assert.That(user.UserName,Is.EqualTo("ali"));
        }

    }

   
}
