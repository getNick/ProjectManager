using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Models;

namespace ProjectManager.Areas.Scrum.ViewModels
{
    public class EditTaskViewModel
    {
        public ProjectTask Task { get; set; }
        public bool IsMyTask { get; set; } = false;
        public bool CanIAssingThisTask { get; set; } = false;
        public bool HaveActivePeriod { get; set; }
        public double AlreadySpentTime { get; set; }
    }
}
