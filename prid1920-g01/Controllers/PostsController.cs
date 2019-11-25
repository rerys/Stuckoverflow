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
    public class PostsController : ControllerBase
    {
        private readonly Prid1920_g01Context _context;
        public PostsController(Prid1920_g01Context context)
        {
            _context = context;
        }

        //GET = Liste tous les posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetAll()
        {
            return (await _context.Posts.ToListAsync()).ToDTO();
        }

        //GET = un seul post CA MARCHE PAS 
        [HttpGet("{title}")]
        public async Task<ActionResult<PostDTO>> GetOne(string title)
        {
            var post = await _context.Posts.Where(u => u.Title == title).SingleOrDefaultAsync();
            if (post == null)
                return NotFound();
            return post.ToDTO();
        }

        //POST = créer un nouveau post
        [HttpPost]
        public async Task<ActionResult<PostDTO>> PostPost(PostDTO data)
        {

            var newPost = new Post()
            {
                Title = data.Title,
                Body = data.Body,
            };
            _context.Posts.Add(newPost);
            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty)
                return BadRequest(res);
            return CreatedAtAction(nameof(GetOne), new { Title = newPost.Title }, newPost.ToDTO());

        }


        //PUT = modifie un post
        [HttpPut("{title}")]
        public async Task<IActionResult> PutPost(string title, PostDTO postDTO)
        {

            if (title != postDTO.Title)
                return BadRequest();
            var post = await _context.Posts.Where(u => u.Title == title).SingleOrDefaultAsync();
            if (post == null)
                return NotFound();
            post.Title = postDTO.Title;
            post.Body = postDTO.Body;
            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty)
                return BadRequest(res);
            return NoContent();
        }


        //DELETE = supprime un post
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
                return NotFound();
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}