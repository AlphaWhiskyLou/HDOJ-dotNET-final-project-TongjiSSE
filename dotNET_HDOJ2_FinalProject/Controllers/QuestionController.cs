using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DataModels.Models;
using Microsoft.AspNetCore.Mvc;
using CheckAnswerService;

namespace dotNET_HDOJ2_FinalProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : Controller
    {
        private compilerContext _CompilerContext = new compilerContext();

        [HttpGet("listQuestions")]
        public async Task<List<Question>> listQuestions()
        {
            IQueryable<Question> questions =
                from question in _CompilerContext.Questions select question;
            return questions.ToList();
        }

        [HttpGet("getQuestion")]
        public async Task<Question> getQuestion([FromQuery]long questionId)
        {
            IQueryable<Question> questions =
                from question in _CompilerContext.Questions where question.QuestionId==questionId select question;

            Question questionRet = questions.FirstOrDefault();

            return questionRet;
        }

        [HttpGet("listSamples")]
        public async Task<List<Sample>> listSamples([FromQuery]long questionId)
        {
            IQueryable<Sample> samples =
                from sample in _CompilerContext.Samples where sample.QuestionId == questionId && sample.IsTest==0 select sample;

            return samples.ToList();
        }

        [HttpPost("addSubmissionHistory")]
        public async Task<JObject> addSubmissionHistory([FromBody]JObject jObject)
        {
            var jsonStr = JsonConvert.SerializeObject(jObject);
            var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);

            long userId = jsonParams.userId;
            long questionId = jsonParams.questionId;
            string result = jsonParams.result;
            string language = jsonParams.language;

            Submission submission = new Submission();
            submission.SubmissionId = System.Guid.NewGuid().ToString();
            submission.UserId = userId;
            submission.QuestionId = questionId;
            submission.Language = language;
            submission.Result = result;
            submission.SubmissionTime = DateTime.Now;

            _CompilerContext.Submissions.Add(submission);
            _CompilerContext.SaveChanges();

            JObject returnJson = new JObject();
            returnJson.Add("code", 200);
            returnJson.Add("message", "注册成功");

            return jObject;
        }

        [HttpGet("submissionTable")]
        public async Task<List<Submission>> submissionTable([FromQuery]long userId, [FromQuery]long questionId)
        {
            IQueryable<Submission> submissions =
                from submission in _CompilerContext.Submissions where submission.QuestionId == questionId && submission.UserId == userId select submission;

            return submissions.ToList();
        }

        [HttpPost("check")]
        public async Task<JObject> check([FromBody] JObject jObject)
        {
            var jsonStr = JsonConvert.SerializeObject(jObject);
            var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);

            string clientId = "cd6d8a995171b3c59d622b2175d3b5d8"; //Replace with your client ID
            string clientSecret = "3e72cad6c57d97d3f3c015c0a59538a937b1c0b697b454db14a9b65cd1aa877c"; //Replace with your client Secret
            string script = jsonParams.code;  //program to compile and execute
            //string stdin = jsonParams.stdin;  //StdIn
            string language = jsonParams.language;  //language of the script
            string versionIndex = jsonParams.versionIndex; //version of the language to be used
            string output;
            long questionId = jsonParams.questionId;
            CheckAnswerService.CheckAnswerService checkAnswerService = new CheckAnswerService.CheckAnswerService();
            JObject returnJson =checkAnswerService.getRemoteCheck(clientId, clientSecret, script, language, versionIndex,questionId);
            return returnJson;
        }


    }
}
