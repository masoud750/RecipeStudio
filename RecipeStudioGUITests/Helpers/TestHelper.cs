using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RecipesDataAccess.Data;


namespace RecipeStudioUI.Tests.Helpers
{
    internal class TestHelper
    {
        public static RecipesDBContext GetDbContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open(); // keep connection alive for the test

            var options = new DbContextOptionsBuilder<RecipesDBContext>()
                .UseSqlite(connection)
                .Options;

            var context = new RecipesDBContext(options);
            context.Database.EnsureCreated(); // apply schema

            return context;
        }

    }
}
