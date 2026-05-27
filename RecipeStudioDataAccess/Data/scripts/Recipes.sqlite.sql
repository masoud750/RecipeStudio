--PRAGMA foreign_keys = ON;
-- Users table
CREATE TABLE IF NOT EXISTS Users (
  user_id INTEGER PRIMARY KEY AUTOINCREMENT,
  username TEXT NOT NULL,
  email TEXT NOT NULL,
  password TEXT NOT NULL,
  valid_from DATETIME DEFAULT CURRENT_TIMESTAMP,
  valid_to DATETIME DEFAULT '2090-01-01 00:00:00'
);

-- Recipes table
CREATE TABLE IF NOT EXISTS Recipes (
  recipe_id INTEGER PRIMARY KEY AUTOINCREMENT,
  name TEXT NOT NULL UNIQUE,
  description TEXT NULL ,
  user_id INTEGER NOT NULL,

  valid_from DATETIME DEFAULT CURRENT_TIMESTAMP,
  valid_to DATETIME DEFAULT '2090-01-01 00:00:00',
  FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- Categories table
CREATE TABLE IF NOT EXISTS Categories (
  category_id INTEGER PRIMARY KEY AUTOINCREMENT,
  name TEXT NOT NULL UNIQUE,
  valid_from DATETIME DEFAULT CURRENT_TIMESTAMP,
  valid_to DATETIME DEFAULT '2090-01-01 00:00:00'
);

-- CategoryRecipes junction table
CREATE TABLE IF NOT EXISTS CategoryRecipes (
  category_id INTEGER NOT NULL,
  recipe_id INTEGER NOT NULL,
  PRIMARY KEY (category_id, recipe_id),
  FOREIGN KEY (category_id) REFERENCES Categories(category_id),
  FOREIGN KEY (recipe_id) REFERENCES Recipes(recipe_id)
);

-- Steps table
CREATE TABLE IF NOT EXISTS Steps (
  step_id INTEGER PRIMARY KEY AUTOINCREMENT,
  name TEXT NOT NULL,
  valid_from DATETIME DEFAULT CURRENT_TIMESTAMP,
  valid_to DATETIME DEFAULT '2090-01-01 00:00:00'
);

-- Ingredients table
CREATE TABLE IF NOT EXISTS Ingredients (
  ingredient_id INTEGER PRIMARY KEY AUTOINCREMENT,
  name TEXT NOT NULL UNIQUE,
  valid_from DATETIME DEFAULT CURRENT_TIMESTAMP,
  valid_to DATETIME DEFAULT '2090-01-01 00:00:00'
);

-- RecipeSteps junction table
CREATE TABLE IF NOT EXISTS RecipeSteps (
  recipe_id INTEGER NOT NULL,
  step_id INTEGER NOT NULL,
  PRIMARY KEY (recipe_id, step_id),
  FOREIGN KEY (recipe_id) REFERENCES Recipes(recipe_id),
  FOREIGN KEY (step_id) REFERENCES Steps(step_id)
);

-- RecipeIngredients junction table
CREATE TABLE IF NOT EXISTS RecipeIngredients (
  recipe_id INTEGER NOT NULL,
  ingredient_id INTEGER NOT NULL,
  PRIMARY KEY (recipe_id, ingredient_id),
  FOREIGN KEY (recipe_id) REFERENCES Recipes(recipe_id),
  FOREIGN KEY (ingredient_id) REFERENCES Ingredients(ingredient_id)
);

-- Favorites junction table
CREATE TABLE IF NOT EXISTS Favorites (
  recipe_id INTEGER NOT NULL,
  user_id INTEGER NOT NULL,
  PRIMARY KEY (recipe_id, user_id),
  FOREIGN KEY (recipe_id) REFERENCES Recipes(recipe_id),
  FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

CREATE TRIGGER validate_email_before_insert
BEFORE INSERT ON Users
FOR EACH ROW
WHEN NEW.email NOT LIKE '%_@__%.__%'
BEGIN
    SELECT RAISE(ABORT, 'Invalid email address');
END;

CREATE TRIGGER prevent_self_favorite
BEFORE INSERT ON Favorites
FOR EACH ROW
WHEN (SELECT user_id FROM Recipes WHERE recipe_id = NEW.recipe_id) = NEW.user_id
BEGIN
    SELECT RAISE(ABORT, 'Users cannot favorite their own recipes');
END;