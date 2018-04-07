using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspTodo.Models
{
    public class TodoList
    {
        public string ListID { get; set; }
        public string ListName { get; set; }
        public string OwnerID { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual ICollection<TodoItem> TodoItems { get; set; }
        public virtual ICollection<Sharing> Sharings { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
    }
}
