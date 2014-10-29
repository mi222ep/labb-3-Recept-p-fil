using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FiledRecipes.Domain
{
    /// <summary>
    /// Holder for recipes.
    /// </summary>
    public class RecipeRepository : IRecipeRepository
    {
        /// <summary>
        /// Represents the recipe section.
        /// </summary>
        private const string SectionRecipe = "[Recept]";

        /// <summary>
        /// Represents the ingredients section.
        /// </summary>
        private const string SectionIngredients = "[Ingredienser]";

        /// <summary>
        /// Represents the instructions section.
        /// </summary>
        private const string SectionInstructions = "[Instruktioner]";

        /// <summary>
        /// Occurs after changes to the underlying collection of recipes.
        /// </summary>
        public event EventHandler RecipesChangedEvent;

        /// <summary>
        /// Specifies how the next line read from the file will be interpreted.
        /// </summary>
        private enum RecipeReadStatus { Indefinite, New, Ingredient, Instruction };

        /// <summary>
        /// Collection of recipes.
        /// </summary>
        private List<IRecipe> _recipes;

        /// <summary>
        /// The fully qualified path and name of the file with recipes.
        /// </summary>
        private string _path;

        /// <summary>
        /// Indicates whether the collection of recipes has been modified since it was last saved.
        /// </summary>
        public bool IsModified { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the RecipeRepository class.
        /// </summary>
        /// <param name="path">The path and name of the file with recipes.</param>
        public RecipeRepository(string path)
        {
            // Throws an exception if the path is invalid.
            _path = Path.GetFullPath(path);

            _recipes = new List<IRecipe>();
        }

        /// <summary>
        /// Returns a collection of recipes.
        /// </summary>
        /// <returns>A IEnumerable&lt;Recipe&gt; containing all the recipes.</returns>
        public virtual IEnumerable<IRecipe> GetAll()
        {
            // Deep copy the objects to avoid privacy leaks.
            return _recipes.Select(r => (IRecipe)r.Clone());
        }

        /// <summary>
        /// Returns a recipe.
        /// </summary>
        /// <param name="index">The zero-based index of the recipe to get.</param>
        /// <returns>The recipe at the specified index.</returns>
        public virtual IRecipe GetAt(int index)
        {
            // Deep copy the object to avoid privacy leak.
            return (IRecipe)_recipes[index].Clone();
        }

        /// <summary>
        /// Deletes a recipe.
        /// </summary>
        /// <param name="recipe">The recipe to delete. The value can be null.</param>
        public virtual void Delete(IRecipe recipe)
        {
            // If it's a copy of a recipe...
            if (!_recipes.Contains(recipe))
            {
                // ...try to find the original!
                recipe = _recipes.Find(r => r.Equals(recipe));
            }
            _recipes.Remove(recipe);
            IsModified = true;
            OnRecipesChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Deletes a recipe.
        /// </summary>
        /// <param name="index">The zero-based index of the recipe to delete.</param>
        public virtual void Delete(int index)
        {
            Delete(_recipes[index]);
        }

        /// <summary>
        /// Raises the RecipesChanged event.
        /// </summary>
        /// <param name="e">The EventArgs that contains the event data.</param>
        protected virtual void OnRecipesChanged(EventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of 
            // a race condition if the last subscriber unsubscribes 
            // immediately after the null check and before the event is raised.
            EventHandler handler = RecipesChangedEvent;

            // Event will be null if there are no subscribers. 
            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, e);
            }
        }
//Klassen använder i samband 
//med inläsning av recept lämpligen den uppräkningsbara typen RecipeReadStatus för att hålla 
//ordningen på vilken typ av data som lästs in från textfilen
        public void Load()
        {
        //Skapa en dynamisk Array där listan sparas
            List<IRecipe> receipeRow = new List<IRecipe>();
            RecipeReadStatus status = RecipeReadStatus.Indefinite;

            //Indefinite, New, Ingredient, Instruction
            //Skapar automatiskt en try-finally-sats som stänger StreamReader när det är klart.
            using (StreamReader sr = new StreamReader(_path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if(line == SectionRecipe)
                    {
                        status = RecipeReadStatus.New;
                    }
                    if (line == SectionIngredients)
                    {
                        status = RecipeReadStatus.Ingredient;
                    }
                    if (line == SectionInstructions)
                    {
                        status = RecipeReadStatus.Instruction;
                    }
                    else
                    {
                        IRecipe recept = null;

                       if (status == RecipeReadStatus.New)
                       {
                           recept = new Recipe(line);
                           receipeRow.Add(recept);
                       }
                       if (status == RecipeReadStatus.Ingredient)
                       {
                           string[] ingredients = line.Split(';');
                           Ingredient ingr = new Ingredient();
                           ingredients[0] = ingr.Amount;
                           ingredients[1] = ingr.Measure;
                           ingredients[2] = ingr.Name;
                           
                           recept.Add(ingr);

                       }
                       if (status == RecipeReadStatus.Instruction)
                       {
                           recept.Add(line);
                       }
                       else
                       {
                           throw new FileFormatException();
                       }

                    }
                }
                receipeRow.Sort();
                _recipes = receipeRow;
                IsModified = false;
                OnRecipesChanged(EventArgs.Empty);

            }
        }
        public void Save() 
        {
            using (StreamWriter sw = new StreamWriter("_path"))
            {
                foreach (Recipe item in _recipes) 
                {
                    sw.Write(SectionRecipe);
                    sw.Write(item.Name);
                    sw.Write(SectionIngredients);
                    foreach (Ingredient ingred in item.Ingredients) { 
                    sw.Write("[0];[1];[2];", ingred.Amount, ingred.Measure, ingred.Name);
                    }
                    sw.Write(SectionInstructions);
                    foreach(string instruction in item.Instructions){
                        sw.Write(item);
                    }
                }
            }
        }
    }
}

