using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public class CustomerWish:BaseModel
    {
        public Project Project { get; set; }
        public DateTime AddingTime { get; set; }
        public DateTime Deadline { get; set; }
        public AdditionalFiles AdditionalFiles { get; set; }
    }
}
