using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication13.Models;

namespace WebApplication13.Controllers
{
    //我试试能不能push成功
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult InitialRec(string search )
        {
            string searchstr = null;        
            var gender= Request.Form["option"];
            var age = Request.Form["age"];
            Dictionary<string, string> recipesMap = new Dictionary<string, string>();
            if (search == null)
            {
                 searchstr = "http://food2fork.com/api/search?key=7980b1abc6ccc9eb785e5aee4e972120&q=chinese";
            }else
            {
                 searchstr = "http://food2fork.com/api/search?key=7980b1abc6ccc9eb785e5aee4e972120&q=" + search;              
            }
            var request = (HttpWebRequest)WebRequest.Create(searchstr);
            var response = (HttpWebResponse)request.GetResponse();
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            RecipesRootObject rb = JsonConvert.DeserializeObject<RecipesRootObject>(responseString);
            RecipesList recipesList = new RecipesList();
            recipesList.Rlist = new List<Recipe>();
            int number = 0;
            foreach (Recipes recipes in rb.recipes)
            {  
                if (!recipesMap.ContainsKey(recipes.title))
                {
                    Recipe recipe = new Recipe();
                    recipe.title = recipes.title;
                    recipe.ImageUrl = recipes.image_url;
                    recipesList.Rlist.Add(recipe);
                }
                number++;
                if (number == 18)
                {
                    break;
                }
            }
           // ViewData["data"] = recipesMap;
            
            return View("MenuList",recipesList);
        }

        public ActionResult ChangeRecipes(string search)
        {
            string searchstr = null;
            var gender = Request.Form["option"];
            var age = Request.Form["age"];
            Dictionary<string, string> recipesMap = new Dictionary<string, string>();
            if (search == null)
            {
                searchstr = "http://food2fork.com/api/search?key=7980b1abc6ccc9eb785e5aee4e972120&q=chinese";
            }
            else
            {
                searchstr = "http://food2fork.com/api/search?key=7980b1abc6ccc9eb785e5aee4e972120&q=" + search;
            }
            var request = (HttpWebRequest)WebRequest.Create(searchstr);
            var response = (HttpWebResponse)request.GetResponse();
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            RecipesRootObject rb = JsonConvert.DeserializeObject<RecipesRootObject>(responseString);
            RecipesList recipesList = new RecipesList();
            recipesList.Rlist = new List<Recipe>();
            int number = 0;
            foreach (Recipes recipes in rb.recipes)
            {
                if (!recipesMap.ContainsKey(recipes.title))
                {
                    Recipe recipe = new Recipe();
                    recipe.title = recipes.title;
                    recipe.ImageUrl = recipes.image_url;
                    recipesList.Rlist.Add(recipe);
                }
                number++;
                if (number == 18)
                {
                    break;
                }
            }
            // ViewData["data"] = recipesMap;

            return PartialView("Recipeslist", recipesList);
        }


        public int GetDefaultRecipe()
        {
            return 1;
        }




        public class Full_nutrients
        {
            public string attr_id { get; set; }
            public string value { get; set; }
        }

        public class Metadata
        {
        }

        public class Tags
        {
            public string item { get; set; }
            public string measure { get; set; }
            public string quantity { get; set; }
            public string tag_id { get; set; }
        }

        public class Alt_measures
        {
            public string serving_weight { get; set; }
            public string measure { get; set; }
            public string seq { get; set; }
            public string qty { get; set; }
        }

        public class Photo
        {
            public string thumb { get; set; }
            public string highres { get; set; }
        }

        public class Foods
        {
            public string food_name { get; set; }
            public string brand_name { get; set; }
            public string serving_qty { get; set; }
            public string serving_unit { get; set; }
            public string serving_weight_grams { get; set; }
            public string nf_calories { get; set; }
            public string nf_total_fat { get; set; }
            public string nf_saturated_fat { get; set; }
            public string nf_cholesterol { get; set; }
            public string nf_sodium { get; set; }
            public string nf_total_carbohydrate { get; set; }
            public string nf_dietary_fiber { get; set; }
            public string nf_sugars { get; set; }
            public string nf_protein { get; set; }
            public string nf_potassium { get; set; }
            public string nf_p { get; set; }
            public List<Full_nutrients> full_nutrients { get; set; }
            public string nix_brand_name { get; set; }
            public string nix_brand_id { get; set; }
            public string nix_item_name { get; set; }
            public string nix_item_id { get; set; }
            public string upc { get; set; }
            public string consumed_at { get; set; }
            public Metadata metadata { get; set; }
            public string source { get; set; }
            public string ndb_no { get; set; }
            public Tags tags { get; set; }
            public List<Alt_measures> alt_measures { get; set; }
            public string lat { get; set; }
            public string lng { get; set; }
            public string meal_type { get; set; }
            public Photo photo { get; set; }
        }

        public class RootObject
        {
            public List<Foods> foods { get; set; }
        }

        public class Recipes
        {
            public string publisher { get; set; }
            public string f2f_url { get; set; }
            public string title { get; set; }
            public string source_url { get; set; }
            public string recipe_id { get; set; }
            public string image_url { get; set; }
            public string social_rank { get; set; }
            public string publisher_url { get; set; }
        }

        public class RecipesRootObject
        {
            public string count { get; set; }
            public List<Recipes> recipes { get; set; }
        }

    }
}