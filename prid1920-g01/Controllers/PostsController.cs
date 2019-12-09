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
        [HttpGet("{filter}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetAll(string filter = "")
        {
            var posts = await _context.Posts.Where(p => p.Title != null && (p.Title.Contains(filter) || p.Body.Contains(filter))).ToListAsync();
            if (posts == null) { return NoContent(); }
            return posts.ToDTO();
        }

        [AllowAnonymous]
        [HttpGet("newest")]
        [HttpGet("newest/{filter}")]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetNewest(string filter = "")
        {
            var posts = await _context.Posts.Where(p => p.Title != null && (p.Title.Contains(filter) || p.Body.Contains(filter))).ToListAsync();
            posts = posts.OrderByDescending(p => p.Timestamp).ToList();
            if (posts == null) { return NoContent(); }
            return posts.ToDTO();
        }

        [AllowAnonymous]
        [HttpGet("votes")]
        [HttpGet("votes/{filter}")]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetVotes(string filter = "")
        {
            var posts = await _context.Posts.Where(p => p.Title != null && (p.Title.Contains(filter) || p.Body.Contains(filter))).ToListAsync();
            posts = posts.OrderByDescending(p => p.Votes.Count()).ToList();
            if (posts == null) { return NoContent(); }
            return posts.ToDTO();
        }

        [AllowAnonymous]
        [HttpGet("unanswered")]
        [HttpGet("unanswered/{filter}")]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetUnanswered(string filter)
        {
            var posts = await _context.Posts.Where(p => p.Title != null && p.Responses.Count() == 0 && (p.Title.Contains(filter) || p.Body.Contains(filter))).ToListAsync();
            if (posts == null) { return NoContent(); }
            return posts.ToDTO();
        }

        [AllowAnonymous]
        [HttpGet("tags")]
        [HttpGet("tags/{filter}")]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetTags(string filter)
        {
            var posts = await _context.Posts.Where(p => p.Tags == null && (p.Title.Contains(filter) || p.Body.Contains(filter))).ToListAsync();
            posts = posts.OrderByDescending(p => p.Tags.Count()).ToList();
            if (posts == null) { return NoContent(); }
            return posts.ToDTO();
        }

        [AllowAnonymous]
        [HttpGet("question/{id}")]
        public async Task<ActionResult<PostDTO>> GetQuestion(int id)
        {
            var post = await _context.Posts.Where(p => p.Id == id).SingleOrDefaultAsync();
            if (post == null) { return NotFound(); }
            return post.ToDTO();
        }

        //Create new Post
        [HttpPost]
        public async Task<ActionResult<PostDTO>> PostPost(PostDTO data)
        {

            var user = await _context.Users.Where(u => u.Pseudo == User.Identity.Name).SingleOrDefaultAsync();
            var newPost = data.ToOBJ();
            newPost.UserId = user.Id;
            _context.Posts.Add(newPost);
            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty)
                return BadRequest(res);
            return CreatedAtAction(nameof(GetAll), newPost.ToDTO());
        }

        [HttpPost("{idParent}")]
        public async Task<ActionResult<PostDTO>> PostReply([FromRoute]int idParent, [FromBody]PostDTO data)
        {

            var user = await _context.Users.Where(u => u.Pseudo == User.Identity.Name).SingleOrDefaultAsync();
            var post = await _context.Posts.Where(p => p.Id == idParent && p.ParentId == null).SingleOrDefaultAsync();
            if(user == null || post == null) return BadRequest();

            var newPost = data.ToOBJ(); 
            newPost.UserId = user.Id;
            //newPost.User = user;
            newPost.ParentId = idParent;
            //newPost.Parent = post;
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
            // post.Tags = postDTO.Tags.ToOBJ();
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