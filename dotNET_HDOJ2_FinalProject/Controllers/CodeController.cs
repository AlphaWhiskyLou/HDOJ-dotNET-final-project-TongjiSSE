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
    public class CodeController : ControllerBase
    {
        private compilerContext _CompilerContext = new compilerContext();

        [HttpGet("getTheLatestCode")]
        public async Task<Code> getTheLatestCode([FromQuery]long userId, [FromQuery]long questionId)
        {
            IQueryable<Code> codes =
                from code in _CompilerContext.Codes where code.UserId==userId && code.QuestionId==questionId select code;

            Code tempCode = codes.FirstOrDefault();

            return tempCode;
        }

        [HttpGet("getCodeByUidAndQid")]
        public async Task<Code> getCodeByUidAndQid([FromQuery] long userId, [FromQuery]long questionId)
        {
            IQueryable<Code> codes =
                from code in _CompilerContext.Codes where code.UserId == userId && code.QuestionId == questionId select code;

            Code tempCode = codes.FirstOrDefault();

            return tempCode;
        }

        [HttpPost("saveCode")]
        public async Task<JObject> saveCode([FromBody]JObject jObject)
        {
            var jsonStr = JsonConvert.SerializeObject(jObject);
            var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);

            long userId = jsonParams.userId;
            long questionId = jsonParams.questionId;
            long state = jsonParams.state;
            string content = jsonParams.content;
            string language = jsonParams.language;

            IQueryable<Code> TmpCode = from tmpCode in _CompilerContext.Codes where tmpCode.UserId == userId && tmpCode.QuestionId == questionId select tmpCode;

            if (TmpCode.Count() == 0 )
            {
                Code code = new Code();
                code.UserId = userId;
                code.QuestionId = questionId;
                code.Language = language;
                code.State = state;
                code.Content = content;
                code.SubmitTime = System.DateTime.Now;

                _CompilerContext.Codes.Add(code);
                _CompilerContext.SaveChanges();
            }
            else
            {
                Code codeUpdate = 
                    _CompilerContext.Codes.First(c => c.UserId == userId && c.QuestionId == questionId);
                codeUpdate.Content = content;
                codeUpdate.SubmitTime = System.DateTime.Now;
                codeUpdate.State = state;
                codeUpdate.Language = language;

                _CompilerContext.SaveChanges();
            }

            JObject returnJson = new JObject();
            returnJson.Add("code", 200);
            returnJson.Add("message", "保存成功");

            return returnJson;
        }
    }
}
