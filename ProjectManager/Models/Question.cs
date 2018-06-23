﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public class Question
    {
        public string Text { get; set; }
        public List<string> Answers { get; set; }
    }
}
