using domain;
using HallOfFame.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HallOfFame.Infrostructure
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly ApplicationContext _context;

        public PeopleRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetPeople()
        {
            return await _context.People.ToListAsync();
        }

        public async Task<Person> GetPerson(long id)
        {
            return await _context.People.FindAsync(id); 
        }

        public async Task<bool> TryToUpdatePerson(long id, Person person)
        {
            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }
        private bool IsCorrectLevel(byte level)
        {
            return level <= 10 && level >= 0;
        }
        public async Task<bool> TryToCreatePerson(Person person)
        {
            if (person.SkillsCollection == null)
            {
                return false;
            }

            List<Skill> skills = new List<Skill>();

            foreach(var skill in person.SkillsCollection)
            {
                if (IsCorrectLevel(skill.Level))
                    skills.Add(skill);
                else
                    return false;
            }
            await _context.Skills.AddRangeAsync(skills);
            await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Person> DeletePerson(long id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return null;
            }

            _context.People.Remove(person);
            await _context.SaveChangesAsync();

            return person;
        }

        private bool PersonExists(long id)
        {
            return _context.People.Any(e => e.Id == id);
        }
    }
}
