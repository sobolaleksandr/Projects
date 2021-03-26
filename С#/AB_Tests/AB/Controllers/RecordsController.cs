using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AB.Domain.Models;
using AB.Services;
using System.Threading.Tasks;

namespace AB.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : Controller
    {
        private readonly RecordsService recordsService;
        public RecordsController(RecordsService recordsService)
        {
            this.recordsService = recordsService;
        }

        [HttpPost]
        public async Task<IActionResult> Save(IEnumerable<tblRecord> records)
        {
            if (await recordsService.TryToAddRecords(records))
                return Ok(records);

            return BadRequest(records);
        }

        [HttpGet]
        public async Task<IEnumerable<HistogramColumn>> CalculateHistogram(int days = 7) => 
            await recordsService.CalculateHistogram(days);

    }
}