using System.Collections.Generic;

namespace prid1920_g01.Models
{
    public class TagDTO
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Posts { get; set; }
    }
}