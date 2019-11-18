using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace prid1920_g01.Models
{
    public enum Role
    {
        Admin = 2, Manager = 1, Member = 0
    }
    public class User : IValidatableObject
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression("^[a-zA-Z][a-zA-Z0-9_]*$", ErrorMessage = "Characters are not allowed.")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Remark must have min length of 3 and max Length of 10")]
        public string Pseudo { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Remark must have min length of 3 and max Length of 10")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Remark must have min length of 3 and max Length of 50")]
        public string LastName { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Remark must have min length of 3 and max Length of 50")]
        public string FirstName { get; set; }

        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int Reputation { get; set; }
        public Role Role { get; set; } = Role.Member;
        [NotMapped]
        public string Token { get; set; }

        public int? Age
        {
            get
            {
                if (!BirthDate.HasValue)
                    return null;
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Value.Year;
                if (BirthDate.Value.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Prid1920_g01Context currContext = validationContext.GetService(typeof(DbContext)) as Prid1920_g01Context;
            Debug.Assert(currContext != null);
            if (BirthDate.HasValue && BirthDate.Value.Date > DateTime.Today)
                yield return new ValidationResult("Can't be born in the future in this reality", new[] { nameof(BirthDate) });
            else if (Age.HasValue && Age < 18)
                yield return new ValidationResult("Must be 18 years old", new[] { nameof(BirthDate) });

            if (LastName == null && FirstName != null)
            {
                yield return new ValidationResult("you must have a LastName", new[] { nameof(LastName) });
            }
            if (FirstName == null && LastName != null)
            {
                yield return new ValidationResult("you must have a FirstName", new[] { nameof(FirstName) });
            }
            var existPseudo = (from u in currContext.Users where Id != u.Id && u.Pseudo == this.Pseudo select u).SingleOrDefault();
            if (existPseudo != null)
            {
                yield return new ValidationResult("Pseudo already used", new[] { nameof(Pseudo) });
            }
            var existEmail = (from u in currContext.Users where Id != u.Id && u.Email == this.Email select u).SingleOrDefault();
            if (existEmail != null)
            {
                yield return new ValidationResult("Email already used", new[] { nameof(Email) });
            }

        }
    }
}