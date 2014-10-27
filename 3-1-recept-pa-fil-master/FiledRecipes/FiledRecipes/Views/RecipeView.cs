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
//Presentationslagret innehåller ett stort antal typer av vilka typerna IRecipeView och RecipeView är 
//intressantast. Interfacet IRecipeView definierar den funktionalitet den konkreta klassen RecipeView
//ska ha. Basklassen ViewBase innehåller intressanta medlemmar som Header, ShowHeaderPanel och 
//ContinueOnKeyPressed, som kommer till användning vid implementation av metoderna 
//IRecipeView deklarerar.
        //Basklassen ViewBase innehåller intressanta medlemmar som Header, ShowHeaderPanel och 
        //ContinueOnKeyPressed, som kommer till användning vid implementation av metoderna 
        //IRecipeView deklarerar
        public void Show(IEnumerable<IRecipe> recipies)
        {
            //Tar emot sträng från RecipeRepository.cs
            //Olika handlingar görs beroende på vad det är för sträng
            //Om det är en tom rad, gör ingenting
            //Om det är en 
        }
        public void Show(IRecipe recipe)
        {

        }
    }
}
