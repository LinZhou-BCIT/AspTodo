using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspTodo.Models
{
    public class TodoItem
    {
        public string ListID { get; set; }
        public int ItemOrder { get; set; }
        public string ItemName { get; set; }
        public bool Completed { get; set; }
        public DateTime DueDate { get; set; }
        public string Notes { get; set; }
        public virtual TodoList TodoList { get; set; }
    }
}
