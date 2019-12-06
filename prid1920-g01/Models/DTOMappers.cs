using System;
using System.Collections.Generic;
using System.Linq;

namespace prid1920_g01.Models
{

    public static class DTOMappers
    {

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                                                                          //
        //                                             DTOMapper User                                               //
        //                                                                                                          //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //convertisseur d'un User vers un UserDTO
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
                Posts = user.Posts.ToDTO(),
                Votes = user.Votes.ToDTO(),
                Comments = user.Comments.ToDTO()

            };

        }

        public static UserDTO ToSimpleDTO(this User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Pseudo = user.Pseudo,
                Reputation = user.Reputation
            };
        }
        public static List<UserDTO> ToSimpleDTO(this IEnumerable<User> users)
        {
            return users.Select(u => u.ToSimpleDTO()).ToList();
        }


        //convertisseur d'une liste User vers une liste UserDTO
        public static List<UserDTO> ToDTO(this IEnumerable<User> users)
        {
            return users.Select(u => u.ToDTO()).ToList();
        }

        //convertisseur d'un UserDTO vers un User
        public static User ToOBJ(this UserDTO u)
        {
            return new User
            {
                Id = u.Id,
                Pseudo = u.Pseudo,
                Email = u.Email,
                LastName = u.LastName,
                FirstName = u.FirstName,
                BirthDate = u.BirthDate,
                Reputation = u.Reputation,
                Role = u.Role,
                Posts = u.Posts.ToOBJ(),
                Votes = u.Votes.ToOBJ(),
                Comments = u.Comments.ToOBJ()
            };
        }

        //convertisseur d'une liste UserDTO vers une liste User
        public static List<User> ToOBJ(this IEnumerable<UserDTO> users)
        {
            return users.Select(u => u.ToOBJ()).ToList();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                                                                          //
        //                                             DTOMapper Post                                               //
        //                                                                                                          //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //convertisseur d'un Post vers un PostDTO
        public static PostDTO ToDTO(this Post post)
        {

            return new PostDTO
            {

                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                Timestamp = post.Timestamp,
                User = post.User.ToSimpleDTO(),
                Responses = post.Responses.Select(r => new PostDTO
                {
                    Id = r.Id,
                    Body = r.Body,
                    Timestamp = r.Timestamp,
                    User = r.User.ToSimpleDTO(),
                    Comments = r.Comments.ToDTO()


                }).ToList(),

                // Accepted = new PostDTO{
                //     Id = post.Accpeted.Id
                // },

                Tags = post.Tags.Select(t => new TagDTO
                {
                    Id = t.Id,
                    Name = t.Name

                }).ToList(),
                Votes = post.Votes.Select(r => new VoteDTO
                {
                    UpDown = r.UpDown
                }).ToList(),

                // Comments = post.Comments.Select(t => new CommentDTO
                // {
                //     Id = t.Id,
                //     Body = t.Body

                // }).ToList()


                Comments = post.Comments.ToDTO()

            };

        }

        //convertisseur d'une liste Post vers une liste PostDTO
        public static List<PostDTO> ToDTO(this IEnumerable<Post> posts)
        {
            return posts.Select(p => p.ToDTO()).ToList();
        }

        //convertisseur d'un PostDTO vers un Post
        public static Post ToOBJ(this PostDTO p)
        {
            return new Post
            {
                Id = p.Id,
                Title = p.Title,
                Body = p.Body,
                Timestamp = p.Timestamp,
                User = p.User.ToOBJ(),
                //Tags = p.Tags.ToOBJ(),
                Votes = p.Votes.ToOBJ(),
                Comments = p.Comments.ToOBJ()

            };
        }

        //convertisseur d'une liste PostDTO vers une liste Post
        public static List<Post> ToOBJ(this IEnumerable<PostDTO> posts)
        {
            return posts.Select(p => p.ToOBJ()).ToList();
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                                                                          //
        //                                            DTOMapper Comment                                             //
        //                                                                                                          //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //convertisseur d'un Comment vers un CommentDTO
        public static CommentDTO ToDTO(this Comment comment)
        {

            return new CommentDTO
            {

                Id = comment.Id,
                Body = comment.Body,
                Timestamp = comment.Timestamp,
                User = comment.User.ToSimpleDTO(),
                //Post = comment.Post.ToDTO()

            };

        }

        //convertisseur d'une liste Comment vers une liste CommentDTO
        public static List<CommentDTO> ToDTO(this IEnumerable<Comment> comments)
        {
            return comments.Select(c => c.ToDTO()).ToList();
        }

        //convertisseur d'un CommentDTO vers un Comment
        public static Comment ToOBJ(this CommentDTO c)
        {
            return new Comment
            {
                Id = c.Id,
                Body = c.Body,
                Timestamp = c.Timestamp,
                User = c.User.ToOBJ(),
                Post = c.Post.ToOBJ()
            };
        }

        //convertisseur d'une liste CommentDTO vers une liste Comment
        public static List<Comment> ToOBJ(this IEnumerable<CommentDTO> comments)
        {
            return comments.Select(c => c.ToOBJ()).ToList();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                                                                          //
        //                                             DTOMapper Tag                                                //
        //                                                                                                          //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //convertisseur d'un Tag vers un TagDTO
        public static TagDTO ToDTO(this Tag tag)
        {
            return new TagDTO
            {

                Id = tag.Id,
                Name = tag.Name,
                Posts = tag.Posts.ToDTO()

            };
        }

        //convertisseur d'une liste Tag vers un TagDTO
        public static List<TagDTO> ToDTO(this IEnumerable<Tag> tags)
        {
            return tags.Select(t => t.ToDTO()).ToList();
        }

        //convertisseur d'un TagDTO vers un Tag
        public static Tag ToOBJ(this TagDTO t)
        {
            return new Tag
            {
                Id = t.Id,
                Name = t.Name,
                // Posts = t.Posts.ToOBJ()
            };
        }

        //convertisseur d'une liste TagDTO vers un Tag
        public static List<Tag> ToOBJ(this IEnumerable<TagDTO> tags)
        {
            return tags.Select(t => t.ToOBJ()).ToList();
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                                                                          //
        //                                             DTOMapper Vote                                               //
        //                                                                                                          //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //convertisseur d'un Vote vers un VoteDTO
        public static VoteDTO ToDTO(this Vote vote)
        {
            return new VoteDTO
            {
                UpDown = vote.UpDown,
                User = vote.User.ToDTO(),
                Post = vote.Post.ToDTO()

            };
        }

        //convertisseur d'une liste Vote vers une liste VoteDTO
        public static List<VoteDTO> ToDTO(this IEnumerable<Vote> votes)
        {
            return votes.Select(v => v.ToDTO()).ToList();
        }

        //convertisseur d'un VoteDTO vers un Vote
        public static Vote ToOBJ(this VoteDTO v)
        {
            return new Vote
            {
                UpDown = v.UpDown,
                User = v.User.ToOBJ(),
                Post = v.Post.ToOBJ()
            };
        }

        //convertisseur d'une liste VoteDTO vers une liste Vote
        public static List<Vote> ToOBJ(this IEnumerable<VoteDTO> votes)
        {
            return votes.Select(v => v.ToOBJ()).ToList();
        }


    }

}