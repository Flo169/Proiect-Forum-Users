using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_Forum.Models
{
    public class Topic
    {
        [Key]
        public int TopicId { get; set; }

        [Required(ErrorMessage = "Title is mandatory!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is mandatory!")]
        public string Content { get; set; }

        public DateTime Date { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categ { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual Category Category { get; set; }
    }
}