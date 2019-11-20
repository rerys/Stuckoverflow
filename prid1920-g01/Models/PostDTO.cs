using System;
using System.Collections.Generic;

namespace prid1920_g01.Models
{

    public class PostDTO
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Timestamp { get; set; }
        public string User { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<string> Votes { get; set; }
        public IEnumerable<string> Comments { get; set; }


    }

}