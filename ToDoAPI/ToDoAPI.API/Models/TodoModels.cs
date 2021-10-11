using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ToDoAPI.DATA.EF;

namespace ToDoAPI.API.Models
{
    public class TodoViewModels
    {

        [Key]
        public int TodoId { get; set; }
        [Required]
        [StringLength(150, ErrorMessage = "Action is required. **")]
        public string Action { get; set; }
        [Required]
        public bool Done { get; set; }
        [Required]
        public Nullable<int> CategoryId { get; set; }

        public virtual CategoryViewModel Category { get; set; }

    }

    public class CategoryViewModel
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(25, ErrorMessage = "** Max 25 characters")]
        public string CategoryName { get; set; }
        [StringLength(50, ErrorMessage = "** Max 50 characters")]
        public string CategoryDescription { get; set; }
    }

}