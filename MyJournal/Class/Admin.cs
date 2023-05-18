using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJournal.Class
{
    internal class Admin
    {
        private Admin(){}
        public static Teacher TeacherCreate(string name,string password,List<string> groups,int mainGroup)
        {
            return new Teacher(name,password,groups,mainGroup);
        }
        public static Student StudentCreate(string name, string password,int group)
        {
            return new Student(name, password, group);
        }

    }
}
