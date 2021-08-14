using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenBasedAuth_NetCore.Entities;
using TokenBasedAuth_NetCore.Entities.Trivia;

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
    public class TriviaDbContext : DbContext
    {
        public TriviaDbContext(DbContextOptions options) : base(options)
        {

        }
        DbSet<Category> Categories { get; set; }

    }
}
