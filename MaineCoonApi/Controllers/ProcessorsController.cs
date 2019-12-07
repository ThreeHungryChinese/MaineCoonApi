using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MaineCoonApi.Data;
using MaineCoonApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.IO;

namespace MaineCoonApi.Controllers.SchoolAdmin {

    [ApiController]
    [Route("[controller]")]
    public class ProcessorsController : ControllerBase {
        private readonly MaineCoonApiContext _context;

        public ProcessorsController(MaineCoonApiContext context) {
            _context = context;
        }

        // GET: Processers
        [HttpGet]
        public async Task<string> Index() {
            var processors = from processor in  _context.Processors 
                             join user in _context.User on processor.belongsToUserID equals user.Id 
                             select new { processor.Id, processor.friendlyName,user.UserName, processor.instruction};

            return await Task.Run(() => {
                return JsonConvert.SerializeObject(processors.ToList()).Replace("\\", "");
            });

        }
        // GET: Processers/5
        [HttpGet("{id}")]
        public async Task<string> List(int? id) {
            var processors = from processor in _context.Processors
                             where processor.Id==id
                             join user in _context.User on processor.belongsToUserID equals user.Id
                             select new { processor.Id, processor.friendlyName,user.UserName, processor.instruction,processor.algorithmParameterJson };
            return await Task.Run(() => {
                return JsonConvert.SerializeObject(processors.ToList()).Replace("\\", "");
            });
        }
        // GET: Processers/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id) {
            /////
            ///Add some auth function here
            var currentUserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(
                claim => claim.Type == ClaimTypes.NameIdentifier)?.Value);
            /////
            if (id == null) {
                return NotFound();
            }

            var processors = from p in _context.Processors
                             where (p.belongsToUserID == currentUserId && p.Id == id)
                             select new { 
                                 p.Id,
                                 p.friendlyName,
                                 p.instruction,
                                 p.trainCallbackURL,
                                 p.resetURL,
                                 p.getResultURL,
                                 p.isGetResultNeedWaitCallback,
                                 p.TLSversion,
                                 p.algorithmParameterJson
                             };
            if (!processors.Any()) {
                return NotFound();
            }
                return Content(JsonConvert.SerializeObject(processors.ToList()).Replace("\\", ""));
        }
        [HttpGet("Users/{Userid}")]
        public async Task<IActionResult> ListUsersAllProgram(int? Userid) {
            /////
            ///Add some auth function here
            /////
            if (Userid == null) {
                return NotFound();
            }

            var Processers = from p in _context.Processors
                             where p.belongsToUserID == Userid
                             select new { 
                                 Id=p.Id, 
                                 name = p.friendlyName, 
                                 instruction = p.instruction, 
                                 count = p.count };
            return await Task.Run(()=> { 
                return Content(JsonConvert.SerializeObject(Processers.ToList()).Replace("\\",""));
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id) {
            try {
                var currentUserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(
                    claim => claim.Type == ClaimTypes.NameIdentifier)?.Value);
                if (currentUserId == 0) return BadRequest();
                var Processers = from p in _context.Processors
                                 where p.belongsToUserID == currentUserId && p.Id==id
                                 select new { p.Id, p.friendlyName, p.instruction, p.count };
                if (!Processers.Any()) {
                    throw new Exception("Not belongs to this user!");
                }
                string resquestInfoJson;
                using (var reader = new StreamReader(Request.Body)) {
                    resquestInfoJson = await reader.ReadToEndAsync();
                }
                var processorInfo = JsonConvert.DeserializeObject<JObject>(resquestInfoJson);
                var processor = new Processor {
                    Id = id,
                    friendlyName = processorInfo.Value<string>("friendlyName"),
                    instruction = processorInfo.Value<string>("instruction"),
                    trainCallbackURL = new Uri(processorInfo.Value<string>("trainCallbackURL")),
                    resetURL = new Uri(processorInfo.Value<string>("resetURL")),
                    getResultURL = new Uri(processorInfo.Value<string>("getResultURL")),
                    isGetResultNeedWaitCallback = processorInfo.Value<bool>("isGetResultNeedWaitCallback"),
                    TLSversion = (Processor.TLSVersion)processorInfo.Value<int>("TLSversion"),
                    algorithmParameterJson = processorInfo.Value<JArray>("algorithmParameterJson"),
                    belongsToUserID = currentUserId
                };
                _context.Entry(processor).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch {

                return BadRequest();
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create() {
            try {
                string resquestInfoJson;
                using (var reader = new StreamReader(Request.Body)) {
                    resquestInfoJson = await reader.ReadToEndAsync();
                }
                var processorInfo = JsonConvert.DeserializeObject<JObject>(resquestInfoJson);

                var currentUserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(
                    claim => claim.Type == ClaimTypes.NameIdentifier)?.Value);
                if (currentUserId == 0) return BadRequest();
                var Processers = from p in _context.Processors
                                 where p.belongsToUserID == currentUserId && p.friendlyName == processorInfo.Value<string>("processorInfo")
                                 select p;
                if (Processers.Any()) {
                    throw new Exception("Existed!");
                }
                var processor = new Processor {
                    friendlyName = processorInfo.Value<string>("friendlyName"),
                    instruction = processorInfo.Value<string>("instruction"),
                    trainCallbackURL = new Uri(processorInfo.Value<string>("trainCallbackURL")),
                    resetURL = new Uri(processorInfo.Value<string>("resetURL")),
                    getResultURL = new Uri(processorInfo.Value<string>("getResultURL")),
                    isGetResultNeedWaitCallback = processorInfo.Value<bool>("isGetResultNeedWaitCallback"),
                    TLSversion = (Processor.TLSVersion)processorInfo.Value<int>("TLSversion"),
                    algorithmParameterJson = processorInfo.Value<JArray>("algorithmParameterJson"),
                    belongsToUserID = currentUserId
                };
                _context.Add(processor);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch {

                return BadRequest();
            }
        }
        // POST: Processerss/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            /////Authorize User

            /////
            if (id <= 0) return BadRequest();
            var Processers = await _context.Processors.FindAsync(id);
            _context.Processors.Remove(Processers);
            await _context.SaveChangesAsync();
            return Ok();
        }
        
            private bool ProcessersExists(int id) {
            return _context.Processors.Any(e => e.Id == id);
        }
        /*

            // GET: Processers/Edit/5
            public async Task<IActionResult> Edit(int? id) {
                if (id == null) {
                    return NotFound();
                }

                var processer = await _context.Processers.FindAsync(id);
                if (processer == null) {
                    return NotFound();
                }
                return View(processer);
            }

            // POST: Processers/Edit/5
            // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
            // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("Id,friendlyName,isTrained,trainCallbackURL,getResultURL,isGetResultNeedWaitCallback,resetURL,TLSversion,publicKey,belongsToUserID,count,AlgorithmParameterJson,Instruction")] Processer processer) {
                if (id != processer.Id) {
                    return NotFound();
                }

                if (ModelState.IsValid) {
                    try {
                        _context.Update(processer);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException) {
                        if (!ProcesserExists(processer.Id)) {
                            return NotFound();
                        }
                        else {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(processer);
            }
            */
    }
}
