using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspTodo.Models
{
    public class Invitation
    {
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public string ListID { get; set; }
        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Receiver { get; set; }
        public virtual TodoList TodoList { get; set; }
    }
}
