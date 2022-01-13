using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoDBFirst.Extensions;
using ToDoDBFirst.Models.Attributes;

namespace ToDoDBFirst.Models
{
    public class DayOfWeekViewModel
    {
        /// <summary>
        /// Id
        /// </summary> 
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        /// <summary>
        /// Название дня недели
        /// </summary>    
        [Required]
        [Display(Name = "День недели", Order = 5)]
        public string DayOfWeekName { get; set; }
                
        
    }
}