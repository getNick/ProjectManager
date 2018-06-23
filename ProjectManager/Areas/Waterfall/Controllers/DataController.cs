using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Areas.Waterfall.Models;
using ProjectManager.Data;

namespace ProjectManager.Areas.Waterfall.Controllers
{
    public class DataController : Controller
    {
        private ApplicationDbContext _db;

        public DataController(ApplicationDbContext context)
        {
            _db = context;
        }
        [HttpGet]
        public GanttDto Get()
        {
            return new GanttDto
            {
                data = new TaskController(_db).Get(),
                links = new LinkController(_db).Get()
            };
        }
    }
}