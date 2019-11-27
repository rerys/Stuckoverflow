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
            //Création d'un object vote depuis le DTO
            var newVote = data.ToOBJ();
            //Ajout du vote dans le context 
            _context.Votes.Add(newVote);
            var questUser = newVote.Post.User;
            //Gestion de la reputation 
            if (newVote.UpDown == 1)
            {
                questUser.Reputation += WINVOTEUP;
            }
            else
            {
                questUser.Reputation -= LOSSVOTEDOWN;
                newVote.User.Reputation -= LOSSVOTEDOWNVOTER;
            }
            //Sauvegarde de l'ajout
            var res = await _context.SaveChangesAsyncWithValidation();
            //Contrôle de la sauvegarde 
            if (!res.IsEmpty) { return BadRequest(res); }
            return NoContent();
            //return CreatedAtAction(nameof(GetOne), new { pseudo = newUser.Pseudo }, newUser.ToDTO());

        }


        //Update d'un vote
        [HttpPut("{userId, postId}")]
        public async Task<IActionResult> PutVote(int userId, int postId, VoteDTO voteDTO)
        {
            var currentUser = await _context.Users.FindAsync(User.Identity.Name);
            //Contrôle vote appartient à l'user
            if (voteDTO.User.Id != currentUser.Id) { return BadRequest(); }
            //Contrôle vote appartient à l'ID du user recherché 
            if (userId != voteDTO.User.Id) { return BadRequest(); }
            //Contrôle vote appartient à l'ID du post recherché 
            if (postId != voteDTO.Post.Id) { return BadRequest(); }
            //récuperation du commentaire dans le context 
            var vote = await _context.Votes.Where(v => v.UserId == userId && v.PostId == postId).SingleOrDefaultAsync();
            //Contrôle résultat du context 
            if (vote == null) { return NotFound(); }
            //Contrôle du changement 
            if (voteDTO.UpDown == vote.UpDown) { return NoContent(); }
            //Ajout de la modification
            if(vote.UpDown == 1 && voteDTO.UpDown == -1){
                vote.Post.User.Reputation += (WINVOTEUP - LOSSVOTEDOWN);
            }else if(vote.UpDown == -1 && voteDTO.UpDown == 1){
                vote.Post.User.Reputation += (WINVOTEUP + LOSSVOTEDOWN);
                vote.User.Reputation += LOSSVOTEDOWNVOTER;
            }
            //Sauvegarde dans le context
            var res = await _context.SaveChangesAsyncWithValidation();
            //Contrôle sauvegarde
            if (!res.IsEmpty) { return BadRequest(res); }
            //end
            return NoContent();
        }


        //Delete d'un vote
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVote(int id)
        {
            //récuperation du vote dans le context 
            var vote = await _context.Votes.FindAsync(id);
            var currentUser = await _context.Users.FindAsync(User.Identity.Name);
            //Contrôle résultat du context 
            if (vote == null) { return NotFound(); }
            //Contrôle vote appartient à l'user
            if (currentUser.Id != vote.User.Id) { return BadRequest(); }
            //gestion de la reputation
            var questUser = vote.Post.User;
            if (vote.UpDown == 1)
            {
                questUser.Reputation -= WINVOTEUP;
            }
            else
            {
                questUser.Reputation += LOSSVOTEDOWN;
                vote.User.Reputation += LOSSVOTEDOWNVOTER;
            }
            //Suppression du comment dans le context
            _context.Votes.Remove(vote);
            await _context.SaveChangesAsync();
            //end 
            return NoContent();
        }



    }
}