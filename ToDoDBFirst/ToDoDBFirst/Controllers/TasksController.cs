using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ToDoDBFirst.Models;
using ToDoDBFirst.Models.Entities;

namespace ToDoDBFirst.Controllers
{
    public class TasksController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var db = new ToDoEntities();
            var tasks = MappingTasks(db.Tasks.ToList());

            return View(tasks);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var task = new TaskViewModel();
            return View(task);
        }

        [HttpPost]
        public ActionResult Create(TaskViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var db = new ToDoEntities();

            var task = new Task();
            MappingTask(model, task, db);


            db.Tasks.Add(task);
            db.SaveChanges();

            return RedirectPermanent("/Tasks/Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = new ToDoEntities();
            var task = db.Tasks.FirstOrDefault(x => x.Id == id);
            if (task == null)
                return RedirectPermanent("/Tasks/Index");

            db.Tasks.Remove(task);
            db.SaveChanges();

            return RedirectPermanent("/Tasks/Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new ToDoEntities();
            var task = MappingTasks(db.Tasks.Where(x => x.Id == id).ToList()).FirstOrDefault(x => x.Id == id);
            if (task == null)
                return RedirectPermanent("/Tasks/Index");

            return View(task);
        }

        [HttpPost]
        public ActionResult Edit(TaskViewModel model)
        {
            var db = new ToDoEntities();
            var task = db.Tasks.FirstOrDefault(x => x.Id == model.Id);
            if (task == null)
                ModelState.AddModelError("Id", "Пара не найдена");

            if (!ModelState.IsValid)
                return View(model);

            MappingTask(model, task, db);

            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectPermanent("/Tasks/Index");
        }

        private void MappingTask(TaskViewModel sourse, Task destination, ToDoEntities db)
        {
            destination.Name = sourse.Name;          
            destination.PerformerId = sourse.PerformerId;
            destination.Performer = sourse.Performer;
            destination.Description = sourse.Description;


            if (destination.DayOfWeeks != null)
                destination.DayOfWeeks.Clear();

            if (sourse.DayOfWeekIds != null && sourse.DayOfWeekIds.Any())
                destination.DayOfWeeks = db.DayOfWeeks.Where(s => sourse.DayOfWeekIds.Contains(s.Id)).ToList();           
        }

        [HttpGet]
        public ActionResult GetImage(int id)
        {
            var db = new ToDoEntities();
            var image = db.PerformerImages.FirstOrDefault(x => x.Id == id);
            if (image == null)
            {
                FileStream fs = System.IO.File.OpenRead(Server.MapPath(@"~/Content/Images/not-foto.png"));
                byte[] fileData = new byte[fs.Length];
                fs.Read(fileData, 0, (int)fs.Length);
                fs.Close();

                return File(new MemoryStream(fileData), "image/jpeg");
            }

            return File(new MemoryStream(image.Data), image.ContentType);
        }

        [HttpGet]
        public ActionResult Description(int id)
        {
            var db = new ToDoEntities();
            var task = db.Tasks.FirstOrDefault(x => x.Id == id);
            if (task == null)
                return RedirectPermanent("/Tasks/Index");

            return View(task);
        }
        private List<TaskViewModel> MappingTasks(List<Task> tasks)
        {
            var result = tasks.Select(x => new TaskViewModel()
            {
                Id = x.Id,
                Description = x.Description,
                PerformerId = x.PerformerId,
                Performer = x.Performer,
                Name = x.Name,
                DayOfWeeks = x.DayOfWeeks

            }).ToList();

            return result;
        }
    }
}