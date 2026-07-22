
using RecipeStudio.UI.Helpers;
using RecipeStudioUI.Tests.Helpers;
using RecipeStudioUI.Tests.Mocks;
using RecipeStudio.UI.ViewModels;
using RecipeStudio.Repository.Presistence;

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
            var usrRepMock = new UserRepositoryMock();
            var vm = new RegisterVM(usrRepMock,
            new MessageServiceMock());
            var sut = vm.RegisterCmd;
            vm.Username = "Ali";
            vm.Email = "test@yahoo.com";
            vm.Password = "123";
            Assert.IsTrue(sut.CanExecute(vm));
            string pwd = TestContext.CurrentContext.Test.Name;            
            sut.Execute(vm);
            Assert.IsTrue(usrRepMock.RegisteredUsers.Count == 1);
            var user = usrRepMock.RegisteredUsers[0];
            Assert.IsNotNull(user);
            Assert.IsTrue(user.Email == vm.Email);
            Assert.IsTrue(user.UserName == vm.Username);
        }

    }

   
}
