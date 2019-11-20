using System;
namespace prid1920_g01.Models
{

    public class VoteDTO
    {

        public DateTime Timestamp { get; set; }
        public int UpDown { get; set; }
        public string User { get; set; }
        public string Post { get; set; }

    }

}