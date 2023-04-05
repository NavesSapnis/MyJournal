using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJournal
{
    public abstract class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        protected User(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}
