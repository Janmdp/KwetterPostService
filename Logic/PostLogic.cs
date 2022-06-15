using Data;
using Models;
using System;
using System.Collections.Generic;

namespace Logic
{
    public class PostLogic
    {
        private readonly PostRepository repository;

        public PostLogic(PostDbContext ctx)
        {
            repository = new PostRepository(ctx);
        }

        public void CreatePost(Post post)
        {
            repository.CreatePost(post);
        }

        public Post GetPost(int id)
        {
            return repository.GetPost(id);
        }

        public List<Post> GetAllPosts()
        {
            return repository.GetAllPosts();
        }

        public void DeletePost(int id)
        {
            repository.DeletePost(id);
        }

        public List<Post> GetAllPostsByUser(int userId)
        {
            return repository.GetAllPostsByUser(userId);
        }

        public void DeleteAllPostsByUser(int id)
        {
            repository.DeleteAllPostsByUser(id);
        }
    }
}
