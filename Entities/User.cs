using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TokenBasedAuth_NetCore.Entities
{
    public class User
    {
        [Key]

        public int Id { get; set; }
        [MaxLength(50)]

        [Required(ErrorMessage ="UserName is Required")]
        [MinLength(5, ErrorMessage = "UserName can't be less than 5 characters")]
        public string UserName { get; set; }
        public string Name { get; set; }

        [MaxLength(50)]

        public string SurName { get; set; }

        //[Required(ErrorMessage = "You must provide a phone number")]
        //[Display(Name = "Home Phone")]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        //public string PhoneNumber { get; set; }

        //public DateTime LastLogon { get; set; }
        //public DateTime CreatedOn { get; set; }
        public int ActivationCode { get; set; }

        [MaxLength(50)]

        public string Login { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [MinLength(5, ErrorMessage = "Password can't be less than 5 characters")]
        public string Password { get; set; }
        //public string Token { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime LastLogon = DateTime.Now;
        public DateTime CreatedOn { get; set; }
    
        public string Token { get; set; }
    }
}
