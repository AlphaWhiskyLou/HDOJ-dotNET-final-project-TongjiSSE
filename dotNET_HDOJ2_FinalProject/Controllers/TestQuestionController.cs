using Microsoft.AspNetCore.Mvc;
using DataModels.Models;
using System.Linq;

namespace ASP.NET_WebApi_DirtyPrototype.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestQuestionController : ControllerBase
    {
        private compilerContext _TestContext = new compilerContext();
        [HttpGet(Name = "GetProducts")]
        public async Task<List<Question>> GetQuestionInfo([FromQuery]string name, [FromQuery] string password)
        {
            List<Question> response = new List<Question>();
            var context = _TestContext;
            IQueryable<Question> questions =
                from question in context.Questions select question;
            response.AddRange(questions);

            Response.Clear();
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
            //https://blog.csdn.net/wf824284257/article/details/87991205?ops_request_misc=%257B%2522request%255Fid%2522%253A%2522165371057216782246488724%2522%252C%2522scm%2522%253A%252220140713.130102334..%2522%257D&request_id=165371057216782246488724&biz_id=0&utm_medium=distribute.pc_search_result.none-task-blog-2~all~sobaiduend~default-2-87991205-null-null.142^v11^pc_search_result_control_group,157^v12^control&utm_term=Asp.net%E8%B7%A8%E5%9F%9F&spm=1018.2226.3001.4187

        }

    }

}
