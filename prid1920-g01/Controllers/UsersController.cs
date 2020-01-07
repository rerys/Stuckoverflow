using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using prid1920_g01.Models;
using PRID_Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Text;
using System.Security.Claims;
using prid1920_g01.Helpers;

namespace prid1920_g01.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Prid1920_g01Context _context;
        private readonly TokenHelper _tokenHelper;

        public UsersController(Prid1920_g01Context context)
        {
            _context = context;
            _tokenHelper = new TokenHelper(context);

        }


        //List
        [Authorized(Role.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            return (await _context.Users.ToListAsync()).ToDTO();
        }

        //Read
        [AllowAnonymous]
        [HttpGet("{pseudo}")]
        public async Task<ActionResult<UserDTO>> GetOne(string pseudo)
        {
            var user = await _context.Users.Where(u => u.Pseudo == pseudo).SingleOrDefaultAsync();
            if (user == null)
                return NotFound();
            return user.ToDTO();
        }


        //Read
        [AllowAnonymous]
        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserDTO>> GetOneByEmail(string email)
        {
            var user = await _context.Users.Where(u => u.Email == email).SingleOrDefaultAsync();
            if (user == null)
                return NotFound();
            return user.ToDTO();
        }



        //Create
        [Authorized(Role.Admin)]
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO data)
        {
            var newUser = data.ToOBJ();
            var passwordLength = data.Password.Length;
            if (!(passwordLength >= 3 && passwordLength <= 10))
            {
                var err = new ValidationErrors().Add("Remark must have min length of 3 and max Length of 10", nameof(newUser.Password));
                return BadRequest(err);
            }

            _context.Users.Add(newUser);
            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty)
                return BadRequest(res);
            return CreatedAtAction(nameof(GetOne), new { pseudo = newUser.Pseudo }, newUser.ToDTO());

        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<ActionResult<UserDTO>> PostUserSignUp(UserDTO data)
        {

            var newUser = data.ToOBJ();
            var passwordLength = data.Password.Length;
            if (!(passwordLength >= 3 && passwordLength <= 10))
            {
                var err = new ValidationErrors().Add("Remark must have min length of 3 and max Length of 10", nameof(newUser.Password));
                return BadRequest(err);
            }
            newUser.Reputation = 1;
            newUser.Role = Role.Member;
            _context.Users.Add(newUser);
            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty)
                return BadRequest(res);
            return CreatedAtAction(nameof(GetOne), new { pseudo = newUser.Pseudo }, newUser.ToDTO());

        }

        //Update
        [Authorized(Role.Admin)]
        [HttpPut("{pseudo}")]
        public async Task<IActionResult> PutUser(string pseudo, UserDTO userDTO)
        {
            if (pseudo != userDTO.Pseudo)
                return BadRequest();
            var user = await _context.Users.Where(u => u.Pseudo == pseudo).SingleOrDefaultAsync();
            if (user == null)
                return NotFound();
            user.Pseudo = userDTO.Pseudo;
            user.Email = userDTO.Email;
            user.LastName = userDTO.LastName;
            user.FirstName = userDTO.FirstName;
            user.BirthDate = userDTO.BirthDate;
            user.Reputation = userDTO.Reputation;
            if (userDTO.Password != null)
            {
                var passwordLength = userDTO.Password.Length;
                if (!(passwordLength >= 3 && passwordLength <= 10))
                {
                    var err = new ValidationErrors().Add("Remark must have min length of 3 and max Length of 10", nameof(user.Password));
                    return BadRequest(err);
                }
                user.Password = TokenHelper.GetPasswordHash(userDTO.Password);
            }

            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty)
                return BadRequest(res);
            return NoContent();
        }


        //delete
        [Authorized(Role.Admin)]
        [HttpDelete("{pseudo}")]
        public async Task<IActionResult> DeleteUser(string pseudo)
        {
            var user = await _context.Users.Where(u => u.Pseudo == pseudo).SingleOrDefaultAsync();

            if (user == null)
                return NotFound();


            //delete de tous les commentaires de l'user
            _context.Comments.RemoveRange(from c in _context.Comments where c.UserId == user.Id select c);
            //delete de tous les votes de l'user
            _context.Votes.RemoveRange(from v in _context.Votes where v.UserId == user.Id select v);
            //delete de tous les posts de l'user à supprimer 
            //recursion: suppression de toutes les réponses si le post est une question
            var posts = (from p in _context.Posts where p.UserId == user.Id select p);
            foreach (var p in posts)
            {

                _context.Comments.RemoveRange(from c in _context.Comments where c.PostId == p.Id select c);
                _context.Votes.RemoveRange(from v in _context.Votes where v.PostId == p.Id select v);
                _context.PostTags.RemoveRange(from post in _context.PostTags where post.PostId == p.Id select post);
                var responses = (from r in _context.Posts where r.ParentId == p.Id select r);

                foreach (var r in responses)
                {
                    _context.Comments.RemoveRange(from c in _context.Comments where c.PostId == r.Id select c);
                    _context.Votes.RemoveRange(from v in _context.Votes where v.PostId == r.Id select v);
                }

                _context.Posts.RemoveRange(responses);
                await _context.SaveChangesAsync();
                _context.Posts.Remove(p);
            }


            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<User>> Authenticate(UserDTO data)
        {
            var user = await Authenticate(data.Pseudo, data.Password);
            if (user == null)
                return BadRequest(new ValidationErrors().Add("user not found", "Pseudo"));
            if (user.Token == null)
                return BadRequest(new ValidationErrors().Add("Incorrect password", "Password"));
            return Ok(user.AuthenticateDTO());
        }

        private async Task<User> Authenticate(string pseudo, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Pseudo == pseudo);
            // return null if member not found
            if (user == null)
                return null;

            var hash = TokenHelper.GetPasswordHash(password);

            if (user.Password == hash)
            {
                // authentication successful so generate jwt token
                user.Token = TokenHelper.GenerateJwtToken(user.Pseudo, user.Role);
                // Génère un refresh token et le stocke dans la table Members
                var refreshToken = TokenHelper.GenerateRefreshToken();
                await _tokenHelper.SaveRefreshTokenAsync(pseudo, refreshToken);
            }

            // remove password before returning
            user.Password = null;

            return user;
        }


        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult<TokensDTO>> Refresh([FromBody] TokensDTO tokens)
        {
            var principal = TokenHelper.GetPrincipalFromExpiredToken(tokens.Token);
            var pseudo = principal.Identity.Name;
            var savedRefreshToken = await _tokenHelper.GetRefreshTokenAsync(pseudo);
            if (savedRefreshToken != tokens.RefreshToken)
                throw new SecurityTokenException("Invalid refresh token");

            var newToken = TokenHelper.GenerateJwtToken(principal.Claims);
            var newRefreshToken = TokenHelper.GenerateRefreshToken();
            await _tokenHelper.SaveRefreshTokenAsync(pseudo, newRefreshToken);

            return new TokensDTO
            {
                Token = newToken,
                RefreshToken = newRefreshToken
            };
        }


    }
}