using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DataModels.Models;
using System;

namespace dotNET_HDOJ2_FinalProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private compilerContext _CompilerContext = new compilerContext();

        [HttpGet("listComments")]
        public async Task<List<Comment>> listComments([FromQuery]long solutionId)
        {
            IQueryable<Comment> comments =
                from comment in _CompilerContext.Comments where comment.SolutionId == solutionId select comment;
            List<DateTime> commentTime;
            foreach(var c in comments)
            {

                //c.CommentTime = c.CommentTime.ToDateTime();
                //c.CommentTime.ToDateTime();
                //var timeOnly = new TimeOnly(0, 0, 0);
                //var datetime = c.CommentTime.ToDateTime(timeOnly);

            }
            return comments.ToList();
        }

        [HttpPost("likesIncrement")]
        public async Task<JObject> likesIncrement([FromQuery] long commentId)
        {
            Comment commentupdate = 
                _CompilerContext.Comments.FirstOrDefault(c => c.CommentId == commentId);
            commentupdate.Likes += 1;
            _CompilerContext.SaveChanges();

            JObject returnJson = new JObject();
            returnJson.Add("code", 200);
            returnJson.Add("message", "null");
            return returnJson;
        }

        [HttpPost("addComment")]
        public async Task<JObject> addComment([FromBody] JObject jObject)
        {
            var jsonStr = JsonConvert.SerializeObject(jObject);
            var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);

            long userId = jsonParams.userId;
            string content = jsonParams.content;
            long solutionId = jsonParams.solutionId;
            //long commentId = jsonParams.commentId;
            long likes = jsonParams.likes;
            DateTime createdTime = DateTime.Now;

            Comment comment = new Comment();
            comment.UserId = userId;
            comment.Content = content;
            comment.SolutionId = solutionId;
            //comment.CommentId = commentId;
            comment.Likes = likes;
            comment.CommentTime = createdTime;

            _CompilerContext.Comments.Add(comment);
            _CompilerContext.SaveChanges();

            JObject returnJson = new JObject();
            returnJson.Add("code", 200);
            returnJson.Add("message", "评论提交成功！");
            return returnJson;
        }

        [HttpGet("deleteComment")]
        public async Task<JObject> deleteComment([FromQuery] long commentId)
        {
            Comment commentdelete =
               _CompilerContext.Comments.FirstOrDefault(c => c.CommentId == commentId);
            _CompilerContext.Comments.Remove(commentdelete);
            _CompilerContext.SaveChanges();

            JObject returnJson = new JObject();
            returnJson.Add("code", 200);
            returnJson.Add("message", "null");
            return returnJson;
        }

    }
}
