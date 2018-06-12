using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;
using ProjectManager.Models;
using ProjectManager.Models.ConstAndEnums;
using ProjectManager.Models.DashboardViewModels;

namespace ProjectManager.Controllers
{
    public class DashboardController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _db = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = _userManager.GetUserId(HttpContext.User);
            var participants = _db.Participants.Include(x=>x.Person).Include(x=>x.Project).Where(x => x.Person.Id == userId).ToList();
            if (participants.Count==0)
            {
                return View("AvailableProjectNotFound");
            }

            PersonalDashboard dashboard=new PersonalDashboard();
            //if (selectedProjectId !=null)
            //{
            //    var partisipantForSelectedProj = participants.FirstOrDefault(x => x.Project.Id == selectedProjectId);
            //    if (partisipantForSelectedProj == null)
            //    {
            //        return View("AccessToProjectDenied");
            //    }
            //    dashboard.ActiveProject =partisipantForSelectedProj.Project ;
            //    dashboard.AssignedForMe = _db.Tasks.Where(x => (x.Assignee.Id == partisipantForSelectedProj.Id)&(x.Status!=TaskStatusEnum.Done)).ToList();
            //    dashboard.ComplitedTasks= _db.Tasks.Where(x => (x.Assignee.Id == partisipantForSelectedProj.Id) & (x.Status == TaskStatusEnum.Done)).ToList();
            //}
            
            return View(dashboard);
        }
    }
}