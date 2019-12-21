using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using prid1920_g01.Models;
using PRID_Framework;
using Microsoft.AspNetCore.Authorization;

namespace prid1920_g01.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VotesController : ControllerBase
    {

        private readonly Prid1920_g01Context _context;
        private static int WINVOTEUP = 10;
        private static int LOSSVOTEDOWN = 2;
        private static int LOSSVOTEDOWNVOTER = 1;

        public VotesController(Prid1920_g01Context context)
        {
            _context = context;

        }


        //Create un nouveau vote
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostVote(VoteDTO data)
        {
            var connectedUser = await _context.Users.Where(u => u.Pseudo == User.Identity.Name).SingleOrDefaultAsync();
            var existVote = await _context.Votes.Where(v => v.PostId == data.PostId && v.UserId == connectedUser.Id).SingleOrDefaultAsync();
            if (existVote != null) return BadRequest();
            var vote = new Vote
            {
                UpDown = data.UpDown,
                UserId = connectedUser.Id,
                PostId = data.PostId
            };
            _context.Votes.Add(vote);
            var res = await _context.SaveChangesAsyncWithValidation();
            //recuperation du nouveau vote en DB
            //var newVote = await _context.Votes.Where(v => v.PostId == data.PostId && v.UserId == connectedUser.Id).SingleOrDefaultAsync();
            //recuperation du l'user du post 
            //var poster = await _context.Users.Where(u => u.Id == newVote.UserId).SingleOrDefaultAsync();
            //setReputation(newVote, poster, connectedUser);
            //res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty) { return BadRequest(res); }
            return NoContent();
            //return CreatedAtAction(nameof(GetOne), new { pseudo = newUser.Pseudo }, newUser.ToDTO());

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVote(int id)
        {
            var connectedUser = await _context.Users.Where(u => u.Pseudo == User.Identity.Name).SingleOrDefaultAsync();
            var existVote = await _context.Votes.Where(v => v.PostId == id && v.User.Pseudo == User.Identity.Name).SingleOrDefaultAsync();
            if (existVote == null) return NotFound();
            if (existVote.UserId != connectedUser.Id) return BadRequest();
            //var poster = await _context.Users.Where(u => u.Id == existVote.UserId).SingleOrDefaultAsync();
            //restorReputation(existVote, poster, connectedUser);
            _context.Votes.Remove(existVote);
            await _context.SaveChangesAsync();
            return NoContent();
        }

 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVote(int id, VoteDTO data)
        {
            var connectedUser = await _context.Users.Where(u => u.Pseudo == User.Identity.Name).SingleOrDefaultAsync();
            var existVote = await _context.Votes.Where(v => v.PostId == data.PostId && v.UserId == data.UserId).SingleOrDefaultAsync();
            if (existVote == null) return NotFound();
            if (existVote.User.Id != connectedUser.Id) return BadRequest();
            if (existVote.UpDown != data.UpDown) 
            {
                //var poster = await _context.Users.Where(u => u.Id == existVote.UserId).SingleOrDefaultAsync();
                //restorReputation(existVote, poster, connectedUser);
                existVote.UpDown = data.UpDown;
                //setReputation(existVote, poster, connectedUser);
                var res = await _context.SaveChangesAsyncWithValidation();
                if (!res.IsEmpty)
                    return BadRequest(res);

            }
            return NoContent();
        }


        private void setReputation(Vote vote, User poster, User votant)
        {
            if (vote.UpDown == 1)
            {
                poster.Reputation += WINVOTEUP;
            }
            else
            {
                poster.Reputation -= LOSSVOTEDOWN;
                votant.Reputation -= LOSSVOTEDOWNVOTER;
            }

        }

        private void restorReputation(Vote vote, User poster, User votant)
        {
            if (vote.UpDown == 1)
            {
                poster.Reputation -= WINVOTEUP;

            }
            else
            {
                poster.Reputation += LOSSVOTEDOWN;
                votant.Reputation += LOSSVOTEDOWNVOTER;
            }


        }

    }
}