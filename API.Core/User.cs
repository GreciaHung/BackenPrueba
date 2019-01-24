using Api.Core.Helper;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Core
{
    public class User : ISoftDeleted
    {
        public User()
        {
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        
        public string Hash { get; set; }
        public string Salt { get; set; }
        public bool Deleted { get; set; }
    }
}