using System.Collections.Generic;

namespace UnitTestApp.Models
{
    public interface IRepository
    {
        IEnumerable<User> GetAll();
        //Получить данные о счете
        User Get(long id);
        void Create(User user);
        // Положить на счет со счета
        void Update(User user);
    }
}