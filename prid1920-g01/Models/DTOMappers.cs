using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prid1920_g01.Models {

    public static class DTOMappers {

        public static UserDTO ToDTO(this User user) {

            return new UserDTO {

                Id = user.Id,

                Pseudo = user.Pseudo,

                Email = user.Email,

                LastName = user.LastName,

                FirstName = user.FirstName,

                BirthDate = user.BirthDate,

                Reputation = user.Reputation,
                
                Role = user.Role,

            };

        }

        public static List<UserDTO> ToDTO(this IEnumerable<User> users) {

            return users.Select(u => u.ToDTO()).ToList();

        }

    }

}


