using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
    public class CommentsController : ControllerBase
    {

        private readonly Prid1920_g01Context _context;

        public CommentsController(Prid1920_g01Context context)
        {
            _context = context;

        }


        //Create un nouveau comment
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostComment(CommentDTO data)
        {
            //Création d'un object comment depuis le DTO
            var newComment = data.ToOBJ();
            //Ajout du comment dans le context 
            _context.Comments.Add(newComment);
            //Sauvegarde de l'ajout
            var res = await _context.SaveChangesAsyncWithValidation();
            //Contrôle de la sauvegarde 
            if (!res.IsEmpty) { return BadRequest(res); }
            return NoContent();
            //return CreatedAtAction(nameof(GetOne), new { pseudo = newUser.Pseudo }, newUser.ToDTO());

        }


        //Update d'un comment
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, CommentDTO commentDTO, UserDTO currentUser)
        {
            //Contrôle comment appartient à l'user
            if (commentDTO.User.Id != currentUser.Id) { return BadRequest(); }
            //Contrôle commentaire appartient à l'ID du commentaire recherché 
            if (id != commentDTO.Id) { return BadRequest(); }
            //récuperation du commentaire dans le context 
            var comment = await _context.Comments.Where(c => c.Id == id).SingleOrDefaultAsync();
            //Contrôle résultat du context 
            if (comment == null) { return NotFound(); }
            //Ajout de la modification
            comment.Body = commentDTO.Body;
            comment.Timestamp = DateTime.Now;
            //Sauvegarde dans le context
            var res = await _context.SaveChangesAsyncWithValidation();
            //Contrôle sauvegarde
            if (!res.IsEmpty) { return BadRequest(res); }
            //end
            return NoContent();
        }

        //Delete d'un comment 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id, UserDTO currentUser)
        {
            //récuperation du commentaire dans le context 
            var comment = await _context.Comments.FindAsync(id);
            //Contrôle résultat du context 
            if (comment == null) { return NotFound(); }
            //Contrôle comment appartient à l'user
            if (currentUser.Id != comment.User.Id) { return BadRequest(); }
            //Suppression du comment dans le context
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            //end 
            return NoContent();
        }


    }

}