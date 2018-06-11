using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public class Comment:BaseModel
    {
        public Comments Owner { get; set; }
        public Participant Autor { get; set; }
        public DateTime Time { get; set; }
    }
}
