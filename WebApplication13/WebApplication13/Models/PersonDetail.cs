using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication13.Models
{
    public class PersonDetail
    {
        public String Gender { get; set; }
        public String Age { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Expect { get; set; } //记录用户是想增肥还是减肥还是维持
        public double planCal { get; set; } //记录用户目标的卡路里
        public double currentCal { get; set; } //记录用户当前添加的总卡路里数量
        public RecipesList myRecipes { get; set; }//记录用户想吃的所有菜品

        //默认一个构造函数，并赋予默认值。 防止用户不填写信息直接进入页面
        public PersonDetail()
        {
            this.Gender = "Male";
            this.Age = "28";
            this.Height = "180";
            this.Weight = "70";
            this.Expect = "noChange";//默认不想增肥
            this.planCal = 2000;
            this.currentCal = 0;
            this.myRecipes = null;
        }
    }
}
