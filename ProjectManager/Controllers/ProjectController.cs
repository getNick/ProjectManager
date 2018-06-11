using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectManager.Data;
using ProjectManager.Models;
using ProjectManager.Services;

namespace ProjectManager.Controllers
{
    public class ProjectController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private ApplicationDbContext _db;
        private IHostingEnvironment _appEnvironment;

        public ProjectController(ApplicationDbContext context,UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, IHostingEnvironment appEnvironment)
        {
            _db = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _appEnvironment = appEnvironment;
        }
        public IActionResult Index()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit(int? projectId)
        {
            return View();
        }

        public  IActionResult Save(string name, string description,string deadline)
        {
            var myFile = Request.Form.Files["files"];
            var targetLocation = Path.Combine(_appEnvironment.WebRootPath, "images");

            try
            {
                var path = Path.Combine(targetLocation, myFile.FileName);
                using (var fileStream = System.IO.File.Create(path))
                {
                    myFile.CopyTo(fileStream);
                }
            }
            catch
            {
                Response.StatusCode = 400;
            }
            var proj = new Project()
            {
                Name = name,
                Description = description,
                ImageUrl = myFile.FileName,
            };
            _db.Projects.Add(proj);
            _db.SaveChanges();
            return View("AddTeams",proj);
        }

        public IActionResult Show(int? projectId)
        {
            return View();
        }
        public IActionResult Delete(int? projectId)
        {
            return View();
        }
    }
}