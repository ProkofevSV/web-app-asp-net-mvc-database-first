using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ToDoDBFirst.Models;
using ToDoDBFirst.Models.Entities;

namespace ToDoDBFirst.Controllers
{
    public class PerformersController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var db = new ToDoEntities();
            var performers = MappingPerformers(db.Performers.ToList());
          
            return View(performers);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var performer = new PerformerViewModel();
            return View(performer);
        }

        [HttpPost]
        public ActionResult Create(PerformerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var db = new ToDoEntities();


            if (model.PerformerImageFile != null)
            {
                var data = new byte[model.PerformerImageFile.ContentLength];
                model.PerformerImageFile.InputStream.Read(data, 0, model.PerformerImageFile.ContentLength);

                model.PerformerImage = new PerformerImage()
                {
                    Guid = Guid.NewGuid(),
                    DateChanged = DateTime.Now,
                    Data = data,
                    ContentType = model.PerformerImageFile.ContentType,
                    FileName = model.PerformerImageFile.FileName
                };
            }

            var performer = new Performer();
            MappingPerformer(model, performer, db);

            db.Performers.Add(performer);
            db.SaveChanges();

            return RedirectPermanent("/Performers/Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = new ToDoEntities();
            var performer = db.Performers.FirstOrDefault(x => x.Id == id);
            if (performer == null)
                return RedirectPermanent("/Performers/Index");

            db.Performers.Remove(performer);
            db.SaveChanges();

            return RedirectPermanent("/Performers/Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new ToDoEntities();
            var performer = MappingPerformers(db.Performers.Where(x => x.Id == id).ToList()).FirstOrDefault(x => x.Id == id);
            if (performer == null)
                return RedirectPermanent("/Performers/Index");

            return View(performer);
        }

        [HttpPost]
        public ActionResult Edit(PerformerViewModel model)
        {
            var db = new ToDoEntities();
            var performer = db.Performers.FirstOrDefault(x => x.Id == model.Id);
            if (performer == null)
                ModelState.AddModelError("Id", "Преподаватель не найден");

            if (!ModelState.IsValid)
                return View(model);

            MappingPerformer(model, performer, db);

            db.Entry(performer).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectPermanent("/Performers/Index");
        }

        private void MappingPerformer(PerformerViewModel sourse, Performer destination, ToDoEntities db)
        {
            destination.Name = sourse.Name;
            destination.Sex = (int)sourse.Sex;

            if (sourse.PerformerImageFile != null)
            {
                var image = db.PerformerImages.FirstOrDefault(x => x.Id == sourse.Id);
                if (image != null)
                    db.PerformerImages.Remove(image);

                var data = new byte[sourse.PerformerImageFile.ContentLength];
                sourse.PerformerImageFile.InputStream.Read(data, 0, sourse.PerformerImageFile.ContentLength);

                destination.PerformerImage = new PerformerImage()
                {
                    Guid = Guid.NewGuid(),
                    DateChanged = DateTime.Now,
                    Data = data,
                    ContentType = sourse.PerformerImageFile.ContentType,
                    FileName = sourse.PerformerImageFile.FileName
                };
            }
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

        private List<PerformerViewModel> MappingPerformers(List<Performer> performers)
        {
            var result = performers.Select(x => new PerformerViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Sex = (Sex)x.Sex,
                PerformerImage = x.PerformerImage
            }).ToList();

            return result;
        }
    }
}