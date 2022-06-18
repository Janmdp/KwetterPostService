using Data;
using Logic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KwetterPostService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly PostLogic logic;

        public PostController(PostDbContext ctx)
        {
            logic = new PostLogic(ctx);
        }

        [HttpPost("create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreatePost([FromBody]Post post)
        {
            var userId = User
                .Claims
                .SingleOrDefault();

            if (userId == null)
                return Unauthorized("No valid user was supplied");

            post.AuthorId = Convert.ToInt32(userId.Value);
            post.CreatedAt = DateTime.Now;
            //todo: Post id here?
            logic.CreatePost(post);
            return Ok("Post created");
        }

        [HttpGet("Post/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetPost(int id)
        {
            var data = logic.GetPost(id);
            return Ok(data);
        }

        [HttpGet("Posts")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllPost()
        {
            var data = logic.GetAllPosts();
            return Ok(data);
        }

        [HttpGet("postsbyuser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllPostByUser(int userId)
        {
            var data = logic.GetAllPostsByUser(userId);
            return Ok(data);
        }


        [HttpDelete("delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeletePost(int id)
        {
            logic.DeletePost(id);
            return Ok("Delete post");
        }

        //[HttpDelete("deleteall")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<IActionResult> DeleteAllPostsByUser(int id)
        //{
        //    logic.DeleteAllPostsByUser(id);
        //    return Ok("Delete post");
        //}
    }
}
