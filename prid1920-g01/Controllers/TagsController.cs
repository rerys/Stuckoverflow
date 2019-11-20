using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using prid1920_g01.Models;
using PRID_Framework;
using Microsoft.AspNetCore.Authorization;
using prid1920_g01.Helpers;

namespace prid1920_g01.Controllers
{ 

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly Prid1920_g01Context _context;

        public TagsController(Prid1920_g01Context context)
        {
            _context = context;

        }


        //méthode GET
        //retourne une liste de tous les Tags
        [Authorized(Role.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDTO>>> GetAll()
        {
            return (await _context.Tags.ToListAsync()).ToDTO();
        }


        //méthode GET
        //retourne un Tag par son nom 
        //paramètre string "name" 
        [HttpGet("{name}")]
        public async Task<ActionResult<TagDTO>> GetOne(string name)
        {
            var tag = await _context.Tags.Where(t => t.Name == name).SingleOrDefaultAsync();
            if (tag == null)
                return NotFound();
            return tag.ToDTO();
        }

        //méthode POST
        //Créer un nouveau Tag
        //paramètre TagDTO
        [Authorized(Role.Admin)]
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostTag(TagDTO data)
        {

            var newTag = data.ToOBJ();
            _context.Tags.Add(newTag);

            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty) return BadRequest(res);
            return CreatedAtAction(nameof(GetOne), new { pseudo = newTag.Name }, newTag.ToDTO());

        }

        //méthode PUT
        //modifie un tag 
        //paramètres Id + TagDTO
        [Authorized(Role.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTag(int id, TagDTO tagDTO)
        {

            if (id != tagDTO.Id) return BadRequest();
            var tag = await _context.Tags.Where(t => t.Id == id).SingleOrDefaultAsync();
            if (tag == null) return NotFound();

            tag.Name = tagDTO.Name;
            tag.Posts = tagDTO.Posts.ToOBJ();

            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty) return BadRequest(res);
            return NoContent();

        }


        //méthode DELETE
        //supprime un TAG
        //paramètres Id 
        [Authorized(Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);

            if (tag == null)
                return NotFound();

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}