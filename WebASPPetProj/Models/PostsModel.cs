using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebASPPetProj.Models
{
    public class Post
    {
        [Key,Required]
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public bool Posted { get; set; }
        public DateTime PostedOn { get; set; }
        public ApplicationUser Publisher { get; set; }
        public Category Category { get; set; }
        public List<Tag> Tags { get; set; }

        public string ShortUrl { get; set; }
    }

    public class Category
    {
        [Key,Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Post> Posts { get; set; }

        public string ShortUrl { get; set; }
    }

    public class Tag
    {
        [Key,Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Post> Posts { get; set; }

       // public string ShortUrl { get; set; }
    }
}