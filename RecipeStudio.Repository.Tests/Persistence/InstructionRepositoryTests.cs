using Microsoft.EntityFrameworkCore;
using RecipeStudio.DataAccess.Data;
using RecipeStudio.Domain.Entities;
using RecipeStudio.Repository;
using RecipeStudio.Repository.Presistence;
using RecipeStudio.Repository.Tests.Helpers;



namespace RecipeStudio.UI.Tests.RepositoriesTests
{
    internal class InstructionRepositoryTests
    {
        [SetUp]
        public void SetUp() {
            _dbContext = TestHelper.GetDbContext();
        }

        RecipesDBContext _dbContext;

        [TearDown]
        public void TearDown() {
            _dbContext.Dispose();
        }

        [Test]
        public void Constructor_PassingInvalidContext_ArgNullEx()
        {
            var ex = Assert.Throws<ArgumentNullException>(static () =>
            {
                _ = new InstructionRepository(null!);
            }
            );
            Assert.That(ex.ParamName, Is.EqualTo("context"));
        }

        [Test]

        public async Task GetAllAsync_ItemsExist_Success()
        {
            var sut = new InstructionRepository(_dbContext);
            IEnumerable<StepDM> result = await sut.GetAllAsync();
            Assert.Multiple(() => {
                Assert.That(result,Is.Not.Null);
                Assert.That(result, Is.Not.Empty);
                Assert.That(result.Count, Is.EqualTo(10));
                Assert.That(result.Any( x => x.IsExpired ), Is.False);
            });
          
        }

        [Test]

        public async Task GetByIdAsync_PassingValidId_Success()
        {
            var sut = new InstructionRepository(_dbContext);
            StepDM? result = await sut.GetByIdAsync(1);
             Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);
                Assert.That(result!.Id, Is.EqualTo(1));
                Assert.That(result.IsExpired, Is.False);
            });
            
        }

       
        [Test]
        public async Task GetByIdAsync_PassingInvalidId_NotSupportedException()
        {

            var sut = new InstructionRepository(_dbContext);
            var result = await sut.GetByIdAsync(0);
            Assert.IsNull(result);

        }

        [Test]

        public async Task AddAsync_AddInvalidEntity_ArgNullEx()
        {
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var sut = new InstructionRepository(_dbContext);
                await sut.AddAsync(null!);
            });

            Assert.That(ex.ParamName, Is.EqualTo("entity"));
        }


        [Test]
     
    
        public async Task AddAsync_AddingValidEntity_Success()
        {
            var sut = new InstructionRepository(_dbContext);
            List<StepDM> instructions = (List<StepDM>)await sut.GetAllAsync();            
            int count = instructions.Count();
            Assert.IsTrue(count > 0);
            string name = TestContext.CurrentContext.Test.Name;
            var newInstruction = new StepDM() {
                Name = name,            
            };
            
            
            await sut.AddAsync(newInstruction);
            var items = await sut.GetAllAsync();
            Assert.Multiple(() =>
                    {
                        Assert.That(count + 1 == items.Count());
                        Assert.That(items.Any( x => x.Name == name),Is.True);

                    });   
        }


        [Test]
        public async Task AddAsync_PassingNull_ThrowsArgumentNullException()
        {
            var sut = new InstructionRepository(_dbContext);

            var ex = Assert.ThrowsAsync<ArgumentNullException>(
               async () => await sut.AddAsync(null!)
            );

            Assert.That(ex.ParamName, Is.EqualTo("entity"));
        }

        [Test]
      

        public async Task UpdateAsync_ChangeName_Success()
        {

            var sut = new InstructionRepository(_dbContext);
            StepDM? untracked = await _dbContext.Steps
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
            StepDM? tracked = await sut.GetByIdAsync(4);
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(tracked);
                Assert.That(tracked!.Id, Is.EqualTo(4));
                Assert.That(tracked.Name, 
                    Is.EqualTo(TestContext.CurrentContext.Test.Name));

            });         

        }

        [Test]
       

        public async Task UpdateAsync_PassingInvalidEntity_Success()
        {
            var sut = new InstructionRepository(_dbContext);
      
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async ()  =>
            {
                await sut.UpdateAsync(null!);
            });

            Assert.That(ex.ParamName, Is.EqualTo("entity"));
        }

        [Test]
     
        public async Task DeleteAsync_PassValidId_Success()
        {
            var sut = new InstructionRepository(_dbContext);
            StepDM? result = await sut.GetByIdAsync(1);
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);
                Assert.That(result!.Id, Is.EqualTo(1));
                Assert.That(result.IsExpired, Is.False);
            });
            string name = result.Name;
            await sut.DeleteAsync(1);
          
            IEnumerable<StepDM> instructions  = await sut.GetAllAsync();
            Assert.Multiple(async () =>
            {
                Assert.That(instructions, Is.Not.Null);
                Assert.That(instructions.Count, Is.GreaterThan(0));
                Assert.That(instructions.Any( x=> x.Name == name), Is.False);
            });
        }


    }
}

