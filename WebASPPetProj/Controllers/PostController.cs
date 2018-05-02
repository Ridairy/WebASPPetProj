using WebASPPetProj.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebASPPetProj;
using System.Text.RegularExpressions;

namespace WebASPPetProj.Controllers
{
 
    public class PostController : Controller
    {
        readonly int CutDescriptions = 500;
        private ApplicationDbContext db = new ApplicationDbContext();
    

        // GET: Post
        [Authorize]
        public ActionResult Index()
        {
            using (db)
            {
                List<Category> category = new List<Category>(db.Categories);
                List<Tag> tags = new List<Tag>(db.Tags);
                ViewBag.Category = category;
                ViewBag.Tags = tags;
            }           
            return View();
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Index(PostCreate model)
        {
            if (ModelState.IsValid)
            {
                using (db)
                {
                    {
                        //Post post = new Post()
                        //{
                        //    Title = model.Title,
                        //    Description = model.Description,
                        //    Category = db.Categories.FirstOrDefault(m => m.Id == model.CategoryId),
                        //    Tags = TagSelect(model.TagsId),
                        //    PostedOn = DateTime.Now,
                        //    Posted = true,
                        //    Publisher = await UserManager.FindByEmailAsync(User.Identity.Name),
                        //    ShortDescription = (model.Description.Length > 30) ? model.Description.Substring(0, 30) : model.Description
                        //};
                    }
                    List<Tag> Tags = new List<Tag>();
                    foreach (var t in model.TagsId)
                    {
                        Tag tag = db.Tags.FirstOrDefault(m => m.Id == t);
                        Tags.Add(tag);
                    }
                    db.Posts.Add(new Post() {
                        Title = model.Title,
                        Description = model.Description,
                        Category = db.Categories.FirstOrDefault(m => m.Id == model.CategoryId),
                        Tags = Tags,
                        PostedOn = DateTime.Now,
                        Posted = true,
                        Publisher = db.Users.FirstOrDefault(u=>u.Email==User.Identity.Name),
                        ShortDescription = (model.Description.Length > CutDescriptions) ? model.Description.Substring(0, CutDescriptions) : model.Description,
                        ShortUrl= Slug(model.Title.Replace(" ", "_").ToLower())
                    });
                    await db.SaveChangesAsync();
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index",model) ;
        }
        
        [Authorize]
        public ActionResult CreateCategory()
        {
            return View();
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> CreateCategory(TagCatCreate model)
        {
            if (ModelState.IsValid)
            {
                using (db)
                {
                    if (db.Categories.FirstOrDefault(
                        c=>c.Name.
                        Replace(" ","").
                        ToLower()==
                        model.Name.
                        Replace(" ","").
                        ToLower())!=
                        null)
                    {
                        return View();
                    }
                    Category category = new Category()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        ShortUrl=Slug(model.Name.Replace(" ","_").ToLower())
                    };
                    
                    db.Categories.Add(category);
                    await db.SaveChangesAsync();
                         
                }


            }
                return View();
        }


        [Authorize]
        public ActionResult CreateTag()
        {
            return View();
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> CreateTag(TagCatCreate model)
        {

            if (ModelState.IsValid)
            {
                using (db)
                {
                    if (db.Tags.FirstOrDefault(
                        c => 
                        c.Name.Replace(" ","").
                        ToLower() == 
                        model.Name.Replace(" ","").
                        ToLower()) !=
                        null)
                    {
                        return View();
                    }
                    Tag tag = new Tag()
                    {
                        Name = model.Name,
                        Description = model.Description
                    };
                
                    db.Tags.Add(tag);
                    await db.SaveChangesAsync();
                }
            }
            return View();
        }

        [AllowAnonymous]
        [Route("post/{category}/{pname}/")]
        public ActionResult ShowPost(string category,string postname)
        {

            return View();
        }

        [AllowAnonymous]
        [Route("post/{category}/")]
        public ActionResult ShowAllInCategory(string category)
        {
            return View();
        }

        [AllowAnonymous]
        [Route("user/{username}")]
        public ActionResult ShowUserInfo(string username)
        {
            if (User.Identity.Name==username)
            {
                return RedirectToAction("Index","Manage");
            }
            
            return View();
        }

        private string Slug(string str) {
            string patern = "[a-z0-9_]";
            return Regex.Replace(str, patern,String.Empty);
        }
    }
}