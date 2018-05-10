using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication13.Models
{
    public class RecipesList
    {
        public List<SpecificRecipe> Rlist { get; set; }
        public PersonDetail Pd { get; set; }
    }
    public class SpecificRecipe
    {
        public String title { get; set; }
        public String ImageUrl { get; set; }
        public string Id { get; set; }
        public string Ingredients { get; set; }
        public string Calorie { get; set; }
        public int Yield { get; set; } //记录这道菜是几人份
    }
}