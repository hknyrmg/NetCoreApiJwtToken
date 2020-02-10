using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TokenBasedAuth_NetCore.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "UserName is required", AllowEmptyStrings = false)]
        [StringLength(100)]
        public string UserName { get; set; }
        private DateTime createdDate;

        public DateTime? CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value ?? DateTime.UtcNow; }
        }
    }
}
