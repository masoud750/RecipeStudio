using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;
using RecipeStudio.DataAccess.Data;
using RecipeStudio.Domain.Entities;
using RecipeStudio.UI.Helpers;

using RecipeStudio.UI.Services;
using RecipeStudio.UI.UserCtrls;
using RecipeStudio.UI.ViewModels;
using RecipeStudio.Repository.Interfaces;
using RecipeStudio.Repository.Presistence;



namespace RecipeStudio.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

  
    public partial class App : Application
    {


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();


            var dbFolder = Path.Combine(Environment.GetFolderPath(
                                            Environment.SpecialFolder.ApplicationData), 
                                                                "RecipeStudioDemo");
            if (!Directory.Exists(dbFolder)) 
            { 
                Directory.CreateDirectory(dbFolder); 
            }
            
            var dbPath = Path.Combine(dbFolder, "RecipesDB.db");


            // 1. DbContext registrieren
            services.AddDbContext<RecipesDBContext>(options => { 
                                options.UseSqlite($"Data Source={dbPath}"); });

            // 2. Repositories
            services.AddScoped<IRepository<UserDM>, UserRepository>();
            services.AddScoped<IRepository<RecipeDM>, RecipeRepository>();
            services.AddScoped<IRepository<CategoryDM>, CategoryRepository>();
            services.AddScoped<IRepository<StepDM>, InstructionRepository>();
            services.AddScoped<IRepository<IngredientDM>, IngredientRepository>();

            // 3. DataService
            services.AddScoped<IDataService, DataService>();

            // 4. MessageService
            services.AddSingleton<IMessageService, MessageBoxService>();

            // 5. ViewModels
            services.AddSingleton<MainVM>();
            services.AddTransient<CategoryTableVM>();
            services.AddTransient<RecipeTableVM>();
            services.AddTransient<StepTableVM>();
            services.AddTransient<IngredientTableVM>();
            services.AddTransient<RecipeQueryTableVM>();
            services.AddTransient<TableCollectionVM>();
            services.AddTransient<RegisterVM>();

            // 6. Views
            services.AddSingleton<MainWindow>();

            // otheres
            services.AddTransient<LogInVM>();
            services.AddTransient<TableController>();


            // 7. Provider bauen
            Services = services.BuildServiceProvider();

            // 8. Migration ausführen
            using (var scope = Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<RecipesDBContext>();
                db.Database.Migrate();
            }

            // 9. MainWindow starten
            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        public IServiceProvider? Services { get; private set; }
    }
}
