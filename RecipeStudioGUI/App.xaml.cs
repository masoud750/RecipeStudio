using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipesDataAccess.Data;
using RecipeStudioUI.Helpers;
using RecipeStudioUI.Repositories;
using RecipeStudioUI.Services;
using RecipeStudioUI.UserCtrls;
using RecipeStudioUI.ViewModels;
using System.IO;
using System.Windows;

namespace RecipesGUI
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
            services.AddTransient<TableColumnsVM>();
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
