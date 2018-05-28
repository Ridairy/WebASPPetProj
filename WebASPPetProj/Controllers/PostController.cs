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
        //Create post
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
                    if (model.TagsId != null)
                    {
                        foreach (var t in model.TagsId)
                        {
                            Tags.Add(db.Tags.FirstOrDefault(m => m.Id == t));
                        }
                    }
                    else
                    {
                        Tags.Add(db.Tags.Find(1));
                    }

                    Category category = db.Categories.FirstOrDefault(m => m.Id == model.CategoryId);

                    ApplicationUser user = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);

                    String Description = model.Description.Replace("\r\n", "<br/>").Replace("\n", "< br />").Replace(Environment.NewLine, "<br/>");
                    Post post = new Post()
                    {
                        Title = model.Title,
                        Description = Description,
                        Category = category,
                        Tags = Tags,
                        PostedOn = DateTime.Now,
                        Posted = true,
                        Publisher = user,
                        ShortDescription = (model.Description.Length > CutDescriptions) ? model.Description.Substring(0, CutDescriptions) : model.Description,
                        ShortUrl = Slug(model.Title.Replace(" ", "_").ToLower())
                    };
                    db.Posts.Add(post);


                    await db.SaveChangesAsync();
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", model);
        }
        //Create category
        [Authorize]
        public ActionResult Category(int page = 1)
        {
            byte PageSize = 10;

            IEnumerable<Category> categories = db.Categories.OrderByDescending(c => c.Name).Skip((page - 1) * PageSize).Take(PageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = PageSize, TotalItems = db.Categories.Count() };
            CategoriesView model = new CategoriesView();
            model.Categories = categories;
            model.pageInfo = pageInfo;
            return View(model);
        }
        //Craete tag
        [Authorize]
        public ActionResult Tag(int page = 1)
        {
            byte PageSize = 10;

            //IEnumerable<Category> categories = db.Categories.OrderByDescending(c => c.Name).Skip((page - 1) * PageSize).Take(PageSize);
            IEnumerable<Tag> tags = db.Tags.OrderByDescending(c => c.Name).Skip((page - 1) * PageSize).Take(PageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = PageSize, TotalItems = db.Tags.Count() };
            TagsView model = new TagsView();
            model.Tags = tags;
            model.pageInfo = pageInfo;
            return View(model);
        }
        //Show some post
        [AllowAnonymous]
        [Route("showpost/{category}/{pname}/")]
        public ActionResult ShowPost(string category, string pname)
        {
            PostView model;
            using (db)
            {
                var post = db.Posts.Where(p => p.ShortUrl == pname).FirstOrDefault();

                if (post != null)
                {
                    model = new PostView
                    {
                        Title = post.Title,
                        Category = post.Category,
                        Description = post.Description,
                        Publisher = post.Publisher,
                        PostedOn = post.PostedOn,
                        Tags = post.Tags
                    };
                    return View(model);
                }
            }
            return View();

        }

        [AllowAnonymous]
        [Route("showpost/{category}/")]
        [Route("showpost/{category}/{page:int}")]
        public ActionResult ShowAllInCategory(string category, int page = 1)
        {
            if (category == null)
            {
                RedirectToAction("Index", "Home");
            }
            int PageSize = 5;
            Category Category = db.Categories.Where(c => c.ShortUrl == category).FirstOrDefault();
            IEnumerable<Post> posts = Category.Posts.OrderByDescending(p => p.PostedOn).Skip((page - 1) * PageSize).Take(PageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = PageSize, TotalItems = Category.Posts.Count() };
            CategoryView model = new CategoryView
            {
                Posts = posts.ToList(),
                CurrCategory = Category,
                PageInfo = pageInfo
            };

            return View(model);
        }

        private string Slug(string str)
        {
            string patern = "[^a-z0-9_]";
            return Regex.Replace(str, patern, String.Empty);
        }

        [Authorize]
        [Route("post/CatTagChange/Create/{type}/{name}/")]
        [Route("post/CatTagChange/Create/{type}/{name}/{description}")]
        //Create category and tag
        public async Task<ActionResult> CreateCatTag(string type, string name, string description = "")
        {
            if (type.ToLower() == "category")
            {
                using (db)
                {
                    Category cat = new Category
                    {
                        Name = name,
                        Description = description,
                        ShortUrl = Slug(name.ToLower().Replace(" ", "_"))
                    };
                    if (db.Categories.Where(c => c.Name == name).FirstOrDefault() == null)
                    {
                        db.Categories.Add(cat);
                        await db.SaveChangesAsync();
                    }
                    return RedirectToAction("Category", "Post");
                }
            }
            else if (type.ToLower() == "tag")
            {
                using (db)
                {
                    Tag tag = new Tag
                    {
                        Name = name,
                        Description = description
                    };
                    if (db.Tags.Where(t => t.Name == name).FirstOrDefault() == null)
                    {
                        db.Tags.Add(tag);
                        await db.SaveChangesAsync();
                    }
                    return RedirectToAction("Tag", "Post");
                }
            }
            else
            {
                return HttpNotFound();
            }
        }
        [Authorize]
        [Route("post/CatTagChange/Delete/{type}/{id}")]
        //Delete category and tag
        public async Task<ActionResult> DeleteCatTag(string type, int id)
        {
            if (type.ToLower() == "category")
            {
                using (db)
                {
                    Category cat = db.Categories.Where(c => c.Id == id).FirstOrDefault();
                    if (cat != null)
                    {
                        db.Categories.Remove(cat);
                        await db.SaveChangesAsync();
                    }

                }
                return RedirectToAction("Category", "Post");
            }
            else if (type.ToLower() == "tag")
            {
                using (db)
                {
                    Tag tag = db.Tags.Where(t => t.Id == id).FirstOrDefault();
                    if (tag != null)
                    {
                        db.Tags.Remove(tag);
                        await db.SaveChangesAsync();
                    }
                    return RedirectToAction("Tag", "Post");
                }
            }
            else
            {
                return HttpNotFound();
            }
        }
        [Authorize]
        [Route("post/CatTagChange/Change/{type}/{id}/{NewName}/")]
        [Route("post/CatTagChange/Change/{type}/{id}/{NewName}/{NewDescription}")]
        //Change category and tag
        public async Task<ActionResult> ChangeCatTag(string type, int id, string NewName, string NewDescription)
        {
            if (type.ToLower() == "category")
            {
                using (db)
                {
                    var cat = db.Categories.Where(c => c.Id == id).FirstOrDefault();
                    if (cat != null)
                    {
                        cat.Name = NewName;
                        cat.Description = NewDescription;
                        cat.ShortUrl = Slug(NewName.ToLower().Replace(" ", "_"));

                        await db.SaveChangesAsync();
                        return RedirectToAction("Category", "Post");
                    }
                }
            }
            else if (type.ToLower() == "tag")
            {
                var tag = db.Tags.Where(t => t.Id == id).FirstOrDefault();
                if (tag!=null)  
                {
                    tag.Name = NewName;
                    tag.Description = NewDescription;

                    await db.SaveChangesAsync();
                    return RedirectToAction("Tag", "Post");
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}