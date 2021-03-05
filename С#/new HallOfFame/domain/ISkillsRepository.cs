using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace domain
{
    public interface ISkillsRepository
    {
        Task<IEnumerable<Skill>> GetSkills();
        Task<Skill> GetSkill(long id);
        Task<bool> TryToUpdateSkill(long id, Skill skill);
        Task<bool> TryToCreateSkill(Skill skill);
        Task<bool> TryToCreateSkills(IEnumerable<Skill> skills);
        Task<Skill> DeleteSkill(long id);
    }
}
