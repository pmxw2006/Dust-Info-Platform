using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HaoYouController : Controller
    {
        // GET: HaoYou
        public ActionResult ZhuYe()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Index", "YongHu");
            }
            return View();
        }
    }
}