using System.ComponentModel.DataAnnotations;

namespace UnitTestApp.Models
{
    public class User
    {
        public string Surname { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public System.DateTime BirthDay { get; set; }
        public decimal Balance { get; set; }
    }
}