using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit(int? taskId)
        {
            return View();
        }
        public IActionResult Save(int? taskId)
        {
            return View();
        }

        public IActionResult Show(int? taskId)
        {
            return View();
        }
        public IActionResult Delete(int? taskId)
        {
            return View();
        }
    }
}