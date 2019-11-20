using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace prid1920_g01.Models
{

    public class Vote : IValidatableObject
    {
        [Required(ErrorMessage = "Required DateTime")]
        public DateTime Timestamp { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Required Vote")]
        public int UpDown { get; set; }
        [Required(ErrorMessage = "Required User")]
        public virtual User User { get; set; }
        [Required(ErrorMessage = "Required Post")]
        public virtual Post Post { get; set; }

        [NotMapped]
        public int UserId
        {
            get
            {return User.Id;}
            set{ UserId = value;}
        }

        [NotMapped]
        public int PostId
        {
            get { return Post.Id; }
            set { PostId = value; }

        }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UpDown != -1 || UpDown != 1)
            {
                yield return new ValidationResult("Vote only accept -1 or 1", new[] { nameof(UpDown) });
            }
            if (Timestamp != DateTime.Now)
            {
                yield return new ValidationResult("Bad DateTime", new[] { nameof(Timestamp) });
            }

        }
    }

}