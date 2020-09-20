using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenBasedAuth_NetCore.Entities;

namespace TokenBasedAuth_NetCore.Models
{
    public class TokenModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public string Name { get; set; }

        public string Token { get; set; }

        public TokenModel(User user, string token)
        {
            Id = user.Id;
            Name = user.Name;
            UserName = user.UserName;
            Token = token;
        }
    }
}
