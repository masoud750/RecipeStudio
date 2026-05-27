using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesDataAccessIntegrationTests.Tables
{
    [TestFixture]
    internal class UsersTests
    {

        [Test]
        public async Task UserModel_KannInsertUser_SavesSuccessfully()
        {
            // Arrange
            using var context = TestHelper.GetDbContext();

            var user = new UserDM { UserName = "chef1", Email = "chef1@test.com", PasswordHash = "7##65" };


            // Act
            context.Users.Add(user);

            await context.SaveChangesAsync();

            // Assert
            var savedUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "chef1");
            Assert.IsNotNull(savedUser);
            Assert.That(savedUser, Is.EqualTo(user));
            Assert.That(savedUser.ValidFrom, Is.Not.EqualTo(DateTime.MinValue)); // not default 0001-01-01
            Assert.That(savedUser.ValidFrom.Date, Is.EqualTo(DateTime.Today));   // matches today's date
            Assert.That(savedUser.ValidTo, Is.EqualTo(new DateTime(2090, 1, 1, 0, 0, 0)));
        }

        [Test]
        public void UserModel_Names_AreEqualIgnoringCase()
        {
            // Arrange
            var user1 = new UserDM { UserName = "ChefMaster", Email = "chef@test.com", PasswordHash = "abc123" };
            var user2 = new UserDM { UserName = "chefmaster", Email = "chef@test.com", PasswordHash = "abc123" };

            // Act & Assert: case-insensitive comparison
            Assert.That(user1.UserName, Is.EqualTo(user2.UserName).IgnoreCase);
        }

        [Test]
        public void UserModel_Emails_AreEqualIgnoringCase()
        {
            // Arrange
            var user1 = new UserDM { UserName = "chef", Email = "Chef@Test.com", PasswordHash = "abc123" };
            var user2 = new UserDM { UserName = "chef", Email = "chef@test.com", PasswordHash = "abc123" };

            // Act & Assert: case-insensitive comparison
            Assert.That(user1.Email, Is.EqualTo(user2.Email).IgnoreCase);
        }
    }
}
