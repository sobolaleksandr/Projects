﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace domain
{
    public interface IPeopleRepository
    {
        Task<IEnumerable<Person>> GetPeople();
        Task<Person> GetPerson(long id);
        Task<bool> TryToUpdatePerson(long id, Person person);
        Task<bool> TryToCreatePerson(Person person);
        Task<Person> DeletePerson(long id);
    }
}
