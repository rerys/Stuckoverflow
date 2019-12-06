using System;
namespace prid1920_g01.Models
{

    public class VoteDTO
    {
        public int UpDown { get; set; }
        public int UserId { get; set; }
        public UserDTO User { get; set; }
        public int PostId { get; set; }
        public PostDTO Post { get; set; }

    }

}