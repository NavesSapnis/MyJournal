using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJournal.MarkHelper
{
    class MarksHelper
    {
        public static double GetAverage(int sum,int count)
        {
            return Math.Round((float)sum / (float)count, 2);
        }
    }
}
