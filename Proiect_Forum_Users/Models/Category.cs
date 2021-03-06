﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_Forum.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Title is mandatory!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is mandatory!")]
        public string Description { get; set; }

        public virtual ICollection<Topic> Topics { get; set; }
    }
}