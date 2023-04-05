using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJournal
{
    public class Student : User
    {
        public string Group { get; set; }
        public string[] Subjects { get; set; }
        public List<int>[] Marks { get; set; }
        public Student(string name, string password, string group, string[] subjects, List<int>[] marks) : base(name, password)
        {
            Name = name;
            Password = password;
            Group = group;
            Subjects = subjects;
            Marks = marks;
        }
    }
}
