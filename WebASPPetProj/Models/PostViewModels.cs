using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebASPPetProj.Controllers;

namespace WebASPPetProj.Models
{
    //View models for:
    //Create post page
    public class PostCreate
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public List<int> TagsId { get; set; }
    }
    //Category create
    public class CategoriesView
    {
        public IEnumerable<Category> Categories { get; set; }
        public PageInfo pageInfo { get; set; }
    }
    //Tags create
    public class TagsView
    {
        public IEnumerable<Tag> Tags { get; set; }
        public PageInfo pageInfo { get; set; }
    }
    //Index page, show all post view
    public class MainView
    {
        public List<Post> Posts { get; set; }
        public PageInfo PageInfo { get; set; }
    }
    public class PostView
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public List<Tag> Tags { get; set; }
        public DateTime PostedOn { get; set; }
        public ApplicationUser Publisher { get; set; }
    }
    //Posts in category
    public class CategoryView
    {
        public List<Post> Posts { get; set; }
        public Category CurrCategory { get; set; }
        public PageInfo PageInfo { get; set; }
    }
    
}