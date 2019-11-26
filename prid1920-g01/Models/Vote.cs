using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace prid1920_g01.Models
{

    public class Vote : IValidatableObject
    {
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
            if(UpDown == 1 && User.Reputation < 15){
                yield return new ValidationResult("To vote up, you must have at least a reputation of 15", new[] { nameof(User) });
            }
            if(UpDown == -1 && User.Reputation < 30){
                yield return new ValidationResult("To vote down, you must have at least a reputation of 30", new[] { nameof(User) });
            }
        }
    }

}