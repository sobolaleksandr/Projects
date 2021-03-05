using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HallOfFame.Data;
using domain;

namespace HallOfFame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillsRepository _skillsRepository;

        public SkillsController(ISkillsRepository skillsRepository)
        {
            _skillsRepository = skillsRepository;
        }

        // GET: api/Skills
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkills()
        {
            return new ObjectResult(await _skillsRepository.GetSkills());
        }

        // GET: api/Skills/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Skill>> GetSkill(long id)
        {
            var skill = await _skillsRepository.GetSkill(id);

            if (skill == null)
            {
                return NotFound();
            }

            return new ObjectResult(skill);
        }

        // PUT: api/Skills/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSkill(long id, Skill skill)
        {
            if (id != skill.Id)
            {
                return BadRequest();
            }

            if (await _skillsRepository.TryToUpdateSkill(id, skill))
            {
                return Ok();
            }

            return NotFound();
        }

        // POST: api/Skills
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Skill>> PostSkill(Skill skill)
        {
            if (skill == null)
            {
                return BadRequest();
            }

            await _skillsRepository.TryToCreateSkill(skill);
            return CreatedAtAction(nameof(GetSkill), new { id = skill.Id }, skill);
        }

        // DELETE: api/Skills/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Skill>> DeleteSkill(long id)
        {
            var skill = await _skillsRepository.DeleteSkill(id);
            if (skill == null)
            {
                return NotFound();
            }

            return new ObjectResult(skill);
        }

        [HttpPost]
        public async Task<ActionResult<Skill>> PostSkills(IEnumerable<Skill> skills)
        {
            if (skills == null)
            {
                return BadRequest();
            }

            await _skillsRepository.TryToCreateSkills(skills);
            return Ok();
        }
    }
}
