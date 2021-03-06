using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HallOfFame.Web.Controllers
{
    [Produces("application/json")]
    [Route("")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPeopleRepository _peopleRepository;

        public PeopleController(IPeopleRepository peopleRepository)
        {
            _peopleRepository = peopleRepository;
        }

        [Route("api/v1/persons")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            var persons = await _peopleRepository.GetPeople();
            return new ObjectResult(persons);
        }

        [HttpGet("api/v1/person/{id}")]
        public async Task<ActionResult<Person>> GetPerson(long id)
        {
            var person = await _peopleRepository.GetPerson(id);

            if (person == null)
                return NotFound();

            return new ObjectResult(person);
        }

        [Route("api/v1/person")]
        [HttpPut]
        public async Task<IActionResult> CreatePerson(Person person)
        {
            if (person != null)
                if (await _peopleRepository.TryToCreatePerson(person))
                    return Ok();

            return BadRequest();
        }

        [HttpPost("api/v1/person/{id?}")]
        public async Task<ActionResult<Person>> UpdatePerson(long? id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

            bool result;

            if (id.HasValue)
                result = await _peopleRepository.TryToUpdatePerson(id.Value, person);
            else
                result = await _peopleRepository.TryToUpdatePerson(person.Id, person);

            if (result)
                return Ok();

            return NotFound();
        }

        [HttpDelete("api/v1/person/{id}")]
        public async Task<ActionResult<Person>> DeletePerson(long id)
        {
            var person = await _peopleRepository.DeletePerson(id);
            if (person == null)
                return NotFound();

            return Ok();
        }
    }
}
