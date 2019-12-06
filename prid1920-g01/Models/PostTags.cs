using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prid1920_g01.Models
{

    public class PostTag
    {


        public int PostId { get; set; }
        public virtual Post Post { get; set; }
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }

    }
}