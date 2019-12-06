namespace prid1920_g01.Models
{

    public class PostTagDTO
    {
        public int PostId { get; set; }
        public PostDTO Post { get; set; }
        public int TagId { get; set; }
        public TagDTO Tag { get; set; }
    }
}