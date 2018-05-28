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
        //Show all posts
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int page=1)
        {
            int pageSize = 5;
            //Get posts on this page
            IEnumerable<Post> postPerPage = db.Posts.OrderByDescending(p=>p.PostedOn).Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = db.Posts.Count() };
            MainView model = new MainView
            {
                Posts = postPerPage.ToList(),
                PageInfo = pageInfo
            };
            return View(model);
        }

    }
}