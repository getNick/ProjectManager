using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Models.ConstAndEnums;

namespace ProjectManager.Models
{
    public class ProjectTask:BaseModel
    {
        public Participant Assignee { get; set; }
        public DateTime AddingTime { get; set; }
        public DateTime Deadline { get; set; }
        public AdditionalFiles AdditionalFiles { get; set; }
        public PriorityEnum Priority { get; set; }
        public List<WorkPeriod> WorkPeriods { get; set; }
        public TaskStatusEnum Status { get; set; }
        public TimeSpan MinEstimate { get; set; }
        public TimeSpan MaxEstimate { get; set; }
        public int ComplitedLine { get; set; }
    }
}
