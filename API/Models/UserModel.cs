using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Core;

namespace API.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}