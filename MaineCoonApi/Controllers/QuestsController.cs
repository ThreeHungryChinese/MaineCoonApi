using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaineCoonApi.Data;
using MaineCoonApi.Models;

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
        public async Task<ActionResult<IEnumerable<QuestRecord>>> GetQuestRecord()
        {
            return await _context.QuestRecord.ToListAsync();
        }
    }
}
