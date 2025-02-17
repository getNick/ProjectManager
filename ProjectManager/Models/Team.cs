﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public class Team:BaseModel
    {
        public Department Department { get; set; }
        public List<Participant> Participants { get; set; }
        public List<ProjectTask> Tasks { get; set; }
        public List<Sprint> Sprints { get; set; }
        public ProjectActivities Activities { get; set; }
    }
}
