using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public class Sprint:BaseModel
    {
        public Team Team { get; set; }
        public List<ProjectTask> ListTasks { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsActive { get; set; }
    }
}
