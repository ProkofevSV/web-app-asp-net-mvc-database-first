using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ToDoDBFirst.Models;
using ToDoDBFirst.Models.Entities;
using DayOfWeek = ToDoDBFirst.Models.Entities.DayOfWeek;

namespace ToDoDBFirst.Controllers
{
    public class DayOfWeeksController: Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var db = new ToDoEntities();
            var dayOfWeeks = MappingDayOfWeeks(db.DayOfWeeks.ToList());
            return View(dayOfWeeks);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var dayOfWeek = new DayOfWeekViewModel();

            return View(dayOfWeek);
        }

        [HttpPost]
        public ActionResult Create(DayOfWeekViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var db = new ToDoEntities();
            var dayOfWeek = new DayOfWeek();
            MappingDayOfWeek(model, dayOfWeek);

            db.DayOfWeeks.Add(dayOfWeek);
            db.SaveChanges();


            return RedirectPermanent("/DayOfWeeks/Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = new ToDoEntities();
            var dayOfWeek = db.DayOfWeeks.FirstOrDefault(x => x.Id == id);
            if (dayOfWeek == null)
                return RedirectPermanent("/DayOfWeeks/Index");

            db.DayOfWeeks.Remove(dayOfWeek);
            db.SaveChanges();

            return RedirectPermanent("/DayOfWeeks/Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new ToDoEntities();
            var dayOfWeek = MappingDayOfWeeks(db.DayOfWeeks.Where(x => x.Id == id).ToList()).FirstOrDefault(x => x.Id == id);
            if (dayOfWeek == null)
                return RedirectPermanent("/DayOfWeeks/Index");

            return View(dayOfWeek);
        }

        [HttpPost]
        public ActionResult Edit(DayOfWeekViewModel model)
        {

            var db = new ToDoEntities();
            var dayOfWeek = db.DayOfWeeks.FirstOrDefault(x => x.Id == model.Id);
            if (dayOfWeek == null)
            {
                ModelState.AddModelError("Id", "Группа не найдена");
            }
            if (!ModelState.IsValid)
                return View(model);

            MappingDayOfWeek(model, dayOfWeek);


            db.Entry(dayOfWeek).State = EntityState.Modified;
            db.SaveChanges();


            return RedirectPermanent("/DayOfWeeks/Index");
        }

        private void MappingDayOfWeek(DayOfWeekViewModel sourse, DayOfWeek destination)
        {
            destination.DayOfWeekName = sourse.DayOfWeekName;
        }

        private List<DayOfWeekViewModel> MappingDayOfWeeks(List<DayOfWeek> dayOfWeeks)
        {
            var result = dayOfWeeks.Select(x => new DayOfWeekViewModel()
            {
                Id = x.Id,
                DayOfWeekName = x.DayOfWeekName
            }).ToList();

            return result;
        }
    }
}