﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public class ProjectActivities:BaseModel
    {
        public List<ProjectActivity> List { get; set; }
    }
}
