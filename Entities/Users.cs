using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TokenBasedAuth_NetCore.Entities
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserId { get; set; }
        [Required(ErrorMessage = "UserName is required", AllowEmptyStrings = false)]
        [StringLength(100)]
        public string UserName { get; set; }
        private DateTime createdDate;

        public DateTime? CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value ?? DateTime.Now; }
        }


    }
}
