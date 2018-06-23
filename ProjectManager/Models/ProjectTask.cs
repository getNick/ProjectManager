using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Models.ConstAndEnums;

namespace ProjectManager.Models
{
    public class ProjectTask:BaseModel
    {
        public Project Project { get; set; }
        public Department Department { get; set; }
        public Team Team { get; set; }
        public Sprint Sprint { get; set; }
        public Participant Assignee { get; set; }
        public DateTime AddingTime { get; set; }
        public DateTime FinishedTime { get; set; }
        public DateTime Deadline { get; set; }
        public AdditionalFiles AdditionalFiles { get; set; }
        public PriorityEnum Priority { get; set; }
        public List<WorkPeriod> WorkPeriods { get; set; }
        public TaskTypeEnum Type { get; set; }
        public TaskStatusEnum Status { get; set; }
        public TimeSpan MinEstimate { get; set; }
        public TimeSpan MaxEstimate { get; set; }
        public int ComplitedLine { get; set; }
        public ProjectTask BaseTask { get; set; }
        public List<ProjectTask> Subtasks { get; set; }
        public Comments Comments { get; set; }
        public ProjectActivities Activities { get; set; }
        [NotMapped]
        public Color KanbanColor { get; set; }
    }
}
