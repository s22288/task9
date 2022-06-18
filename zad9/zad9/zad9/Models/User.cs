using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace zad9
{
    public class User
    {
        [Key]
        public int IdPatient { get; set; }
        public string FristName { get; set; }
        public string LastName { get; set; }
        [Required]
        [MaxLength(100)]
        public string Password { get; set; }
    }
}
