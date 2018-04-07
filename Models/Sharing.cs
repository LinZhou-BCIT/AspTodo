using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspTodo.Models
{
    public class Sharing
    {
        public string ShareeID { get; set; }
        public string ListID { get; set; }
        public virtual ApplicationUser Sharee { get; set; }
        public virtual TodoList TodoList { get; set; }

    }

    public class SharingAPIModel
    {
        public string ShareeID { get; set; }
        public string ListID { get; set; }
    }
}
