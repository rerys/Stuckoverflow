using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace prid1920_g01.Models
{

    public class Comment : IValidatableObject
    { 

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Required Body")]
        public string Body { get; set; }
        [Required(ErrorMessage = "Required Timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Required User")]
        public virtual User User { get; set; }
        [Required(ErrorMessage = "Required Post")]
        public virtual Post Post { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Timestamp != DateTime.Now)
            {
                yield return new ValidationResult("Bad DateTime", new[] { nameof(Timestamp) });
            }
        }
    }
}