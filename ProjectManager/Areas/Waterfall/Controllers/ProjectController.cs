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

namespace ProjectManager.Waterfall.Controllers
{
    [Area("Waterfall")]
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
        public IActionResult Index(int? selectedProjId)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _db.Users.FirstOrDefault(x => x.Id == userId);
            if (selectedProjId != null)
            {
                user.LastSelectedProjectId = selectedProjId;
            }
            var vm = new ShowProjectViewModel();
            vm.AllProjects = _db.Participants.Include(x => x.User).Include(x => x.Project)
                .Where(x => x.User.Id == userId).Select(x => x.Project).ToList();
            if (user.LastSelectedProjectId != null)
            {
                vm.SelectedProject = vm.AllProjects?.FirstOrDefault(x => x.Id == user.LastSelectedProjectId);
            }
            else
            {
                vm.SelectedProject = vm.AllProjects?.FirstOrDefault();
            }

            if (vm.SelectedProject != null)
            {
                user.LastSelectedProjectId = vm.SelectedProject.Id;
            }
            _db.Users.Update(user);
            _db.SaveChanges();

            return View(vm);
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Select(int? projectId)
        {

            return RedirectToAction("Index", new {selectedProjId = projectId});
        }

        public  IActionResult Save(string name, string description,string deadline,string budget,string typeProj)
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
            
            var proj = new Project()
            {
                //ProjectLead=participant,
                Name = name,
                Description = description,
                ImageUrl = myFile.FileName,
                Budget = double.Parse(budget),
                Deadline = DateTime.Parse(deadline),
                ProjectType = (ProjectTypeEnum)Enum.Parse(typeof(ProjectTypeEnum),typeProj),
                Activities = new ProjectActivities(){List =new List<ProjectActivity>()},
            };
            var participant = new Participant()
            {
                Project = proj,
                Role = RoleEnum.Manager,
                User = user,
            };
            proj.Activities.List.Add(new ProjectActivity()
            {
                Initializer = participant,
                Time = DateTime.Now,
                Description = 
                              "< a href = 'https://www.tutorialspoint.com' > "+ user.FullName + " </ a > "+"<p> created this project.</p>"

            });
           
