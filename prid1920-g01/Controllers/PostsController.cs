using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using prid1920_g01.Models;
using PRID_Framework;
using Microsoft.AspNetCore.Authorization;
using System;

namespace prid1920_g01.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly Prid1920_g01Context _context;
        public PostsController(Prid1920_g01Context context)
        {
            _context = context;
        }

        //Lists all the posts.
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetAll()
        {
            var posts = await _context.Posts.Where(p => p.Title != null).ToListAsync();
            if (posts == null) { return NoContent(); }
            return posts.ToDTO();
        }

        //Create new Post
        [HttpPost]
        public async Task<ActionResult<PostDTO>> PostPost(PostDTO data)
        {

            var newPost = data.ToOBJ();
            _context.Posts.Add(newPost);
            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty)
                return BadRequest(res);
            return CreatedAtAction(nameof(GetAll), newPost.ToDTO());
        }


        //Update a post
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, PostDTO postDTO)
        {
            var connectedUser = await _context.Users.FindAsync(User.Identity.Name);
            if (postDTO.User.Id != connectedUser.Id) { return BadRequest(); }
            if (id != postDTO.Id) { return BadRequest(); }
            var post = await _context.Posts.Where(p => p.Id == id).SingleOrDefaultAsync();
            if (post == null)
                return NotFound();
            post.Title = postDTO.Title;
            post.Body = postDTO.Body;
            post.Timestamp = DateTime.Now;
            post.User = postDTO.User.ToOBJ();
            post.Tags = postDTO.Tags.ToOBJ();
            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty)
                return BadRequest(res);
            return NoContent();
        }


        //Delete a post
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            var connectedUser = await _context.Users.FindAsync(User.Identity.Name);
            if (post.User.Id != connectedUser.Id) { return BadRequest(); }
            if (post == null)
                return NotFound();
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}