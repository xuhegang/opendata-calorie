using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebApplication13.Models;

namespace WebApplication13.Controllers
{
    public class HomeController : Controller
    {
        //test
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
        static PersonDetail pd = new PersonDetail();
        static RecipesList recipesList;
        static List<SpecificRecipe> recommandList = new List<SpecificRecipe>();


        public void GetArray()
        {
            int str1 = 0;
            int str2 = 0;
            int str3 = 0;
            int str4 = 0;
            int str5 = 0;
            int str6 = 0;

           
            foreach (var recipe in pd.myRecipes)
            {
                
                str1 += int.Parse(Regex.Replace(recipe.NutritionMap[1], @"[^0-9]+", ""));
                str2 += int.Parse(Regex.Replace(recipe.NutritionMap[3], @"[^0-9]+", ""));
                str3 += int.Parse(Regex.Replace(recipe.NutritionMap[5], @"[^0-9]+", ""));
                str4 += int.Parse(Regex.Replace(recipe.NutritionMap[7], @"[^0-9]+", ""));
                str5 += int.Parse(Regex.Replace(recipe.NutritionMap[9], @"[^0-9]+", ""));
                str6 += int.Parse(Regex.Replace(recipe.NutritionMap[11], @"[^0-9]+", ""));
            }
            str1 /= 65;
            str2 /= 300;
            str3 /= 2262;
            str4 /= 3467;
            str5 /= 308;
            str6 /= 45;
            string str = str1 + "," + str2 + "," + str3 + "," + str4 + "," + str5 + "," + str6;
  
            Response.Write(str);
            Response.End();
        }
        public void Add(string rid)
        {
            foreach (SpecificRecipe sp in recipesList.Rlist)
            {
                if (sp.Id.Equals(rid) && !pd.myRecipes.Contains(sp))
                {
                    pd.myRecipes.Add(sp);
                    
                }
                else
                {
                    int a = 0;
                }
            }
        }

