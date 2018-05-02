using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebASPPetProj.Models
{
    public class PostCreate
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public List<int> TagsId { get; set; }
    }
    public class TagCatCreate
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class MainView
    {
        public List<Category> Categories { get; set; }
        public List<Post> Posts { get; set; }
    }
}