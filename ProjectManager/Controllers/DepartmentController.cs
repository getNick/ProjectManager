using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit(int? departmentId)
        {
            return View();
        }
        public IActionResult Save(int? departmentId)
        {
            return View();
        }

        public IActionResult Show(int? departmentId)
        {
            return View();
        }
        public IActionResult Delete(int? departmentId)
        {
            return View();
        }
    }
}