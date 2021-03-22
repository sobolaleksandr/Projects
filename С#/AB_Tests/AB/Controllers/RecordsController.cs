using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AB.Domain.Models;
using AB.Services;
using System.Threading.Tasks;

namespace AB.Controllers
{
    [Route("api/[controller]")]
    public class RecordsController : Controller
    {
        private readonly RecordsService recordsService;
        public RecordsController(RecordsService recordsService)
        {
            this.recordsService = recordsService;
        }

        [HttpPost]
        public async Task<IActionResult> Save(IEnumerable<Record> records)
        {
            if (await recordsService.TryToAddRecords(records))
                return Ok(records);

            return BadRequest(records);
        }

        [HttpGet]
        public async Task<IEnumerable<HistogramColumn>> Calculate(int days = 7) => 
            await recordsService.Calculate(days);

    }
}