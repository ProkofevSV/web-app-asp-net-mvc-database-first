using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoDBFirst.Extensions;
using ToDoDBFirst.Models.Attributes;
using ToDoDBFirst.Models.Entities;

namespace ToDoDBFirst.Models
{
    public class TaskViewModel
    {
        /// <summary>
        /// Id
        /// </summary> 
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        /// <summary>
        /// День недели
        /// </summary> 
        [ScaffoldColumn(false)]
        public virtual ICollection<Entities.DayOfWeek> DayOfWeeks { get; set; }

        [ScaffoldColumn(false)]
        public List<int> DayOfWeekIds { get; set; }

        [Display(Name = "День недели", Order = 2)]
        [UIHint("MultipleDropDownList")]
        [TargetProperty("DayOfWeekIds")]
        [NotMapped]
        public IEnumerable<SelectListItem> DayOfWeekDictionary
        {
            get
            {
                var db = new ToDoEntities();
                var query = db.DayOfWeeks;

                if (query != null)
                {
                    var Ids = query.Where(s => s.Tasks.Any(ss => ss.Id == Id)).Select(s => s.Id).ToList();
                    var dictionary = new List<SelectListItem>();
                    dictionary.AddRange(query.ToSelectList(c => c.Id, c => $"{c.DayOfWeekName}", c => Ids.Contains(c.Id)));
                    return dictionary;
                }

                return new List<SelectListItem> { new SelectListItem { Text = "", Value = "" } };
            }
        }

        /// <summary>
        /// Название
        /// </summary>    
        [Required]
        [Display(Name = "Задача", Order = 2)]
        public String Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>    
        [Required]
        [Display(Name = "Описание", Order = 2)]
        [UIHint("TextArea")]
        public string Description { get; set; }


        /// <summary>
        /// Исполнитель
        /// </summary> 
        [ScaffoldColumn(false)]
        public int PerformerId { get; set; }
        [ScaffoldColumn(false)]
        public virtual Performer Performer { get; set; }
        [Display(Name = "Исполнитель", Order = 3)]
        [UIHint("DropDownList")]
        [TargetProperty("PerformerId")]
        [NotMapped]
        public IEnumerable<SelectListItem> PerformerDictionary
        {
            get
            {
                var db = new ToDoEntities();
                var query = db.Performers;

                if (query != null)
                {
                    var dictionary = new List<SelectListItem>();
                    dictionary.AddRange(query.OrderBy(d => d.Name).ToSelectList(c => c.Id, c => c.Name, c => c.Id == PerformerId));
                    return dictionary;
                }

                return new List<SelectListItem> { new SelectListItem { Text = "", Value = "" } };
            }
        }
    }
}