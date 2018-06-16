using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ProjectManager.Models.ConstAndEnums
{
    public enum PriorityEnum
    {
        [Display(Description = "p5.png")]
        Highest,
        [Display(Description = "p4.png")]
        High,
        [Display(Description = "p3.png")]
        Medium,
        [Display(Description = "p2.png")]
        Low,
        [Display(Description = "p1.png")]
        Lowest,
    }

    public static class DisplayAnotation
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType().GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                .Name;
        }
        public static string GetDisplayDescription(this Enum enumValue)
        {
            return enumValue.GetType().GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                .Description;
        }
    }
}
