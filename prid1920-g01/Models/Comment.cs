using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace prid1920_g01.Models
{

    public class Comment
    {

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Required Body")]
        public string Body { get; set; }
        [Required(ErrorMessage = "Required Timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Required User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        [Required(ErrorMessage = "Required Post")]
        public int PostId { get; set; }
        public virtual Post Post { get; set; }

    }
}