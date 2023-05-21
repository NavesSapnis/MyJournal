using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJournal.Class
{
    public class TeachersGroups
    {
        public int TeacherId { get; set; }
        public int GroupId { get; set; }
        public TeachersGroups(int teacherId, int groupId) 
        {
            TeacherId = teacherId;
            GroupId = groupId;
        }
    }
}
