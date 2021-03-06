﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Raptor.Data.Models.Blog
{
    public class BlogPostCategory
    {
        [Key]
        public int PostCategoryId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Slug { get; set; }

        public string Description { get; set; }
        public int ParentId { get; set; }

        public virtual ICollection<BlogPost> Posts { get; set; }
    }
}
