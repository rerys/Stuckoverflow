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
        public ActionResult<IEnumerable<PostDTO>> GetAll(string filter = "")
        {

            var q = _context.Posts.Where(p => p.Title != null && (p.Title.Contains(filter) || p.Body.Contains(filter)))
                 .SelectMany(p => p.Votes.DefaultIfEmpty(), (p, v) => new
                 {
                     p.Id,
                     ParentId = p.ParentId == null ? p.Id : p.ParentId,
                     UpDown = v == null ? 0 : v.UpDown,
                 })
                 .GroupBy(pv => new { pv.Id, pv.ParentId })
                 .Select(g => new { g.Key.Id, g.Key.ParentId, Score = g.Sum(pv => pv.UpDown) })
                 .AsEnumerable()
                 .GroupBy(p => p.ParentId)
                 .Select(g => new ScoredPost { Post = _context.Posts.Where(p => p.Id == g.Key).SingleOrDefault(), MaxScore = g.Max(p => p.Score) })
                 ;

            if (q == null) { return NoContent(); }
            return q.ScoredToPost();
        }

        [AllowAnonymous]
        [HttpGet("newest")]
        [HttpGet("newest/{filter}")]
        public ActionResult<IEnumerable<PostDTO>> GetNewest(string filter = "")
        {
            var q = _context.Posts.Where(p => p.Title != null && (p.Title.Contains(filter) || p.Body.Contains(filter)))
                 .SelectMany(p => p.Votes.DefaultIfEmpty(), (p, v) => new
                 {
                     p.Id,
                     ParentId = p.ParentId == null ? p.Id : p.ParentId,
                     UpDown = v == null ? 0 : v.UpDown,
                 })
                 .GroupBy(pv => new { pv.Id, pv.ParentId })
                 .Select(g => new { g.Key.Id, g.Key.ParentId, Score = g.Sum(pv => pv.UpDown) })
                 .AsEnumerable()
                 .GroupBy(p => p.ParentId)
                 .Select(g => new ScoredPost { Post = _context.Posts.Where(p => p.Id == g.Key).SingleOrDefault(), MaxScore = g.Max(p => p.Score) })
                 .OrderByDescending(r => r.Post.Timestamp)
                 ;

            if (q == null) { return NoContent(); }
            return q.ScoredToPost();
        }

        [AllowAnonymous]
        [HttpGet("votes")]
        [HttpGet("votes/{filter}")]
        public ActionResult<IEnumerable<PostDTO>> GetVotes(string filter = "")
        {

            var q = _context.Posts.Where(p => p.Title != null && (p.Title.Contains(filter) || p.Body.Contains(filter)))
                           .SelectMany(p => p.Votes.DefaultIfEmpty(), (p, v) => new
                           {
                               p.Id,
                               ParentId = p.ParentId == null ? p.Id : p.ParentId,
                               UpDown = v == null ? 0 : v.UpDown,
                           })
                           .GroupBy(pv => new { pv.Id, pv.ParentId })
                           .Select(g => new { g.Key.Id, g.Key.ParentId, Score = g.Sum(pv => pv.UpDown) })
                           .AsEnumerable()
                           .GroupBy(p => p.ParentId)
                           .Select(g => new ScoredPost { Post = _context.Posts.Where(p => p.Id == g.Key).SingleOrDefault(), MaxScore = g.Max(p => p.Score) })
                           .OrderByDescending(r => r.MaxScore)
                           ;

            if (q == null) { return NoContent(); }
            return q.ScoredToPost();
        }

        [AllowAnonymous]
        [HttpGet("unanswered")]
        [HttpGet("unanswered/{filter}")]
        public ActionResult<IEnumerable<PostDTO>> GetUnanswered(string filter)
        {
            if (filter == null)
            {
                filter = "";
            }

            var q = _context.Posts.Where(p => (p.Title != null && (p.Title.Contains(filter) || p.Body.Contains(filter))) && (from r in p.Responses select r).Count() == 0)
                .SelectMany(p => p.Votes.DefaultIfEmpty(), (p, v) => new
                {
                    p.Id,
                    ParentId = p.ParentId == null ? p.Id : p.ParentId,
                    UpDown = v == null ? 0 : v.UpDown,
                })
                .GroupBy(pv => new { pv.Id, pv.ParentId })
                .Select(g => new { g.Key.Id, g.Key.ParentId, Score = g.Sum(pv => pv.UpDown) })
                .AsEnumerable()
                .GroupBy(p => p.ParentId)
                .Select(g => new ScoredPost { Post = _context.Posts.Where(p => p.Id == g.Key).SingleOrDefault(), MaxScore = g.Max(p => p.Score) })
                .OrderByDescending(r => r.Post.Timestamp)
                ;

            if (q == null) { return NoContent(); }
            return q.ScoredToPost();
        }

        [AllowAnonymous]
        [HttpGet("tags")]
        [HttpGet("tags/{filter}")]
        public ActionResult<IEnumerable<PostDTO>> GetTags(string filter)
        {
            if (filter == null)
            {
                filter = "";
            }
            var q = _context.Posts.Where(p => p.Title != null).AsEnumerable().Where(p => p.Tags.Count() > 0 &&
            (from t in p.Tags where t.Name.Contains(filter) select t).Count() > 0)
               .SelectMany(p => p.Votes.DefaultIfEmpty(), (p, v) => new
               {
                   p.Id,
                   ParentId = p.ParentId == null ? p.Id : p.ParentId,
                   UpDown = v == null ? 0 : v.UpDown,
               })
               .GroupBy(pv => new { pv.Id, pv.ParentId })
               .Select(g => new { g.Key.Id, g.Key.ParentId, Score = g.Sum(pv => pv.UpDown) })
               .AsEnumerable()
               .GroupBy(p => p.ParentId)
               .Select(g => new ScoredPost { Post = _context.Posts.Where(p => p.Id == g.Key).SingleOrDefault(), MaxScore = g.Max(p => p.Score) })
               .OrderByDescending(r => r.Post.Timestamp)
               ;

            if (q == null) { return NoContent(); }
            return q.ScoredToPost();
        }

        [AllowAnonymous]
        [HttpGet("question/{id}")]
        public async Task<ActionResult<PostDTO>> GetQuestion(int id)
        {
            var post = await _context.Posts.Where(p => p.Id == id).SingleOrDefaultAsync();
            if (post == null) { return NotFound(); }
            return post.ToDTO();
        }

        public async Task<ActionResult<PostDTO>> GetQuestionByBody(string b)
        {
            var post = await _context.Posts.Where(p => p.Body == b).SingleOrDefaultAsync();
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
            return CreatedAtAction(nameof(GetQuestionByBody), new { Body = newPost.Body }, newPost.ToDTO());
        }

        [HttpPost("{idParent}")]
        public async Task<ActionResult<PostDTO>> PostReply([FromRoute]int idParent, [FromBody]PostDTO data)
        {

            var user = await _context.Users.Where(u => u.Pseudo == User.Identity.Name).SingleOrDefaultAsync();
            var post = await _context.Posts.Where(p => p.Id == idParent && p.ParentId == null).SingleOrDefaultAsync();
            if (user == null || post == null) return BadRequest();

            var newPost = data.ToOBJ();
            newPost.UserId = user.Id;
            newPost.ParentId = idParent;
            _context.Posts.Add(newPost);
            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty)
                return BadRequest(res);
            return CreatedAtAction(nameof(GetQuestionByBody), new { Body = newPost.Body }, newPost.ToDTO());
        }


        //Update a post
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, PostDTO postDTO)
        {
            var connectedUser = await _context.Users.Where(u => u.Pseudo == User.Identity.Name).SingleOrDefaultAsync();
            if (!(postDTO.User.Id == connectedUser.Id || connectedUser.Role == Role.Admin)) { return BadRequest(); }
            if (id != postDTO.Id) { return BadRequest(); }
            var post = await _context.Posts.Where(p => p.Id == id).SingleOrDefaultAsync();
            if (post == null)
                return NotFound();
            post.Title = postDTO.Title;
            post.Body = postDTO.Body;
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
            if (post == null)
                return NotFound();
            var connectedUser = await _context.Users.Where(u => u.Pseudo == User.Identity.Name).SingleOrDefaultAsync();
            if (!(post.User.Id == connectedUser.Id || connectedUser.Role == Role.Admin)) { return BadRequest(); }

            if (post.User.Id == connectedUser.Id && !(post.Comments == null && post.Responses == null))
            {
                return BadRequest();
            }
            _context.Posts.Remove(post);

            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpGet("acceptPost/{id}")]
        public async Task<ActionResult<PostDTO>> acceptPost(int id)
        {
            var connectedUser = await _context.Users.Where(u => u.Pseudo == User.Identity.Name).SingleOrDefaultAsync();
            var response = await _context.Posts.Where(p => p.Id == id).SingleOrDefaultAsync();
            if (response == null) { return NotFound(); }
            var question = response.Parent;
            if (question.User.Id != connectedUser.Id) return BadRequest();
            if(question.Accpeted != null) return BadRequest();
            connectedUser.Reputation += 2;
            response.User.Reputation += 15;
            question.AcceptedAnswerId = response.Id;
            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty)
                return BadRequest(res);
            return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, question.ToDTO());
        }

        [HttpGet("unAcceptPost/{id}")]
        public async Task<ActionResult<PostDTO>> unAcceptPost(int id)
        {
            var connectedUser = await _context.Users.Where(u => u.Pseudo == User.Identity.Name).SingleOrDefaultAsync();
            var response = await _context.Posts.Where(p => p.Id == id).SingleOrDefaultAsync();
            if (response == null) { return NotFound(); }
            var question = response.Parent;
            if (question.User.Id != connectedUser.Id) return BadRequest();
            if(question.Accpeted == null) return BadRequest();
            connectedUser.Reputation -= 2;
            response.User.Reputation -= 15;
            question.AcceptedAnswerId = response.Id;
            question.AcceptedAnswerId = null;
            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty)
                return BadRequest(res);
            return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, question.ToDTO());
        }
    }
}