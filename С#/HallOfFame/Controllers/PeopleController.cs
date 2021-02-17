using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HallOfFame.Models;

namespace HallOfFame.Controllers
{
    [Route("")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly HallOfFameContext _context;

        public PeopleController(HallOfFameContext context)
        {
            _context = context;
        }

        [Route("api/v1/persons")]
        [HttpGet(Name = "GetPersons")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            return await _context.Persons.ToArrayAsync();
        }

        [HttpGet("api/v1/person/{id}", Name = "GetPerson")]
        public async Task<ActionResult<Person>> Get(long id)
        {
            var person = await _context.Persons.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        [HttpPost("api/v1/person/{id}")]
        public async Task<IActionResult> Update(long id, Person person)
        {
            if (id != person.id || person == null)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [Route("api/v1/person")]
        [HttpPut]
        public async Task<IActionResult> Create([FromBody] Person person)
        {
            if (person == null )
            {
                return BadRequest();
            }
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
            CreatedAtAction("GetPerson", new { id = person.id }, person);
            return Ok();
        }

        [HttpDelete("api/v1/person/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool PersonExists(long id)
        {
            return _context.Persons.Any(e => e.id == id);
        }
    }
}
