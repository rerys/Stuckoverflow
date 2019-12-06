using System;

namespace prid1920_g01.Models
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime Timestamp { get; set; }
        public int UserId { get; set; }
        public UserDTO User { get; set; }
        public int PostId { get; set; }
        public PostDTO Post { get; set; }
    }
}