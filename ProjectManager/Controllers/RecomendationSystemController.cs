using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;
using ProjectManager.Models;
using ProjectManager.Models.ProductKnowledge;

namespace ProjectManager.Controllers
{
    public class RecomendationSystemController : Controller
    {
        private ApplicationDbContext _db;

        public RecomendationSystemController(ApplicationDbContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateVariable(string name)
        {
            if (name != null)
            {
                var variable = new ProjectManager.Models.ProductKnowledge.Variable() { Name = name };
                _db.ProductKnowledgeVariables.Add(variable);
                _db.SaveChanges();
            }
            var list = _db.ProductKnowledgeVariables.ToList();
            return View("CreateVariable", list);
        }
        public IActionResult CreateRule(string[] variable, string[] sign, string[] value, string rigthValiable, string rightVarValue)
        {
            if (rigthValiable != null)
            {
                var rule=new Rule();
                rule.Expressions=new List<Expression>();
                for (int i = 0; i < variable.Length; i++)
                {
                    var expression = new Expression()
                    {
                        LeftVariable = _db.ProductKnowledgeVariables.FirstOrDefault(x => x.Id == Int32.Parse(variable[i])),
                        Ratio = (Ratio)Enum.Parse(typeof(Ratio),sign[i]),
                        RightVariable = value[i],
                    };
                    rule.Expressions.Add(expression);
                }

                rule.RightVariable = _db.ProductKnowledgeVariables.FirstOrDefault(x => x.Id == Int32.Parse(rigthValiable));
                rule.Result = rightVarValue;
                _db.ProductKnowledgeRules.Add(rule);
                _db.SaveChanges();
            }
            var vm = new CreateRulePage()
            {
                Rules = _db.ProductKnowledgeRules.Include(x=>x.RightVariable).Include(x=>x.Expressions).ThenInclude(x=>x.LeftVariable).ToList(),
                Variables = _db.ProductKnowledgeVariables.ToList(),
            };
            return View("CreateRule", vm);
        }

    }
}