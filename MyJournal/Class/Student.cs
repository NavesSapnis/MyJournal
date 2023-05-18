﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJournal
{
    public class Student : User
    {
        public int GroupId { get; set; }
        public Student(string name,string password,int groupId) : base(name, password)
        {
            Name = name;
            GroupId = groupId;
        }
    }
}
