using System;
namespace prid1920_g01.Models
{

    public class VoteDTO
    {

        public DateTime Timestamp { get; set; }
        public int UpDown { get; set; }
        public UserDTO User { get; set; }
        public PostDTO Post { get; set; }

    }

}