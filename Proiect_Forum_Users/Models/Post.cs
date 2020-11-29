using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Proiect_Forum.Models
{
    public class Post
    {
        [Key]
        [Column(Order = 1)]
        public int PostId { get; set; }

        [Required(ErrorMessage = "Content is mandatory!")]
        public string Content { get; set; }

        public DateTime Date { get; set; }

        [Key]
        [Column(Order = 2)]
        public int TopicId { get; set; }

        public virtual Topic Topic { get; set; }
    }
}