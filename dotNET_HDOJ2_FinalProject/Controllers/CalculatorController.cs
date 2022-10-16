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
    public class CalculatorController : Controller
    {
        private compilerContext _CompilerContext = new compilerContext();

        [HttpPost("calculate")]
        public async Task<String> calculateResult([FromBody] JObject jObject)
        {
            var jsonStr = JsonConvert.SerializeObject(jObject);
            var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);

            string input = jsonParams.stringClac;
            Console.WriteLine(input);

            CalculatorService.Calculatorref instance = new CalculatorService.Calculatorref();
            return instance.GetText(input);
        }
    }
}
