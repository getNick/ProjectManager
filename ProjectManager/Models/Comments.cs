using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public class Comments:BaseModel
    {
        public List<Comment> List { get; set; }
    }
}
