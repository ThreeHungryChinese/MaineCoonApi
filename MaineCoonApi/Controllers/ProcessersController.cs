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

namespace MaineCoonApi.Controllers.SchoolAdmin {

    [ApiController]
    [Route("[controller]")]
    public class ProcessersController : ControllerBase {
        private readonly MaineCoonApiContext _context;

        public ProcessersController(MaineCoonApiContext context) {
            _context = context;
        }

        // GET: Processers
        [HttpGet]
        public async Task<string> Index() {
            var processors = from processor in  _context.Processors 
                             join user in _context.User on processor.belongsToUserID equals user.Id 
                             select new { processor.Id, processor.friendlyName,user.UserName, processor.Instruction};

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
                             select new { processor.Id, processor.friendlyName,user.UserName, processor.Instruction,processor.AlgorithmParameterJson };
            return await Task.Run(() => {
                return JsonConvert.SerializeObject(processors.ToList()).Replace("\\", "");
            });
        }
        // GET: Processers/Details/5
        [HttpGet("Details/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<IEnumerable<Processor>>> Details(int? id) {
            /////
            ///Add some auth function here
            var currentUserId = 4;
            /////
            if (id == null) {
                return NotFound();
            }

            var processers = from p in _context.Processors
                             where (p.belongsToUserID == currentUserId && p.Id == id)
                             select p;
            return await processers.ToArrayAsync();
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

            var Processers = from p in _context.Processors
                             where p.belongsToUserID == Userid
                             select new { p.Id, p.friendlyName, p.Instruction, p.AlgorithmParameterJson };
            if (Processers == null) {
                return NotFound();
            }
            return await Task.Run(()=> { 
                return Content(JsonConvert.SerializeObject(Processers.ToList()).Replace("\\",""));
            });
        }

        // POST: Processerss/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create"), ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("friendlyName","trainCallbackURL", "getResultURL", "isGetResultNeedWaitCallback", "resetURL", "TLSversion", "publicKey", "AlgorithmParameterJson", "Instruction")] Processor Processer) {
            /////////some function to get current User Id
            var CurrentUserId = 4;
            ////////end
            ///
            if (ModelState.IsValid) {
                Processer.belongsToUserID = CurrentUserId;
                Processer.count = 0;
                _context.Add(Processer);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
        // POST: Processerss/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            /////Authorize User

            /////
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
