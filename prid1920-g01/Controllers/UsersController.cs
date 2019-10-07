using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using prid1920_g01.Models;
using System.ComponentModel.DataAnnotations;
using PRID_Framework;

namespace prid1920_g01.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Prid1920_g01Context _context;

        public UsersController(Prid1920_g01Context context)
        {
            _context = context;

            if (_context.Users.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.Users.Add(new User { Pseudo = "Benito", Password = "Ben", Email = "Ben@epfc.eu", LastName = "Penelle", FirstName = "Ben", Reputation = 1 });
                _context.SaveChanges();
            }
        }


        //List
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            return (await _context.Users.ToListAsync()).ToDTO();
        }

        //Read
        [HttpGet("{pseudo}")]
        public async Task<ActionResult<UserDTO>> GetOne(string pseudo)
        {

            var user = await _context.Users.Where(u => u.Pseudo == pseudo).SingleOrDefaultAsync();

            if (user == null)

                return NotFound();

            return user.ToDTO();

        }


        //Create
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO data)
        {

            var newUser = new User()
            {

                Pseudo = data.Pseudo,

                Password = data.Password,

                Email = data.Email,

                LastName = data.LastName,

                FirstName = data.FirstName,

                BirthDate = data.BirthDate,

                Reputation = data.Reputation,

            };

            _context.Users.Add(newUser);

            var res = await _context.SaveChangesAsyncWithValidation();

            if (!res.IsEmpty)

                return BadRequest(res);

            return CreatedAtAction(nameof(GetOne), new { pseudo = newUser.Pseudo }, newUser.ToDTO());

        }


        //Update
        [HttpPut("{pseudo}")]
        public async Task<IActionResult> PutUser(string pseudo, UserDTO userDTO)
        {

            if (pseudo != userDTO.Pseudo)

                return BadRequest();

            var user = await _context.Users.Where(u => u.Pseudo==pseudo).SingleOrDefaultAsync();

            if (user == null)

                return NotFound();

            user.Pseudo = userDTO.Pseudo;

            user.Email = userDTO.Email;

            user.LastName = userDTO.LastName;

            user.FirstName = userDTO.FirstName;

            user.BirthDate = userDTO.BirthDate;

            user.Reputation = userDTO.Reputation;

            var res = await _context.SaveChangesAsyncWithValidation();

            if (!res.IsEmpty)

                return BadRequest(res);

            return NoContent();

        }


        //delete
        [HttpDelete("{pseudo}")]
        public async Task<IActionResult> DeleteUser(string pseudo)
        {
            var user = await _context.Users.FindAsync(pseudo);

            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}