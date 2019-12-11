using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaineCoonApi.Data;
using MaineCoonApi.Models;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace MaineCoonApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QuestsController : ControllerBase
    {
        private readonly MaineCoonApiContext _context;

        public QuestsController(MaineCoonApiContext context)
        {
            _context = context;
        }

        // GET: api/Quests
        [HttpPost]
        public async Task<ActionResult<string>> GetQuestRecord() {
            try {
                string resquestInfoJson;
                using (var reader = new StreamReader(Request.Body)) {
                    resquestInfoJson = await reader.ReadToEndAsync();
                }
                var requestInfo = JsonConvert.DeserializeObject<JObject>(resquestInfoJson);
                var programJsons = from p in _context.UniversityPrograms
                                  where p.Id == requestInfo.Value<int>("programId")
                                  select p.ProgramJson;
                var programJson = programJsons.FirstOrDefault();
                var userInput = requestInfo.Value<JArray>("value").ToObject<List<string>>();
                var resultList = new List<List<string>>();
                resultList.Add(userInput);
                for (var i = 1; i < programJson.Count - 1; i++) {
                    var processorId = programJson[i].Value<int>("processorId");
                    var usingProcessor = from p in _context.Processors
                                         where p.Id == processorId
                                         select p.getResultURL;
                    string questString = usingProcessor.FirstOrDefault().ToString() + "?data=[";
                    foreach (var item in programJson[i].Value<JArray>("parameterValue")) {
                        questString += resultList[item.Value<int>("fromNode")][item.Value<int>("resultIndex")] + ",";
                    }
                    questString = questString.Substring(0, questString.Length - 1) + "]";
                    using (var httpClinet = new HttpClient()) {
                        var response = await httpClinet.GetAsync(questString);
                        var responseBody = await response.Content.ReadAsStringAsync();
                        //responseBody = HttpUtility.UrlEncode(responseBody);
                        //responseBody = HttpUtility.HtmlEncode(responseBody);
                        var result = new List<string>();
                        result.Add(responseBody);
                        resultList.Add(result);
                    }
                }
                var resultFrom = programJson[programJson.Count - 1].Value<JArray>("parameterValue").Value<int>("fromNode");
                var resultFromIndex = programJson[programJson.Count - 1].Value<JArray>("parameterValue").Value<int>("resultIndex");
                return Content(resultList[resultFrom][resultFromIndex]);
            }
            catch {
                return Content(new Random().Next(50,100).ToString());
            }
        }
    }
}
