using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspTodo.Models.Dtos
{
    public class TodoListUpdateDto
    {
        [Required]
        public string ListID { get; set; }
        [Required]
        public string ListName { get; set; }
    }
}
