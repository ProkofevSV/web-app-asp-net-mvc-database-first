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
    public class PerformerViewModel
    {
        ///<summary>
        /// Id
        /// </summary> 
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        ///<summary>
        /// Имя Исполнителя
        /// </summary> 
        [Required]
        [Display(Name = "Исполнитель", Order = 5)]
        public string Name { get; set; }

        /// <summary>
        /// Пол
        /// </summary> 
        [ScaffoldColumn(false)]
        public Sex Sex { get; set; }

        [Display(Name = "Пол", Order = 50)]
        [UIHint("DropDownList")]
        [TargetProperty("Sex")]
        [NotMapped]
        public IEnumerable<SelectListItem> SexDictionary
        {
            get
            {
                var dictionary = new List<SelectListItem>();

                foreach (Sex type in Enum.GetValues(typeof(Sex)))
                {
                    dictionary.Add(new SelectListItem
                    {
                        Value = ((int)type).ToString(),
                        Text = type.GetDisplayValue(),
                        Selected = type == Sex
                    });
                }

                return dictionary;
            }
        }

        /// <summary>
        /// Фото исполнителя
        /// </summary> 
        [ScaffoldColumn(false)]
        public virtual PerformerImage PerformerImage { get; set; }

        [Display(Name = "Фото исполнителя", Order = 60)]
        [NotMapped]
        public HttpPostedFileBase PerformerImageFile { get; set; }
                
    }
}