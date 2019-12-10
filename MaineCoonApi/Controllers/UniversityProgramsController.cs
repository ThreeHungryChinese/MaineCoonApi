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

namespace MaineCoonApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UniversityProgramsController : ControllerBase {
        private readonly MaineCoonApiContext _context;

        public UniversityProgramsController(MaineCoonApiContext context)
        {
            _context = context;
        }

        // GET: Processers
        [HttpGet]
        public async Task<string> Index() {
            var programs = from p in _context.UniversityPrograms
                             join user in _context.User on p.belongsToUserID equals user.Id
                             select new { 
                                 id = p.Id, 
                                 name = p.ProgramName, 
                                 userName = user.UserName,
                                 instruction = p.ProgramIntroduction };

            return await Task.Run(() => {
                return JsonConvert.SerializeObject(programs.ToList()).Replace("\\", "");
            });

        }
        // GET: Processers/5
        [HttpGet("{id}")]
        public async Task<string> List(int? id) {
            /*
            var programs = from p in _context.UniversityPrograms
                             where p.Id == id
                             join user in _context.User on p.belongsToUserID equals user.Id
                             select new { p.Id, p.ProgramName, user.UserName, p.ProgramIntroduction, p.ProgramJson };
            return await Task.Run(() => {
                return JsonConvert.SerializeObject(programs.ToList()).Replace("\\", "");
            });*/
            var programs = from p in _context.UniversityPrograms
                           where p.Id == id
                           join user in _context.User on p.belongsToUserID equals user.Id
                           select new {p.ProgramParameterJson };
            return await Task.Run(() => {
                return JsonConvert.SerializeObject(programs.FirstOrDefault().ProgramParameterJson.ToList());
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

            var programs = from p in _context.UniversityPrograms
                             where (p.belongsToUserID == currentUserId && p.Id == id)
                             select new {
                                 p.Id,
                                 p.ProgramName,
                                 p.ProgramIntroduction,
                                 p.ProgramJson,
                                 p.UsedProcessorsIdJson,
                                 p.ProgramParameterJson
                             };
            if (!programs.Any()) {
                return NotFound();
            }
            return Content(JsonConvert.SerializeObject(programs.ToList()).Replace("\\", ""));
        }
        [HttpGet("Users/{Userid}")]
        public async Task<IActionResult> ListUsersAllProgram(int? Userid) {
            /////
            ///Add some auth function here
            /////
            if (Userid == null) {
                return NotFound();
            }

            var programs = from p in _context.UniversityPrograms
                           where p.belongsToUserID == Userid
                           select new {
                               id = p.Id,
                               name = p.ProgramName,
                               instruction = p.ProgramIntroduction,
                               count = p.Count
                           };
            if (programs.Count() == 0) {
                return NotFound();
            }
            return await Task.Run(() => {
                return Content(JsonConvert.SerializeObject(programs.ToList()).Replace("\\", ""));
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id) {
            try {
                var currentUserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(
                    claim => claim.Type == ClaimTypes.NameIdentifier)?.Value);
                if (currentUserId == 0) return BadRequest();
                var programs = from p in _context.UniversityPrograms
                                 where p.belongsToUserID == currentUserId && p.Id == id
                                 select p;
                if (!programs.Any()) {
                    throw new Exception("Not belongs to this user!");
                }
                string resquestInfoJson;
                using (var reader = new StreamReader(Request.Body)) {
                    resquestInfoJson = await reader.ReadToEndAsync();
                }
                var programInfo = JsonConvert.DeserializeObject<JObject>(resquestInfoJson);
                var program = programs.FirstOrDefault();
                program.ProgramName = programInfo.Value<string>("ProgramName");
                program.ProgramIntroduction = programInfo.Value<string>("ProgramIntroduction");
                program.ProgramParameterJson = programInfo.Value<JArray>("ProgramParameterJson");
                program.ProgramJson = programInfo.Value<JArray>("ProgramJson");
                program.UsedProcessorsIdJson= programInfo.Value<JArray>("usedProcessorId");

                _context.Entry(program).State = EntityState.Modified;
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
                var programInfo = JsonConvert.DeserializeObject<JObject>(resquestInfoJson);

                var currentUserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(
                    claim => claim.Type == ClaimTypes.NameIdentifier)?.Value);
                if (currentUserId == 0) return BadRequest();
                var programs = from p in _context.UniversityPrograms
                                 where p.belongsToUserID == currentUserId && p.ProgramName == programInfo.Value<string>("ProgramName")
                                 select p;
                if (programs.Any()) {
                    throw new Exception("Existed!");
                }
                var program = new UniversityProgram {
                    belongsToUserID = currentUserId,
                    Count = 0,
                    IsTrainNeeded = true,
                    IsEnabled=0,
                    ProgramName = programInfo.Value<string>("ProgramName"),
                    ProgramIntroduction = programInfo.Value<string>("ProgramIntroduction"),
                    ProgramParameterJson = programInfo.Value<JArray>("ProgramParameterJson"),
                    ProgramJson = programInfo.Value<JArray>("ProgramJson"),
                    UsedProcessorsIdJson = programInfo.Value<JArray>("processorId")
            };
                


                _context.Add(program);
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
            var program = await _context.UniversityPrograms.FindAsync(id);
            _context.UniversityPrograms.Remove(program);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool ProgramExists(int id) {
            return _context.Processors.Any(e => e.Id == id);
        }
        /*

        // POST: UniversityPrograms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProgramName,BelongsToUserId,ProgramJson,ProcesserId,Count,IsTrainNeeded,IsEnabled,ProgramIntroduction")] UniversityProgram universityProgram)
        {
            if (id != universityProgram.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(universityProgram);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UniversityProgramExists(universityProgram.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(universityProgram);
        }

        */
    }
}
