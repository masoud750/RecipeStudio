using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RecipesDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    ValidFrom = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ValidTo = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValue: new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    IngredientId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    ValidFrom = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ValidTo = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValue: new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    Quantity = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.IngredientId);
                });

            migrationBuilder.CreateTable(
                name: "Steps",
                columns: table => new
                {
                    StepId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    ValidFrom = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ValidTo = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValue: new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Steps", x => x.StepId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    ValidFrom = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ValidTo = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ValidTo = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValue: new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.RecipeId);
                    table.ForeignKey(
                        name: "FK_Recipes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryRecipes",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryRecipes", x => new { x.CategoryId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_CategoryRecipes_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryRecipes_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => new { x.UserId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_Favorites_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeIngredients",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false),
                    IngredientId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngredients", x => new { x.RecipeId, x.IngredientId });
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "IngredientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeSteps",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false),
                    StepId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeSteps", x => new { x.RecipeId, x.StepId });
                    table.ForeignKey(
                        name: "FK_RecipeSteps_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeSteps_Steps_StepId",
                        column: x => x.StepId,
                        principalTable: "Steps",
                        principalColumn: "StepId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name", "ValidFrom", "ValidTo" },
                values: new object[,]
                {
                    { 1, "Italian", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(6959), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Indian", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(6969), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Mexican", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(6973), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "French", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(6977), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Japanese", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(6980), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "Chinese", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(6984), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "Mediterranean", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(6988), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "American", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(6992), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, "Thai", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(6996), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, "Spanish", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7000), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "IngredientId", "Name", "Quantity", "ValidFrom", "ValidTo" },
                values: new object[,]
                {
                    { 1, "Spaghetti", "200g", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7052), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Chicken", "500g", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7061), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Tomatoes", "3 pcs", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7066), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Garlic", "2 cloves", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7071), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Olive Oil", "50ml", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7075), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "Rice", "250g", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7080), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "Beef", "400g", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7084), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "Potatoes", "3 pcs", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7089), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, "Cheese", "100g", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7093), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, "Basil", "10 leaves", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7097), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Steps",
                columns: new[] { "StepId", "Name", "ValidFrom", "ValidTo" },
                values: new object[,]
                {
                    { 1, "Boil pasta until al dente.", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7153), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Cook chicken with onions and garlic.", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7160), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Chop vegetables finely.", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7164), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Simmer sauce for 20 minutes.", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7168), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Preheat oven to 180°C.", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7172), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "Grill beef until medium rare.", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7175), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "Steam rice until fluffy.", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7179), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "Mash potatoes with butter.", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7183), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, "Mix salad dressing.", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7188), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, "Serve hot with garnish.", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7192), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "PasswordHash", "UserName", "ValidFrom", "ValidTo" },
                values: new object[,]
                {
                    { 1, "anna@example.com", "hash1", "ChefAnna", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(6401), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "masoud@example.com", "hash2", "ChefSam", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(6464), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "luca@example.com", "hash3", "ChefLuca", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(6481), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "maria@example.com", "hash4", "ChefMaria", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(6487), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "tom@example.com", "hash5", "ChefTom", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(6492), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "sara@example.com", "hash6", "ChefSara", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(6497), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "ali@example.com", "hash7", "ChefAli", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(6502), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "emma@example.com", "hash8", "ChefEmma", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(6507), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, "john@example.com", "hash9", "ChefJohn", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(6511), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, "sophia@example.com", "hash10", "ChefSophia", new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(6516), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "Description", "Name", "UserId", "ValidFrom", "ValidTo" },
                values: new object[,]
                {
                    { 1, "Classic Italian pasta.", "Spaghetti Carbonara", 1, new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7245), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Aromatic curry with chicken.", "Chicken Curry", 2, new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7257), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Mexican street food.", "Beef Tacos", 3, new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7262), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Japanese delicacy.", "Sushi Roll", 4, new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7267), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Rich and savory.", "French Onion Soup", 5, new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7273), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "Chinese classic.", "Sweet and Sour Pork", 6, new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7279), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "Fresh Mediterranean salad.", "Greek Salad", 7, new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7294), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "American comfort food.", "Burger Deluxe", 8, new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7299), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, "Thai stir-fried noodles.", "Pad Thai", 9, new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7304), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, "Spanish rice dish.", "Paella", 10, new DateTime(2026, 5, 27, 12, 5, 30, 828, DateTimeKind.Local).AddTicks(7309), new DateTime(2090, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "CategoryRecipes",
                columns: new[] { "CategoryId", "RecipeId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 5 },
                    { 5, 4 },
                    { 6, 6 },
                    { 7, 7 },
                    { 8, 8 },
                    { 9, 9 },
                    { 10, 10 }
                });

            migrationBuilder.InsertData(
                table: "Favorites",
                columns: new[] { "RecipeId", "UserId" },
                values: new object[,]
                {
                    { 2, 1 },
                    { 1, 2 },
                    { 5, 3 },
                    { 4, 4 },
                    { 3, 5 },
                    { 6, 6 },
                    { 7, 7 },
                    { 8, 8 },
                    { 9, 9 },
                    { 10, 10 }
                });

            migrationBuilder.InsertData(
                table: "RecipeIngredients",
                columns: new[] { "IngredientId", "RecipeId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 7, 3 },
                    { 6, 4 },
                    { 4, 5 },
                    { 7, 6 },
                    { 9, 7 },
                    { 8, 8 },
                    { 6, 9 },
                    { 10, 10 }
                });

            migrationBuilder.InsertData(
                table: "RecipeSteps",
                columns: new[] { "RecipeId", "StepId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 },
                    { 5, 5 },
                    { 6, 6 },
                    { 7, 7 },
                    { 8, 8 },
                    { 9, 9 },
                    { 10, 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryRecipes_RecipeId",
                table: "CategoryRecipes",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_RecipeId",
                table: "Favorites",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_Name",
                table: "Ingredients",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_IngredientId",
                table: "RecipeIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_Name",
                table: "Recipes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_UserId",
                table: "Recipes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSteps_StepId",
                table: "RecipeSteps",
                column: "StepId");

            migrationBuilder.CreateIndex(
                name: "IX_Steps_Name",
                table: "Steps",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryRecipes");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "RecipeIngredients");

            migrationBuilder.DropTable(
                name: "RecipeSteps");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Steps");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
