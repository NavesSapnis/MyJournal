using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJournal.Class
{
    internal class Mark
    {
        public int StudentId { get; set; }
        public int GroupId { get; set; }
        public int Grade { get; set; }
        public Mark(int studentId,int groupId,int grade) 
        {
            StudentId = studentId;
            GroupId = groupId;
            Grade = grade;
        }
    }
}
