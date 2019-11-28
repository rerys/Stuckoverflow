using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace prid1920_g01.Models
{

    public class Post : IValidatableObject
    {

        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        [Required(ErrorMessage = "Must have a content")]
        public string Body { get; set; }
        [Required(ErrorMessage = "Must have a timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.Now;
        [NotMapped]
        public virtual Post Parent { get; set; }
        public virtual IList<Post> Responses { get; set; } = new List<Post>();
        public virtual Post Accpeted { get; set; }
        [Required(ErrorMessage = "Must have an user")]
        public virtual User User { get; set; }
        [NotMapped]
        public virtual IList<Tag> Tags { get; set; } = new List<Tag>();
        public virtual IList<Vote> Votes { get; set; } = new List<Vote>();
        public virtual IList<Comment> Comments { get; set; } = new List<Comment>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Parent == null && Title == null)
            {
                yield return new ValidationResult("Must have a title", new[] { nameof(Title) });
            }
            if (Parent != null && Title != null)
            {
                yield return new ValidationResult("Mustn't have a title", new[] { nameof(Title) });
            }
            if (Parent != null && Responses != null)
            {
                yield return new ValidationResult("An answer can not have an answer", new[] { nameof(Responses) });
            }
            if (Accpeted != null && Parent == null)
            {
                yield return new ValidationResult("A question cannot be accepted", new[] { nameof(Accpeted) });
            }
            if (Accpeted != null && (from r in Responses where r.Accpeted != null select r).SingleOrDefault() != null)
            {
                yield return new ValidationResult("There is already an accepted answer", new[] { nameof(Accpeted) });
            }

        }
    }
}