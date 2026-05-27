

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    namespace RecipesDataAccess.Data
    {
        public class RecipesDBContextFactory : IDesignTimeDbContextFactory<RecipesDBContext>
        {
            public RecipesDBContext CreateDbContext(string[] args)
            {

            // SQLite (local dev)

            //var optionsBuilder = new DbContextOptionsBuilder<RecipesDBContex>();
            //optionsBuilder.UseSqlite("Data Source=RecipesDB.db"); /relative

            var optionsBuilder = new DbContextOptionsBuilder<RecipesDBContext>();
            optionsBuilder.UseSqlite("Data Source = C:\\dev\\RecipesManager.EFCore\\RecipesDB.db");


            // SQL Server (production)
            // optionsBuilder.UseSqlServer("Server=.;Database=RecipesDB;Trusted_Connection=True;");

            // PostgreSQL (alternative)
            // optionsBuilder.UseNpgsql("Host=localhost;Database=RecipesDB;Username=postgres;Password=secret");


            return new RecipesDBContext(optionsBuilder.Options);
            
            }
        }
    }

