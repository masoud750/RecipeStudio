using Microsoft.EntityFrameworkCore;
using NuGet.Frameworks;
using RecipeStudio.DataAccess.Data;
using RecipeStudio.Domain.Entities;
using RecipeStudio.Repository.Presistence;
using RecipeStudio.UI.Tests.Mocks;
using RecipeStudio.UI.ViewModels;
using RecipeStudioUI.Tests.Helpers;
using RecipeStudioUI.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RecipeStudio.UI.Tests.ViewModels
{
    [TestFixture]
    internal class StepTableVMTests
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
        public void Constructor_WhenRepositoryIsNull_ThrowsArgNullEx()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new StepTableVM(null!, new MessageServiceMock());
            });
            Assert.That(ex.ParamName, Is.EqualTo("repository"));
        }

        [Test]
        public void Constructor_WhenMsgServiceIsNull_ThrowsArgNullEx()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new StepTableVM(new InstructionRepositoryMock(), 
                                                null!);
            });
            Assert.That(ex.ParamName, Is.EqualTo("msgService"));
        }


        [Test]
        public async Task AddAsync_WhenInstructionIsNull_ThrowsArgNullEx()
        {
            var repo = new InstructionRepository(_dbContext);
            bool isTriggered = false;
            var sut = new StepTableVM(repo, new MessageServiceMock());
            try
            {
                await sut.AddAsync(null!);
            }
            catch (ArgumentNullException ex)
            {
                Assert.That(ex.ParamName, Is.EqualTo("step"));
                isTriggered = true;
            }

            Assert.That(isTriggered, Is.True);
        }

        [Test]
        public async Task  AddAsync_PassingAnInstruction()
        {
            string name = TestContext.CurrentContext.Test.Name;
            StepDM stepDM = new StepDM()
            {
                Name = name
            };
            
            var repo = new InstructionRepository(_dbContext);
            var sut= new StepTableVM(repo,new MessageServiceMock());
            
            var instructions = await repo.GetAllAsync();
            Assert.IsFalse(instructions.Any(x => x.Name == name));
            int count = instructions.Count();
            await sut.AddAsync(stepDM);       
            instructions = await repo.GetAllAsync();
            int newCount = instructions.Count();
            Assert.That(count+1,Is.EqualTo(newCount));
            Assert.IsTrue(instructions.Any(x => x.Name == name));
            
        }


        [Test]
        public async Task UpdateAsync_WhenInstructionIsNull_ThrowsArgNullEx()
        {
            var repo = new InstructionRepository(_dbContext);
            bool isTriggered = false;
            var sut = new StepTableVM(repo, new MessageServiceMock());
            try
            {
                await sut.UpdateAsync(null!);
            }
            catch (ArgumentNullException ex)
            {
                Assert.That(ex.ParamName, Is.EqualTo("step"));
                isTriggered = true;
            }

            Assert.That(isTriggered, Is.True);
        }


        [Test]
        public async Task UpdateAsync_UpdateName()
        {
            var repo = new InstructionRepository(_dbContext);
            var sut = new StepTableVM(repo, new MessageServiceMock());
            var untracked = await _dbContext.Steps
                                    .AsNoTracking()
                                    .FirstAsync(static x => x.Id == 4);

            Assert.IsNotNull(untracked);
            string name = untracked.Name;

            untracked.Name = TestContext.CurrentContext.Test.Name;
            await sut.UpdateAsync(untracked);
            StepDM? instruction = await repo.GetByIdAsync(4);
            Assert.IsNotNull(instruction);
            Assert.That(instruction.Name, !Is.EqualTo(name));
            Assert.That(instruction.Name,
                Is.EqualTo(TestContext.CurrentContext.Test.Name));

        }

    }

    
}
