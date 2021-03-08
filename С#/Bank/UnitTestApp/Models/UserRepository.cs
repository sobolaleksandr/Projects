using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UnitTestApp.Models
{
    public class UserRepository:IRepository
    {
        private UserContext _context;
        public UserRepository(UserContext context)
        {
            _context = context;
        }
        public User Get(long id)
        {
            return _context.Users.Find(id);
        }
        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }
        public void Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
