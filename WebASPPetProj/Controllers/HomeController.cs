using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebASPPetProj.Models;

namespace WebASPPetProj.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            MainView model = new MainView();
            model.Categories = db.Categories.ToList<Category>();
            model.Posts = db.Posts.ToList<Post>();
            return View(model);
        }

    }
}