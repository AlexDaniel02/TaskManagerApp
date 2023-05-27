using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class Statistics
    {
        public int TasksOverdue { get; set; }
        public int TasksDueToday { get; set; }
        public int TasksDueTomorrow { get; set; }
        public int TasksDone { get; set; }
        public int TasksToBeDone { get; set; }
        public Statistics()
        {
            TasksOverdue = 0;
            TasksDueToday = 0;
            TasksDueTomorrow = 0;
            TasksDone = 0;
            TasksToBeDone = 0;
        }
    }
}
