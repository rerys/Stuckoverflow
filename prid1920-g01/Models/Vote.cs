using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
        public Post Post { get; set; }



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