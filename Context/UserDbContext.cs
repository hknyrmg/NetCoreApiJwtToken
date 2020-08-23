using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenBasedAuth_NetCore.Entities;

namespace TokenBasedAuth_NetCore.Context
{
    public class UserDbContext: DbContext
    {
        public UserDbContext(DbContextOptions options)  : base(options) 
        {
                
        }
        DbSet<Customer> Customers { get; set; }
        DbSet<User> Users { get; set; }

    }
}
