using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prid1920_g01.Models
{

    public static class DTOMappers
    {

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                                                                          //
        //                                             DTOMapper User                                               //
        //                                                                                                          //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static UserDTO ToDTO(this User user)
        {

            return new UserDTO 
            {

                Id = user.Id,
                Pseudo = user.Pseudo,
                Email = user.Email,
                LastName = user.LastName,
                FirstName = user.FirstName,
                BirthDate = user.BirthDate,
                Reputation = user.Reputation,
                Role = user.Role,
                Posts = user.Posts.Select(p => p.Title),
                Votes = user.Votes.Select(v => v.Post.Title),
                Comments = user.Comments.Select(c => c.Post.Title)

            };

        }

        public static List<UserDTO> ToDTO(this IEnumerable<User> users)
        {
            return users.Select(u => u.ToDTO()).ToList();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                                                                          //
        //                                             DTOMapper Post                                               //
        //                                                                                                          //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static PostDTO ToDTO(this Post post)
        {

            return new PostDTO
            {

                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                Timestamp = post.Timestamp,
                User = post.User.Pseudo,
                Tags = post.Tags.Select(t => t.Name).ToList(),
                Votes = post.Votes.Select(v => v.Post.Title),
                Comments = post.Comments.Select(c => c.Body)

            };

        }

        public static List<PostDTO> ToDTO(this IEnumerable<Post> posts)
        {
            return posts.Select(p => p.ToDTO()).ToList();
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                                                                          //
        //                                            DTOMapper Comment                                             //
        //                                                                                                          //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////



        public static CommentDTO ToDTO(this Comment comment)
        {

            return new CommentDTO
            {

                Id = comment.Id,
                Body = comment.Body,
                Timestamp = comment.Timestamp,
                User = comment.User.Pseudo,
                Post = comment.Post.Title

            };

        }

        public static List<CommentDTO> ToDTO(this IEnumerable<Comment> comments)
        {
            return comments.Select(c => c.ToDTO()).ToList();
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                                                                          //
        //                                             DTOMapper Tag                                                //
        //                                                                                                          //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public static TagDTO ToDTO(this Tag tag)
        {
            return new TagDTO
            {

                Id = tag.Id,
                Name = tag.Name,
                Posts = tag.Posts.Select(p => p.Title).ToList()

            };
        }

        public static List<TagDTO> ToDTO(this IEnumerable<Tag> tags)
        {
            return tags.Select(t => t.ToDTO()).ToList();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                                                                          //
        //                                             DTOMapper Vote                                               //
        //                                                                                                          //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static VoteDTO ToDTO(this Vote vote)
        {
            return new VoteDTO
            {

                Timestamp = vote.Timestamp,
                UpDown = vote.UpDown,
                User = vote.User.Pseudo,
                Post = vote.Post.Title

            };
        }

        public static List<VoteDTO> ToDTO(this IEnumerable<Vote> votes)
        {
            return votes.Select(v => v.ToDTO()).ToList();
        }


    }

}


