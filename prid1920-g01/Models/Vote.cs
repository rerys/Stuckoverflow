using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace prid1920_g01.Models
{

    public class Vote : IValidatableObject
    {
        [Required(ErrorMessage = "Required Vote")]
        public int UpDown { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Required User")]
        public virtual User User { get; set; }
        public int PostId { get; set; }
        [Required(ErrorMessage = "Required Post")]
        public virtual Post Post { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UpDown != -1 || UpDown != 1)
            {
                yield return new ValidationResult("Vote only accept -1 or 1", new[] { nameof(UpDown) });
            }
            if (UpDown == 1 && User.Reputation < 15)
            {
                yield return new ValidationResult("To vote up, you must have at least a reputation of 15", new[] { nameof(User) });
            }
            if (UpDown == -1 && User.Reputation < 30)
            {
                yield return new ValidationResult("To vote down, you must have at least a reputation of 30", new[] { nameof(User) });
            }
        }
    }

}