        public void Remove(string rid)
        {
            foreach (SpecificRecipe sp in recipesList.Rlist)
            {
                if (sp.Id.Equals(rid) && pd.myRecipes.Contains(sp))
                {
                    pd.myRecipes.Remove(sp);

                }
            }
        }
        public ActionResult Detail(string rid)
        {
            recommandList.Clear();
            foreach(SpecificRecipe sp in recipesList.Rlist)
            {
                if (sp.Id.Equals(rid))
                {
                    string[] target = new string[3];
                    target[0] = getTargetMile("walk", int.Parse(sp.Calorie));
                    target[1] = getTargetMile("ran", int.Parse(sp.Calorie));
                    target[2] = getTargetMile("bicycling", int.Parse(sp.Calorie));
                    ViewBag.myRecipes = pd.myRecipes;
                    ViewBag.sp = sp;
                    ViewBag.nutrition = sp.NutritionMap;
                    ViewBag.exercise = target;
                    

                }
                else
                {
                    if(recommandList.Count<6)
                        recommandList.Add(sp);
                }
            }
            ViewBag.recommand = recommandList;
            return View("recipe");
        }
        public ActionResult InitialRec(string search )
        {
            string searchstr = null;
            //尝试将数据放入model
            if (search == null)
            {
                pd.Gender = Request.Form["genderOption"];
                pd.Age = Request.Form["age"];
                pd.Height = Request.Form["hight"];
                pd.Weight = Request.Form["weight"];
                pd.Expect = Request.Form["expectOption"];
                switch (pd.Gender)
                {
                    //男: BMR = 66 + ( 13.7 x 体重kg ) + ( 5 x 身高cm ) - ( 6.8 x 年龄years )
                    case "Male":
                        pd.planCal = 66 + (13.7 * double.Parse(pd.Weight)) + (5 * double.Parse(pd.Height)) - (6.8 * double.Parse(pd.Age));
                        break;
                    //女: BMR = 655 + ( 9.6 x 体重kg ) + ( 1.8 x 身高cm ) - ( 4.7 x 年龄years )
                    case "Female":
                        pd.planCal = 655 + (9.6 * double.Parse(pd.Weight)) + (1.8 * double.Parse(pd.Height)) - (4.7 * double.Parse(pd.Age));
                        break;
                    default:
                        break;
                }
                //根据需求判断最终计划的卡路里
                switch (pd.Expect)
                {
                    //男: BMR = 66 + ( 13.7 x 体重kg ) + ( 5 x 身高cm ) - ( 6.8 x 年龄years )
                    case "increase":
                        pd.planCal += 500;
                        break;
                    //女: BMR = 655 + ( 9.6 x 体重kg ) + ( 1.8 x 身高cm ) - ( 4.7 x 年龄years )
                    case "decrease":
                        pd.planCal -= 500;
                        break;
                    default:
                        break;
                }
            }
            Dictionary<string, string> recipesMap = new Dictionary<string, string>();
            if (search == null)
            {
                //old key
                searchstr = "https://api.edamam.com/search?q=Cookies&app_id=548f1190&app_key=a1eca730c61f3d43957dde7be72596ca";
                //new key 旧的用完了就用这个
                //searchstr = "https://api.edamam.com/search?q=Cookies&app_id=a0079ab5&app_key=84e0a515092f45d3f74c8e25bbaabf49";

            }
            else
            {
                //old key
                searchstr = "https://api.edamam.com/search?q=" + search + "&app_id=548f1190&app_key=a1eca730c61f3d43957dde7be72596ca";
                //new key 旧的用完了就用这个
                //searchstr = "https://api.edamam.com/search?q=" + search + "&app_id=a0079ab5&app_key=84e0a515092f45d3f74c8e25bbaabf49";
            }
            var request = (HttpWebRequest)WebRequest.Create(searchstr);
            var response = (HttpWebResponse)request.GetResponse();
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Root rb = JsonConvert.DeserializeObject<Root>(responseString);
            recipesList = new RecipesList();
            recipesList.Rlist = new List<SpecificRecipe>();
            int number = 0;
            int rid = 0;
            foreach (HitsItem hitsItem in rb.hits)
            {  
                if (!recipesMap.ContainsKey(hitsItem.recipe.label))
                {
                    recipesMap.Add(hitsItem.recipe.label, hitsItem.recipe.label);
                    SpecificRecipe specificrecipe = new SpecificRecipe();
                    specificrecipe.title = hitsItem.recipe.label;           
                    specificrecipe.ImageUrl = hitsItem.recipe.image;
                    specificrecipe.Id = (rid++).ToString();
                    foreach(string str in hitsItem.recipe.ingredientLines)
                    {
                        if (specificrecipe.Ingredients == null)
                        {
                            specificrecipe.Ingredients += str;
                        }
                        specificrecipe.Ingredients += (","  + str);
                    }

                    specificrecipe.IngredientsArray = Regex.Split(specificrecipe.Ingredients, ",", RegexOptions.IgnoreCase);
                    specificrecipe.Yield = (int)float.Parse(hitsItem.recipe.yield);
                    specificrecipe.Calorie = ((int)(float.Parse(hitsItem.recipe.calories) / float.Parse(hitsItem.recipe.yield))).ToString();
                    recipesList.Rlist.Add(specificrecipe);
                    /*specificrecipe.NutritionMap = new Dictionary<string, string>();
                    //添加营养数据
                    specificrecipe.NutritionMap.Add("Total Fat", (int)(hitsItem.recipe.totalNutrients.FAT.quantity) + hitsItem.recipe.totalNutrients.FAT.unit);
                    specificrecipe.NutritionMap.Add("Cholesterol", ((int)(hitsItem.recipe.totalNutrients.CHOLE.quantity)) + hitsItem.recipe.totalNutrients.CHOLE.unit);
                    specificrecipe.NutritionMap.Add("Sodium", (int)(hitsItem.recipe.totalNutrients.NA.quantity) + hitsItem.recipe.totalNutrients.NA.unit);
                    specificrecipe.NutritionMap.Add("Potassium", (int)(hitsItem.recipe.totalNutrients.K.quantity) + hitsItem.recipe.totalNutrients.K.unit);
                    specificrecipe.NutritionMap.Add("Total Carbohydrates", (int)(hitsItem.recipe.totalNutrients.SUGAR.quantity)+(int)(hitsItem.recipe.totalNutrients.FIBTG.quantity) + hitsItem.recipe.totalNutrients.SUGAR.unit); 
                    specificrecipe.NutritionMap.Add("Protein", (int)(hitsItem.recipe.totalNutrients.PROCNT.quantity) + hitsItem.recipe.totalNutrients.PROCNT.unit);
                    */
                   
                    
                    specificrecipe.NutritionMap[0] = "Total Fat";
                    if(hitsItem.recipe.totalNutrients.FAT != null)
                    {
                        specificrecipe.NutritionMap[1] = (int)(hitsItem.recipe.totalNutrients.FAT.quantity) + hitsItem.recipe.totalNutrients.FAT.unit;
                    }
                    else
                    {
                        specificrecipe.NutritionMap[1] = "0";
                    }
                    specificrecipe.NutritionMap[2] = "Cholesterol";
                    if (hitsItem.recipe.totalNutrients.CHOLE != null)
                    {
                        specificrecipe.NutritionMap[3] = ((int)(hitsItem.recipe.totalNutrients.CHOLE.quantity)) + hitsItem.recipe.totalNutrients.CHOLE.unit;
                    }
                    else
                    {
                        specificrecipe.NutritionMap[3] = "0";
                    }
                   
                    specificrecipe.NutritionMap[4] = "Sodium";
                    if (hitsItem.recipe.totalNutrients.NA != null)
                    {
                        specificrecipe.NutritionMap[5] = (int)(hitsItem.recipe.totalNutrients.NA.quantity) + hitsItem.recipe.totalNutrients.NA.unit;
                    }
                    else
                    {
                        specificrecipe.NutritionMap[5] = "0";
                    }

                    specificrecipe.NutritionMap[6] = "Potassium";
                    if (hitsItem.recipe.totalNutrients.K != null)
                    {
                        specificrecipe.NutritionMap[7] = (int)(hitsItem.recipe.totalNutrients.K.quantity) + hitsItem.recipe.totalNutrients.K.unit;
                    }
                    else
                    {
                        specificrecipe.NutritionMap[7] = "0";
                    }

                    specificrecipe.NutritionMap[8] = "Total Carbohydrates";
                    if (hitsItem.recipe.totalNutrients.SUGAR != null && hitsItem.recipe.totalNutrients.FIBTG!=null)
                    {
                        specificrecipe.NutritionMap[9] = ((int)(hitsItem.recipe.totalNutrients.SUGAR.quantity) + (int)(hitsItem.recipe.totalNutrients.FIBTG.quantity)) + hitsItem.recipe.totalNutrients.SUGAR.unit;
                    }
                    else
                    {
                        specificrecipe.NutritionMap[9] = "0";
                    }
                    specificrecipe.NutritionMap[10] = "Protein";
                    if (hitsItem.recipe.totalNutrients.PROCNT != null)
                    {
                        specificrecipe.NutritionMap[11] = (int)(hitsItem.recipe.totalNutrients.PROCNT.quantity) + hitsItem.recipe.totalNutrients.PROCNT.unit;
                    }
                    else
                    {
                        specificrecipe.NutritionMap[11] = "0";
                    }
                }
                number++;
                if (number == 9)
                {
                    break;
                }
            }
            //把信息放入大类...
            recipesList.Pd = pd;

            if (search == null)
            {
               return View("MenuList",recipesList);
            }
            else
            {
               return PartialView("Recipeslist", recipesList);
            }
           
        }
        //public string GetIngredient(string recipeid)
        //{
        //    string totalIngredient = "";

