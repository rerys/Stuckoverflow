using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;


namespace prid1920_g01.Models
{

    public class Tag
    {

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Required Name")]
        public string Name { get; set; }
        public virtual IList<PostTag> PostTags { get; set; } = new List<PostTag>();

        [NotMapped]
        public IEnumerable<Post> Posts { get => PostTags.Select(t => t.Post); }

    }
}