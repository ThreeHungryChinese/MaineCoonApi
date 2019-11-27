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

        // GET: UniversityPrograms
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var programs = from program in _context.UniversityPrograms
                           where program.IsEnabled == true
                           join user in _context.User on program.BelongsToUserId equals user.Id
                           select new { program.Id, program.ProgramName, user.UserName, program.ProgramIntroduction, program.ProgramJson };
            return await Task.Run(() => {
                return Content(JsonConvert.SerializeObject(programs.ToList()).Replace("\\",""));
            });

        }

        // GET: UniversityPrograms/List/5
        [HttpGet("{id}")]
        public async Task<IActionResult> List(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var universityProgram = await _context.UniversityPrograms.Where(a => a.IsEnabled == true)
                .FirstOrDefaultAsync(m => m.Id == id);
            var programs = from program in _context.UniversityPrograms
                           where (program.IsEnabled == true && program.Id == id)
                           join user in _context.User on program.BelongsToUserId equals user.Id
                           select new { program.Id, program.ProgramName, user.UserName, program.ProgramIntroduction, program.ProgramJson };
            if (universityProgram == null)
            {
                return NotFound();
            }

            return await Task.Run(() => {
                return Content(JsonConvert.SerializeObject(programs.ToList()).Replace("\\", ""));
            });
        }
        // GET: UniversityPrograms/Details/5
        [HttpGet("Details/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int? id) {
            /////
            ///Add some auth function here
            var currentUserId=4;
            /////
            if (id == null) {
                return NotFound();
            }
            var programs = from program in _context.UniversityPrograms
                           where (program.BelongsToUserId==currentUserId && program.Id == id)
                           join user in _context.User on program.BelongsToUserId equals user.Id
                           select program;
            if (programs == null) {
                return NotFound();
            }
            return await Task.Run(() => {
                return Content(JsonConvert.SerializeObject(programs.ToList()).Replace("\\", ""));
            });
        }
        [HttpGet("Users/{Userid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListUsersAllProgram(int? Userid) {
            /////
            ///Add some auth function here
            /////
            if (Userid == null) {
                return NotFound();
            }

            var programs = from program in _context.UniversityPrograms
                           where program.BelongsToUserId == Userid
                           select new { program.Id,program.ProgramName,program.IsEnabled,program.Count,program.ProgramIntroduction,program.ProgramJson};
            if (programs == null) {
                return NotFound();
            }
            return await Task.Run(() => {
                return Content(JsonConvert.SerializeObject(programs.ToList()).Replace("\\", ""));
            });
        }

        // POST: UniversityPrograms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create"), ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProgramName,ProgramJson,IsTrainNeeded,ProgramIntroduction")] UniversityProgram universityProgram)
        {
            /////////some function to get current User Id
            var CurrentUserId = 4;
            ////////end
            ///
            if (ModelState.IsValid) {
                universityProgram.IsEnabled = false;
                universityProgram.IsTrainNeeded = true;
                universityProgram.ProcesserId = -1;
                universityProgram.BelongsToUserId = CurrentUserId;
                universityProgram.BelongsToUserId = 0;
                _context.Add(universityProgram);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }        
        // POST: UniversityPrograms/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            /////Authorize User
            
            /////
            var universityProgram = await _context.UniversityPrograms.FindAsync(id);
            _context.UniversityPrograms.Remove(universityProgram);
            await _context.SaveChangesAsync();
            return Ok();
        }
        private bool UniversityProgramExists(int id) {
            return _context.UniversityPrograms.Any(e => e.Id == id);
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
