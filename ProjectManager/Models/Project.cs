using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public class Project:BaseModel
    {
        //public Participant ProjectLead { get; set; }
        public List<CustomerWish> CustomerWishes { get; set; }
        public List<Department> Departments { get; set; }
        public string ImageUrl { get; set; }
        public double Budget { get; set; }
        public DateTime Deadline { get; set; }
        public List<Participant> Participants { get; set; }
        public List<ProjectTask> Tasks { get; set; }
    }
}
