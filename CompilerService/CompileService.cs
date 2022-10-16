using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CompilerService
{
    public class CompileService
    {
        public static JObject getRemoteCompile(string clientId, string clientSecret, string script, string stdin, string language, string versionIndex)
        {
            JObject returnError = new JObject();

            try
            {
                HttpClient httpClient = new HttpClient();

                String input = "{\"clientId\": \"" + clientId + "\",\"clientSecret\":\"" + clientSecret + "\",\"script\":\"" + script +
                   "\",\"language\":\"" + language + "\",\"stdin\":\"" + stdin + "\",\"versionIndex\":\"" + versionIndex + "\"} ";

                HttpContent httpContent = new StringContent(input);

                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var events = httpClient.PostAsync("https://api.jdoodle.com/v1/execute", httpContent).Result;

                var res = events.Content.ReadAsStringAsync().Result;

                Console.WriteLine(res);

                var jsonRes = JsonConvert.SerializeObject(res);
                var resParams = JsonConvert.DeserializeObject<dynamic>(jsonRes);

                return (JObject)JsonConvert.DeserializeObject(res);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return returnError;

        }

    }
}