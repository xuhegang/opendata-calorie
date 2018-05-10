using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication13.Controllers
{
    public class DetailController : Controller
    {
        // GET: Detail
        public ActionResult Recipe()
        {
            return View();
        }
    }
}