using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspTodo.Models.Dtos
{
    public class TodoItemCreateDto
    {
        [Required]
        public string ListID { get; set; }
        public int? ItemOrder { get; set; }
        [Required]
        public string ItemName { get; set; }
        public bool? Completed { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }
        public string Notes { get; set; }
    }
}
