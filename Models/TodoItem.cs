using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspTodo.Models
{
    public class TodoItem
    {
        public string ItemID { get; set; }
        public string ListID { get; set; }
        [Required]
        public int ItemOrder { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Required]
        public bool Completed { get; set; }
        public DateTime? DueDate { get; set; }
        public string Notes { get; set; }
        public virtual TodoList TodoList { get; set; }
    }

    public class TodoItemAPIModel
    {
        public string ItemID { get; set; }
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
