using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public class SelectorItem
    {
        public string ID { get; set; }
        public string CategoryId { get; set; }
        public string Text { get; set; }
        public bool Expanded { get; set; }
    }
}
