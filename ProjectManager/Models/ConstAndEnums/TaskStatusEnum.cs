using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models.ConstAndEnums
{
    public enum TaskStatusEnum
    {
        [Display(Name = "In progress")]
        InProgress,
        [Display(Name = "Re-opened")]
        ReOpened,
        [Display(Name = "In risk!")]
        InRisk,
        [Display(Name = "Need review")]
        NeedReview,
        [Display(Name = "Pending")]
        Pending,
        [Display(Name = "Done")]
        Done,

    }
}