        //    string searchstr = "http://food2fork.com/api/get?key=b18c7f80205225f5a6c3585901b06092&rId=" + recipeid;
        //    var request = (HttpWebRequest)WebRequest.Create(searchstr);
        //    var response = (HttpWebResponse)request.GetResponse();
        //    string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

        //    Root rb = JsonConvert.DeserializeObject<Root>(responseString);
        //    int i = 0;
        //    foreach (string ingredient in rb.recipe.ingredients)
        //    {
        //        if (i != 0)
        //        {
        //            totalIngredient = totalIngredient + "," + ingredient;
        //        }
        //        else
        //        {
        //            totalIngredient = totalIngredient + ingredient;
        //        }
        //        i++;
        //    }
        //    // string[] searchArray = Regex.Split(totalIngredient, ",", RegexOptions.IgnoreCase);
        //    return totalIngredient;
        //}


        //public string getNutrition(string[] searchArray,string serveNum)
        //{
        //    String url = "https://api.edamam.com/search?q=chicken&app_id=548f1190&app_key=a1eca730c61f3d43957dde7be72596ca";
        //    searchArray[searchArray.Length - 1] = searchArray[searchArray.Length - 1].Replace("\n", "");
        //    for (int i = 0; i < searchArray.Length; i++)
        //    {
        //        if (searchArray[i].Contains("\""))
        //        {
        //            searchArray[i]= searchArray[i].Replace("\"", " ");
        //        }
        //        if (searchArray[i].Contains("&"))
        //        {
        //            searchArray[i] = searchArray[i].Replace("&", " ");
        //        }

