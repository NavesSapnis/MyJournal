using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJournal.Class
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        
        public Subject(int subjectId, string subjectName)
        {
            SubjectId= subjectId;
            SubjectName= subjectName;
        }
    }
}
