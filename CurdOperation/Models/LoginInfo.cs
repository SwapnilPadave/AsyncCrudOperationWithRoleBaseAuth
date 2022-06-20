using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CurdOperation.Models
{
    public class LoginInfo
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string Roles { get; private set; }
    }
}
