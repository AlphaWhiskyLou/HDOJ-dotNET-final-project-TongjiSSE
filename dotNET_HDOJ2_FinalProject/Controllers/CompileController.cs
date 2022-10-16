using CompilerService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DataModels.Models;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;

namespace dotNET_HDOJ2_FinalProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompileController : ControllerBase
    {
        private compilerContext _CompilerContext = new compilerContext();

        [HttpPost("doCompile")]
        public async Task<JObject> doCompile([FromBody] JObject jObject)
        {
            var jsonStr = JsonConvert.SerializeObject(jObject);
            var jsonParams = JsonConvert.DeserializeObject<dynamic>(jsonStr);

            string clientId = "b376f0e8fbb198e19f16c5c6b30eaa04"; //Replace with your client ID
            string clientSecret = "76bd03f4bba590fa450214ac2d95f8dc9f3eda425537adb66bcbee09efd169b7"; //Replace with your client Secret
            string script = jsonParams.script;  //program to compile and execute
            string stdin = jsonParams.stdin;  //StdIn
            string language = jsonParams.language;  //language of the script
            string versionIndex = jsonParams.versionIndex; //version of the language to be used

            JObject returnJson = CompilerService.CompileService.getRemoteCompile(clientId, clientSecret, script, stdin, language, versionIndex);

            return returnJson;
        }

    }
}
