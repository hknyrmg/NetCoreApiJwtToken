using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenBasedAuth_NetCore.Entities;
using TokenBasedAuth_NetCore.Models;

namespace TokenBasedAuth_NetCore.Services
{
  public  interface IUserService
    {

        TokenModel Authenticate(Entities.LoginModel loginModel);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}
