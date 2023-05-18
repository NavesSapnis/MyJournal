using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJournal
{
    public class Teacher : User
    {
        public List<string> Groups { get; set; }
        public int MainGroup { get; set; }
        public Teacher(string name, string password, List<string> groups, int mainGroup) : base(name, password)
        {
            Name = name;
            Groups = groups;
            Groups = groups;
            MainGroup = mainGroup;
        }
    }
}
