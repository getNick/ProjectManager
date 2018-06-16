using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models.ConstAndEnums
{
    public enum TaskTypeEnum
    {
        [Display(Description = "task.png")]
        Task,
        [Display(Description = "bug.png")]
        Bug,
        [Display(Description = "investigation.png")]
        Investigation,
        [Display(Description = "testing.png")]
        Testing,
    }
}
