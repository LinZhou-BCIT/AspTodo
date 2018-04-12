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
        public float ItemOrder { get; set; }
        public string ItemName { get; set; }
        public bool Completed { get; set; }
        public DateTime? DueDate { get; set; }
        public string Notes { get; set; }
        public virtual TodoList TodoList { get; set; }
    }

    public class TodoItemAPIModel
    {
        public string ItemID { get; set; }
        public string ListID { get; set; }
        public float ItemOrder { get; set; }
        public string ItemName { get; set; }
        public bool Completed { get; set; }
        public DateTime DueDate { get; set; }
        public string Notes { get; set; }
    }
}
