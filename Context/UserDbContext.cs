using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenBasedAuth_NetCore.Entities;
using TokenBasedAuth_NetCore.Entities.Trivia;

namespace TokenBasedAuth_NetCore.Context
{




    public class TriviaDbContext : DbContext
    {
        public TriviaDbContext(DbContextOptions<TriviaDbContext> options) : base(options)
        {

        }
        DbSet<Category> Categories { get; set; }

    }

    public partial class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }
      
    }

    public partial class UserDbContext : DbContext
    {
  
        DbSet<Customer> Customers { get; set; }
        DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

    

        }
    }
}
