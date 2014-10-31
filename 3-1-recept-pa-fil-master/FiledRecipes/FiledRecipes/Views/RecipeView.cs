using FiledRecipes.Domain;
using FiledRecipes.App.Mvp;
using FiledRecipes.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FiledRecipes.Views
{
    /// <summary>
    /// 
    /// </summary>
    public class RecipeView : ViewBase, IRecipeView
    {
        public void Show(IEnumerable<IRecipe> recipies)
        {
            foreach(Recipe item in recipies)
            {
                Show(item);
                ContinueOnKeyPressed();
            }
        }
        public void Show(IRecipe recipe)
        {
            Header = recipe.Name;
            ShowHeaderPanel();
            Console.WriteLine(recipe.Name);
            Console.WriteLine("Gör såhär");
            Console.WriteLine("===============");
            
            foreach(IIngredient item in recipe.Ingredients)
            {
                
                Console.WriteLine(item);
            }
            foreach(string item in recipe.Instructions){
                int count = 1;
                Console.WriteLine("{0}.", count);
                Console.WriteLine(item);
                count++;
            }
        }
    }
}
