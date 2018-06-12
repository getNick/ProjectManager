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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectManager.Data;
using ProjectManager.Models;
using ProjectManager.Models.ProjectViewModels;
using ProjectManager.Services;
using System.Security.Cryptography;
using System.Text;
using ProjectManager.Models.ConstAndEnums;

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

        public  IActionResult Save(string name, string description,string deadline,string budget)
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
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _db.Users.FirstOrDefault(x => x.Id == userId);
            var projLead = new Participant()
            {
                Person = user,
                Role = RoleEnum.Manager,
            };
            var proj = new Project()
            {
                ProjectLead=projLead,
                Name = name,
                Description = description,
                ImageUrl = myFile.FileName,
                Budget = double.Parse(budget),
                Deadline = DateTime.Parse(deadline)
            };
            _db.Projects.Add(proj);
            _db.SaveChanges();
            HttpContext.Session.Set(SessionKeys.Project,proj);
            return RedirectToAction("Show",new{ projectId=proj.Id});
        }


        public IActionResult AddDepartment(int projectId, string name, string description)
        {
            var proj=_db.Projects.FirstOrDefault(x => x.Id == projectId);
            if (proj.Departments == null)
            {
                proj.Departments=new List<Department>();
            }

            var newDep = new Department()
            {
                Name = name,
                Description = description
            };
            proj.Departments.Add(newDep);
            _db.Projects.Update(proj);
            _db.SaveChanges();
            return RedirectToAction("Show", new { projectId = proj.Id, selectedDepartmentId=newDep.Id });
        }

        public async Task<IActionResult> AddTeam(int? departmentId, string name, string description,string[]email,int? teamId)
        {
            var dep = _db.Departments.Include(x => x.Project).Include(x=>x.Teams).ThenInclude(x=>x.Participants).FirstOrDefault(x => x.Id == departmentId);
            if (dep.Teams == null)
            {
                dep.Teams = new List<Team>();
            }

            var newTeam = new Team()
            {
                Name = name,
                Description = description,
                Participants = new List<Participant>(),
                
            };
            foreach (var e in email)
            {
                var user= _db.Users.FirstOrDefault(x => x.Email == e);
              
                if (user == null)
                {
                    user = new ApplicationUser { UserName =e, Email = e };
                    var result = await _userManager.CreateAsync(user, "!QAZ2wsx");
                }
                newTeam.Participants.Add(new Participant()
                {
                    Person = user,
                });
            }
            dep.Teams.Add(newTeam);
            _db.Departments.Update(dep);
            _db.SaveChanges();
            return RedirectToAction("Show", new { projectId = dep.Project.Id, selectedDepartmentId =dep.Id, selectedTeamId=newTeam.Id });
        }

        public IActionResult Show(int? projectId,int? selectedDepartmentId,int? selectedTeamId)
        {
            var proj2 = HttpContext.Session.Get<Project>(SessionKeys.Project);
            var proj = _db.Projects.Include(x=>x.Departments).ThenInclude(x=>x.Teams).ThenInclude(x=>x.Participants).FirstOrDefault(x => x.Id == projectId);
            var vm = new EditProjectViewModel()
            {
                Project = proj,
                SelectedDepartmentId = selectedDepartmentId,
                SelectedTeamId = selectedTeamId,
            };
            return View("Show", vm);
        }
        public IActionResult Delete(int? projectId)
        {
            return View();
        }
    }
}