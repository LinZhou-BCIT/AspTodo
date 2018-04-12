using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspTodo.Models.Dtos
{
    public class UserInfoDto
    {
        [Required]
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
