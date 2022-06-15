using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class PostRepository
    {
        private readonly PostDbContext context;

        public PostRepository(PostDbContext ctx)
        {
            context = ctx;
        }

        public int CreatePost(Post post)
        {
            context.Posts.Add(post);
            context.SaveChanges();
            var id = context.Posts.FirstOrDefault(p => p.AuthorId == post.AuthorId && p.CreatedAt == post.CreatedAt && p.Content == post.Content).Id;
            if(id != null)
                return id;

            return 0;
        }

        public Post GetPost(int id)
        {
            var post = context.Posts.FirstOrDefault(p => p.Id == id);
            return post;
        }

        public List<Post> GetAllPostsByUser(int userId)
        {
            var posts = context.Posts.Where(p => p.AuthorId == userId).OrderByDescending(p => p.CreatedAt).ToList();
            return posts;
        }

        public void DeleteAllPostsByUser(int id)
        {
            var posts = context.Posts.Where(p => p.AuthorId == id).ToList();
            context.Posts.RemoveRange(posts);
            context.SaveChanges();
        }

        public List<Post> GetAllPosts()
        {
            var posts = context.Posts.OrderByDescending(p => p.CreatedAt).Take(100).ToList();
            return posts;
            
        }

        public void DeletePost(int PostId)
        {
            var toRemove = context.Posts.FirstOrDefault(p => p.Id == PostId);
            context.Posts.Remove(toRemove);
            context.SaveChanges();
        }
    }
}
