namespace prid1920_g01.Models
{
    public class ScoredPost
    {
        public virtual Post Post { get; set; }
        public int MaxScore { get; set; }

    }
}
