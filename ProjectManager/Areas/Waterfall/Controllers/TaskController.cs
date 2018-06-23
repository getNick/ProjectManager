using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Areas.Waterfall.Models;
using ProjectManager.Data;

namespace ProjectManager.Areas.Waterfall.Controllers
{
    public class TaskController : Controller
    {
        private ApplicationDbContext _db;

        public TaskController(ApplicationDbContext context)
        {
            _db = context;
        }

        // GET api/Task
        public IEnumerable<TaskDto> Get()
        {
            return _db.GanttTasks
                .ToList()
                .Select(t => (TaskDto)t);
        }

        // GET api/Task/5
        [HttpGet]
        public TaskDto Get(int id)
        {
            return (TaskDto)_db
                .GanttTasks
                .Find(id);
        }

        // PUT api/Task/5
        [HttpPut]
        public IActionResult EditTask(int id, TaskDto taskDto)
        {
            var updatedTask = (Task)taskDto;
            updatedTask.Id = id;
            _db.Entry(updatedTask).State = EntityState.Modified;
            _db.SaveChanges();

            return Ok(new
            {
                action = "updated"
            });
        }

        // POST api/Task
        [HttpPost]
        public IActionResult CreateTask(TaskDto taskDto)
        {
            var newTask = ( Task)taskDto;

            _db.GanttTasks.Add(newTask);
            _db.SaveChanges();

            return Ok(new
            {
                tid = newTask.Id,
                action = "inserted"
            });
        }

        // DELETE api/Task/5
        [HttpDelete]
        public IActionResult DeleteTask(int id)
        {
            var task = _db.GanttTasks.Find(id);
            if (task != null)
            {
                _db.GanttTasks.Remove(task);
                _db.SaveChanges();
            }

            return Ok(new
            {
                action = "deleted"
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}