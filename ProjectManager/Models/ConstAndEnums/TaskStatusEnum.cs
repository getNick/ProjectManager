using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models.ConstAndEnums
{
    public enum TaskStatusEnum
    {
        [Display(Name = "Backlog")]
        Backlog,
        [Display(Name = "To Do")]
        ToDo,
        [Display(Name = "In Progress")]
        InProgress,
        [Display(Name = "Testing")]
        Testing,
        [Display(Name = "Done")]
        Done,

    }
}