        //    }
        //    String content = "{\"query\":\"";
        //    //拼接查询字符串
        //     for (int i = 0; i < searchArray.Length; i++)
        //     {
        //         content = content + searchArray[i]+ " and ";
        //         if (i == searchArray.Length - 1)
        //         {
        //             content = content + "\"}";
        //        }
        //     }
           
        //    String result = "";
        //    Dictionary<string, string> calroieMap = new Dictionary<string, string>();
        //    //发送请求
        //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        //    req.Method = "POST";
        //    req.ContentType = "application/json";
        //    req.Headers.Add("x-app-id", "7258bd60");
        //    req.Headers.Add("x-app-key", "d8de75744e7faca5111b0e60efc09549");
        //    byte[] data = Encoding.UTF8.GetBytes(content);
        //    req.ContentLength = data.Length;
        //    using (Stream reqStream = req.GetRequestStream())
        //    {
        //        reqStream.Write(data, 0, data.Length);
        //        reqStream.Close();
        //    }
        //    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        //    Stream stream = resp.GetResponseStream();
        //    //获取响应内容  
        //    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        //    {
        //        result = reader.ReadToEnd();
        //    }
        //    //json解析
        //    RootObject rb = JsonConvert.DeserializeObject<RootObject>(result);
        //    foreach (Foods ep in rb.foods)
        //    {
        //        if (!calroieMap.ContainsKey(ep.food_name))
        //        {
        //            calroieMap.Add(ep.food_name, ep.nf_calories);
        //        }
        //    }
        //    int calroie = 0;
        //    foreach(var item in calroieMap)
        //    {
        //        calroie = calroie + (int)float.Parse(item.Value);
        //    }

        //    return (calroie/(int)float.Parse(serveNum)).ToString();
            
        //}

        public ActionResult CheckOut()
        
        {
            
           
           
            int totalcal=0;
            foreach(SpecificRecipe sp in pd.myRecipes)
            {
                totalcal += int.Parse(sp.Calorie);
            }
            int targetcal = totalcal - (int)(pd.planCal);
            string[] target = new string[3];
            target[0] = getTargetMile("walk", targetcal);
            target[1] = getTargetMile("ran", targetcal);
            target[2] = getTargetMile("bicycling", targetcal);
            ViewBag.myRecipes = pd.myRecipes;
            ViewBag.exercise = target;
            ViewBag.target = targetcal;
            return View();
        }

