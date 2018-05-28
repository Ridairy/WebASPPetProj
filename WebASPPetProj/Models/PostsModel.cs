using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebASPPetProj.Models
{ 
    //CodeFirst model for:
    public class Post
    {
        [Key,Required]
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public bool Posted { get; set; }
        public DateTime PostedOn { get; set; }
        public virtual ApplicationUser Publisher { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<Tag> Tags { get; set; }
        
        public string ShortUrl { get; set; }
    }

    public class Category
    {
        [Key,Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<Post> Posts { get; set; }

        public string ShortUrl { get; set; }
    }

    public class Tag
    {
        [Key,Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<Post> Posts { get; set; }

       // public string ShortUrl { get; set; }
    }
}