            _db.Participants.Add(participant);
            _db.Projects.Add(proj);
            _db.SaveChanges();
            user.LastSelectedProjectId = proj.Id;
            _db.Users.Update(user);
            _db.SaveChanges();
            return RedirectToAction("Show");
        }


        public IActionResult AddDepartment(string name, string description, string headOfDepartment)
        {

            var userId = _userManager.GetUserId(HttpContext.User);
            var applicationUser = _db.Users.FirstOrDefault(x => x.Id == userId);
            var proj = _db.Projects.Include(x => x.Departments).Include(x=>x.Participants).ThenInclude(x=>x.User).FirstOrDefault(x => x.Id == applicationUser.LastSelectedProjectId);
            
            if (proj.Departments == null)
            {
                proj.Departments = new List<Department>();
            }
            var user = _db.Users.FirstOrDefault(x => x.Email == headOfDepartment) ?? CreateNewApplicationUser(headOfDepartment).Result;
            
            var newDep = new Department()
            {
                Name = name,
                Description = description,
                //HeadOfDepartment = participant,
                Project = proj,
                Activities = new ProjectActivities() { List = new List<ProjectActivity>() },
            };
            newDep.Activities.List.Add(new ProjectActivity()
            {
                Initializer = proj.Participants.FirstOrDefault(x=>x.User.Id==applicationUser.Id),
                Time = DateTime.Now,
                Description =
                    "< a href = 'https://www.tutorialspoint.com' > " + applicationUser.FullName + " </ a > " + "<p> created new department "+newDep.Name+".</p>"

            });
            var participant = new Participant()
            {
                Project = proj,
                Department = newDep,
                Role = RoleEnum.Manager,
                User = user,
            };
            newDep.Activities.List.Add(new ProjectActivity()
            {
                Initializer = proj.Participants.FirstOrDefault(x => x.User.Id == applicationUser.Id),
                Time = DateTime.Now,
                Description =
                    "< a href = 'https://www.tutorialspoint.com' > " + user.FullName + " </ a > " + "<p> new head of department " + newDep.Name + ".</p>"

            });
            _db.Participants.Add(participant);
            proj.Departments.Add(newDep);
            _db.Projects.Update(proj);
            _db.SaveChanges();
            return RedirectToAction("Show", new { selectedDepartmentId = newDep.Id });
        }

        private async Task<ApplicationUser> CreateNewApplicationUser(string email, string pass = "!QAZ2wsx")
        {
            var user = new ApplicationUser { UserName = email,FullName = email,Email = email };
            var result = await _userManager.CreateAsync(user, pass);
            return user;
        }
        public IActionResult AddTeam(int? departmentId, string name, string description, string teamLead, string[] email, int? teamId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var applicationUser = _db.Users.FirstOrDefault(x => x.Id == userId);
            var creator = _db.Participants.FirstOrDefault(x =>
                (x.Project.Id == applicationUser.LastSelectedProjectId) & (x.User.Id == applicationUser.Id));
            var dep = _db.Departments.Include(x => x.Project).Include(x => x.Teams).ThenInclude(x => x.Participants).FirstOrDefault(x => x.Id == departmentId);
            if (dep.Teams == null)
            {
                dep.Teams = new List<Team>();
            }
           
            var newTeam = new Team()
            {
                Name = name,
                Description = description,
                Participants = new List<Participant>(),
                Activities = new ProjectActivities() { List = new List<ProjectActivity>() },
            };
            var participant = new Participant()
            {
                Project = dep.Project,
                Department = dep,
                Team = newTeam,
                Role = RoleEnum.Manager,
                User = _db.Users.FirstOrDefault(x => x.Email == teamLead) ?? CreateNewApplicationUser(teamLead).Result,
            };
            newTeam.Participants.Add(participant);
            newTeam.Activities.List.Add(new ProjectActivity()
            {
                Initializer = creator,
                Time = DateTime.Now,
                Description =
                    "< a href = 'https://www.tutorialspoint.com' > " + participant.User.FullName + " </ a > " + "<p> new teamlead of " + newTeam.Name + ".</p>"

            });
            foreach (var e in email)
            {
                var user = _db.Users.FirstOrDefault(x => x.Email == e) ?? CreateNewApplicationUser(e).Result;
                var newParticipant = new Participant()
                {
                    Department = dep,
                    Project = dep.Project,
                    Role = RoleEnum.Developer,
                    Team = newTeam,
                    User = user,
                };
                newTeam.Participants.Add(newParticipant);
                newTeam.Activities.List.Add(new ProjectActivity()
                {
                    Initializer = creator,
                    Time = DateTime.Now,
                    Description =
                        "< a href = 'https://www.tutorialspoint.com' > " + creator.User.FullName + " </ a > " + "<p> invite to team " + user.FullName + ".</p>"

                });
            }
            
            dep.Teams.Add(newTeam);
            _db.Departments.Update(dep);
            _db.SaveChanges();
            return RedirectToAction("Show", new { selectedDepartmentId = dep.Id, selectedTeamId = newTeam.Id });
        }

        public IActionResult AddCustomers(string[] customerEmail)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var applicationUser = _db.Users.FirstOrDefault(x => x.Id == userId);
            var creator = _db.Participants.FirstOrDefault(x =>
                (x.Project.Id == applicationUser.LastSelectedProjectId) & (x.User.Id == applicationUser.Id));
            var proj = _db.Projects.Include(x=>x.Activities).ThenInclude(x=>x.List).Include(x => x.Departments).ThenInclude(x => x.Teams).ThenInclude(x => x.Participants).FirstOrDefault(x => x.Id == applicationUser.LastSelectedProjectId);
            foreach (var e in customerEmail)
            {
                var user = _db.Users.FirstOrDefault(x => x.Email == e) ?? CreateNewApplicationUser(e).Result;
                var newParticipant = new Participant()
                {
                    Project = proj,
                    Role = RoleEnum.Customer,
                    User = user,
                };
                _db.Participants.Add(newParticipant);
                proj.Activities.List.Add(new ProjectActivity()
                {
                    Initializer = creator,
                    Time = DateTime.Now,
                    Description =
                        "< a href = 'https://www.tutorialspoint.com' > " + creator.User.FullName + " </ a > " + "<p> invite customer " + user.FullName + ".</p>"
                });
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Show(int? selectedDepartmentId,int? selectedTeamId)
        {
            var part = _db.Participants.Include(x => x.User).Include(x => x.Project).ToList();
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _db.Users.FirstOrDefault(x => x.Id == userId);
            var proj = _db.Projects.Include(x=>x.Departments).ThenInclude(x=>x.Teams).ThenInclude(x=>x.Participants).FirstOrDefault(x => x.Id == user.LastSelectedProjectId);
            var vm = new EditProjectViewModel()
            {
                Project = proj,
                SelectedDepartmentId = selectedDepartmentId,
                SelectedTeamId = selectedTeamId,
               // Customers = proj.Participants?.Where(x=>x.Role==RoleEnum.Customer).ToList(),
            };
            return View("Show", vm);
        }
    }
}