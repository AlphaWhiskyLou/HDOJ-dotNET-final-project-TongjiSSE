using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DataModels.Models;
using Microsoft.AspNetCore.Mvc;


namespace dotNET_HDOJ2_FinalProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolutionController : Controller
    {
        private compilerContext _CompilerContext = new compilerContext();

        [HttpGet("getSolutionCount")]
        public async Task<List<int>> GetSolutionCount()
        {
            IQueryable<Question> questions =
                from question in _CompilerContext.Questions select question;
            List<Question> all = questions.ToList();
            List<int> counts = new List<int>();

            foreach (Question question in all)
            {
                long questionId = question.QuestionId;

                IQueryable<Solution> solutions =
                    from solution in _CompilerContext.Solutions where solution.QuestionId == questionId select solution;
                int solutionCount = solutions.Count();
                counts.Add(solutionCount);
            }

            return counts;
        }

        [HttpGet("listSolutions")]
        public async Task<List<Solution>> listSolutions([FromQuery] long questionId)
        {
            IQueryable<Solution> solutions =
                from solution in _CompilerContext.Solutions where solution.QuestionId == questionId select solution;
            Console.WriteLine(solutions.ToList());
            return solutions.ToList();
        }

        [HttpPost("addSolution")]
        public async Task<JObject> addSolution([FromBody] JObject jObject)
        {
            var jsonStr = JsonConvert.SerializeObject(jObject);
            var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);

            long userId = jsonParams.userId;
            string code = jsonParams.code;
            string content = jsonParams.content;
            string language = jsonParams.language;
            string title = jsonParams.title;
            long questionId = jsonParams.questionId;
            long likes = jsonParams.likes;
            DateTime createdTime = System.DateTime.Now; 

            Solution solution = new Solution();
            solution.UserId = userId;
            solution.Code = code;
            solution.Language = language;
            solution.Title = title;
            solution.QuestionId = questionId;
            solution.Likes = likes;
            solution.CreatedTime = createdTime;

            _CompilerContext.Solutions.Add(solution);
            _CompilerContext.SaveChanges();

            JObject returnJson = new JObject();
            returnJson.Add("code", 200);
            returnJson.Add("message", "题解添加成功！");
            return returnJson;
        }

        [HttpGet("deleteSolution")]
        public async Task<JObject> deleteSolution([FromQuery] long solutionId)
        {
            Solution solutiondelete =
                _CompilerContext.Solutions.FirstOrDefault(c => c.SolutionId == solutionId);
            _CompilerContext.Solutions.Remove(solutiondelete);
            _CompilerContext.SaveChanges();

            JObject returnJson = new JObject();
            returnJson.Add("code", 200);
            returnJson.Add("message", "null");
            return returnJson;
        }

        [HttpPost("likesIncrement")]
        public async Task<JObject> likesIncrement(long solutionId)
        {
            Solution solutionupdate =
                _CompilerContext.Solutions.FirstOrDefault(c => c.SolutionId == solutionId);
            solutionupdate.Likes += 1;
            _CompilerContext.SaveChanges();

            JObject returnJson = new JObject();
            returnJson.Add("code", 200);
            returnJson.Add("message", "null");
            return returnJson;
        }

        [HttpGet("getSolutionById")]
        public async Task<JObject> getSolutionById([FromQuery] long solutionId)
        {
            IQueryable<Solution> solutionsqury =
                from solution in _CompilerContext.Solutions where solution.SolutionId == solutionId select solution;
            Solution solutionquery = solutionsqury.FirstOrDefault();
            //IQueryable<User> usersquery =
            //   from user in _CompilerContext.Users where user.UserId == solutionquery.UserId select user;
            //User userquery = usersquery.FirstOrDefault();
            JObject returnJson = new JObject();
            returnJson.Add("data", solutionquery.UserId);
            returnJson.Add("code", 200);
            returnJson.Add("message", "null");
            return returnJson;
        }
    }
}