        public string getTargetMile(string type,int targetcal)
        {
            int[] runCalArr = new int[9];
            string[] runmile = new string[9];
            String url = "https://trackapi.nutritionix.com/v2/natural/exercise";
            //  String[] searchArray = { "five apple", "two bread", "5000ml milk" };
            String content = "{\"query\":\"";
            //拼接查询字符串
            for (int i = 0; i < 9; i++)
            {
                runmile[i] = content + type + " " + (i + 1) + " miles\"," + "\"gender\":\"" + pd.Gender + "\",\"weight_kg\":\"" + pd.Weight + "\",\"height_cm\":\"" + pd.Height + "\",\"age\":\"" + pd.Age + "\"}";

                String result = "";
               
                //发送请求
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/json";
                //old account
                req.Headers.Add("x-app-id", "dd79a6ae");
                req.Headers.Add("x-app-key", "039958acd7a63688e95b981df11901b6");
                //new key
                //req.Headers.Add("x-app-id", "d6be168c");
                //req.Headers.Add("x-app-key", "7eefef8e945dba3acdf87d4c90343bef");
                byte[] data = Encoding.UTF8.GetBytes(runmile[i]);
                req.ContentLength = data.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                //获取响应内容  
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
                //json解析
                rootb rb = JsonConvert.DeserializeObject<rootb>(result);

                foreach (Exercises exercise in rb.exercises)
                {
                    runCalArr[i] = (targetcal - (int)double.Parse(exercise.nf_calories)) < 0 ? -(targetcal - (int)double.Parse(exercise.nf_calories)) : (targetcal - (int)double.Parse(exercise.nf_calories));
                }
            }

            ArrayList al1 = new ArrayList(runCalArr);
            string targetMiles="";
            al1.Sort();
            int min1 = Convert.ToInt32(al1[0]);
            for (int i = 0; i < 9; i++)
            {
                if (runCalArr[i] == min1)
                {
                    targetMiles = (i + 1).ToString();
                }
            }
            return targetMiles;
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

      
        /*public class Recipe
        {
            public string publisher { get; set; }
            public string f2f_url { get; set; }
            public List<string> ingredients { get; set; }
            public string source_url { get; set; }
            public string recipe_id { get; set; }
            public string image_url { get; set; }
            public string social_rank { get; set; }
            public string publisher_url { get; set; }
            public string title { get; set; }
        }

        public class SpecificRecipeRootObject
        {
            public Recipe recipe { get; set; }
        }
        */


        // 。。。。
        public class Params
        {
            /// <summary>
            /// 
            /// </summary>
            public List<string> sane { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> q { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> app_key { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> health { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> from { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> to { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> calories { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> app_id { get; set; }
        }

        public class IngredientsItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string text { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double weight { get; set; }
        }

        public class ENERC_KCAL
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class FAT
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class FASAT
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class FATRN
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class FAMS
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class FAPU
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class CHOCDF
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class FIBTG
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class SUGAR
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class PROCNT
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class CHOLE
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class NA
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class CA
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class MG
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class K
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class FE
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class ZN
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class P
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class VITA_RAE
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class VITC
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class THIA
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class RIBF
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class NIA
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class VITB6A
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class FOLDFE
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class FOLFD
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class VITB12
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class VITD
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class TOCPHA
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class VITK1
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double quantity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class TotalNutrients
        {
            /// <summary>
            /// 
            /// </summary>
            public ENERC_KCAL ENERC_KCAL { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public FAT FAT { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public FASAT FASAT { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public FATRN FATRN { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public FAMS FAMS { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public FAPU FAPU { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public CHOCDF CHOCDF { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public FIBTG FIBTG { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public SUGAR SUGAR { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public PROCNT PROCNT { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public CHOLE CHOLE { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public NA NA { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public CA CA { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public MG MG { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public K K { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public FE FE { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public ZN ZN { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public P P { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public VITA_RAE VITA_RAE { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public VITC VITC { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public THIA THIA { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public RIBF RIBF { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public NIA NIA { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public VITB6A VITB6A { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public FOLDFE FOLDFE { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public FOLFD FOLFD { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public VITB12 VITB12 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public VITD VITD { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public TOCPHA TOCPHA { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public VITK1 VITK1 { get; set; }
        }

        

       
        

        public class TotalDaily
        {
            /// <summary>
            /// 
            /// </summary>
            public ENERC_KCAL ENERC_KCAL { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public FAT FAT { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public FASAT FASAT { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public CHOCDF CHOCDF { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public FIBTG FIBTG { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public PROCNT PROCNT { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public CHOLE CHOLE { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public NA NA { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public CA CA { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public MG MG { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public K K { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public FE FE { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public ZN ZN { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public P P { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public VITA_RAE VITA_RAE { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public VITC VITC { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public THIA THIA { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public RIBF RIBF { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public NIA NIA { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public VITB6A VITB6A { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public FOLDFE FOLDFE { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public VITB12 VITB12 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public VITD VITD { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public TOCPHA TOCPHA { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public VITK1 VITK1 { get; set; }
        }

        public class SubItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string tag { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string schemaOrgTag { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double total { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string hasRDI { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double daily { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
        }

        public class DigestItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string tag { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string schemaOrgTag { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double total { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string hasRDI { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double daily { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string unit { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<SubItem> sub { get; set; }
        }

        public class Recipe
        {
            /// <summary>
            /// 
            /// </summary>
            public string uri { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string image { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string source { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string shareAs { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string yield { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> dietLabels { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> healthLabels { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> cautions { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> ingredientLines { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<IngredientsItem> ingredients { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string calories { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string totalWeight { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string totalTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public TotalNutrients totalNutrients { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public TotalDaily totalDaily { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<DigestItem> digest { get; set; }
        }

        public class HitsItem
        {
            /// <summary>
            /// 
            /// </summary>
            public Recipe recipe { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string bookmarked { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string bought { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public string q { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int from { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int to { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Params params2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string more { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<HitsItem> hits { get; set; }


    }

        public class Photo1
        {
            public string thumb { get; set; }
            public string highres { get; set; }
        }

        public class Exercises
        {
            public string tag_id { get; set; }
            public string user_input { get; set; }
            public string duration_min { get; set; }
            public string met { get; set; }
            public string nf_calories { get; set; }
            public Photo1 photo { get; set; }
            public string compendium_code { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string benefits { get; set; }
        }

        public class rootb
        {
            public List<Exercises> exercises { get; set; }
        }

    }
}