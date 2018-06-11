using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public class Department:BaseModel
    {
        public List<Team> Teams { get; set; }
        public List<ProjectTask> Tasks { get; set; }
    }
}
