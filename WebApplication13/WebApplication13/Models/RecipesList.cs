using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication13.Models
{
    public class RecipesList
    {
        public List<Recipe> Rlist { get; set; }
    }
    public class Recipe
    {
        public String title { get; set; }
        public String ImageUrl { get; set; }
        public string Id { get; set; }
    }
}